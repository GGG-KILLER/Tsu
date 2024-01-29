using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;
using Tsu.Trees.RedGreen.SourceGenerator.Model;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class VisitorGenerator
{
    public static void WriteAbstractAcceptMethods(this IndentedTextWriter writer, Tree tree, INamespaceSymbol ns)
    {
        if (tree.CreateVisitors || tree.CreateWalker)
        {
            writer.WriteLine("public abstract void Accept({0}.{1}Visitor visitor);",
                ns.ToCSharpString(true),
                tree.Suffix);
        }
        if (tree.CreateVisitors || tree.CreateRewriter)
        {
            writer.WriteLine("public abstract TResult? Accept<TResult>({0}.{1}Visitor<TResult> visitor);",
                ns.ToCSharpString(true),
                tree.Suffix);
        }
        if (tree.CreateVisitors)
        {
            writer.WriteLine("public abstract TResult? Accept<T1, TResult>({0}.{1}Visitor<T1, TResult> visitor, T1 arg1);",
                ns.ToCSharpString(true),
                tree.Suffix);
            writer.WriteLine("public abstract TResult? Accept<T1, T2, TResult>({0}.{1}Visitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2);",
                ns.ToCSharpString(true),
                tree.Suffix);
            writer.WriteLine("public abstract TResult? Accept<T1, T2, T3, TResult>({0}.{1}Visitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3);",
                ns.ToCSharpString(true),
                tree.Suffix);
        }
    }

    public static void WriteOverrideAcceptMethods(this IndentedTextWriter writer, Tree tree, INamespaceSymbol ns, Node node)
    {
        if (tree.CreateVisitors || tree.CreateWalker)
        {
            writer.WriteLine("[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]");
            writer.WriteLine("public override void Accept({0}.{1}Visitor visitor) => visitor.Visit{2}(this);",
                ns.ToCSharpString(true),
                tree.Suffix,
                node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));
        }
        if (tree.CreateVisitors || tree.CreateRewriter)
        {
            writer.WriteLine("[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]");
            writer.WriteLine("public override TResult Accept<TResult>({0}.{1}Visitor<TResult> visitor) => visitor.Visit{2}(this);",
                ns.ToCSharpString(true),
                tree.Suffix,
                node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));
        }
        if (tree.CreateVisitors)
        {
            writer.WriteLine("[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]");
            writer.WriteLine("public override TResult Accept<T1, TResult>({0}.{1}Visitor<T1, TResult> visitor, T1 arg1) => visitor.Visit{2}(this, arg1);",
                ns.ToCSharpString(true),
                tree.Suffix,
                node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));
            writer.WriteLine("[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]");
            writer.WriteLine("public override TResult Accept<T1, T2, TResult>({0}.{1}Visitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) => visitor.Visit{2}(this, arg1, arg2);",
                ns.ToCSharpString(true),
                tree.Suffix,
                node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));
            writer.WriteLine("[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]");
            writer.WriteLine("public override TResult Accept<T1, T2, T3, TResult>({0}.{1}Visitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) => visitor.Visit{2}(this, arg1, arg2, arg3);",
                ns.ToCSharpString(true),
                tree.Suffix,
                node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));
        }
    }

    public static void WriteVisitors(this IndentedTextWriter writer, Tree tree, INamedTypeSymbol baseType)
    {
        for (var arity = 0; arity < 5; arity++)
        {
            writer.WriteLineNoTabs("");
            writer.WriteVisitor(tree, baseType, arity);
        }
    }

    public static void WriteVisitor(this IndentedTextWriter writer, Tree tree, INamedTypeSymbol baseType, int arity)
    {
        writer.Write("{0} partial class {1}Visitor", baseType.DeclaredAccessibility.ToCSharpString(), tree.Suffix);
        writer.Write(arity switch
        {
            0 => "",
            1 => "<TResult>",
            2 => "<T1, TResult>",
            3 => "<T1, T2, TResult>",
            4 => "<T1, T2, T3, TResult>",
            _ => throw new ArgumentOutOfRangeException(nameof(arity))
        });
        writer.WriteLine();
        writer.WriteLine('{');
        writer.Indent++;
        {
            var queue = new Queue<Node>();
            foreach (var desc in tree.Root.Descendants)
                queue.Enqueue(desc);

            writer.Write("public virtual {0} Visit({1}? node", arity > 0 ? "TResult?" : "void", baseType.ToCSharpString(false));
            if (arity > 1) writer.Write(", T1 arg1");
            if (arity > 2) writer.Write(", T2 arg2");
            if (arity > 3) writer.Write(", T3 arg3");
            writer.Write(")");
            if (arity > 0)
            {
                writer.WriteLine(" => node == null ? default : node.Accept(this");
                if (arity > 1) writer.Write(", arg1");
                if (arity > 2) writer.Write(", arg2");
                if (arity > 3) writer.Write(", arg3");
                writer.WriteLine(");");
            }
            else
            {
                writer.WriteLine();
                writer.WriteLine('{');
                writer.Indent++;
                {
                    writer.WriteLine("if (node != null)");
                    writer.WriteLine('{');
                    writer.Indent++;
                    {
                        writer.Write("node.Accept(this");
                        if (arity > 1) writer.Write(", arg1");
                        if (arity > 2) writer.Write(", arg2");
                        if (arity > 3) writer.Write(", arg3");
                        writer.WriteLine(");");
                    }
                    writer.Indent--;
                    writer.WriteLine('}');
                }
                writer.Indent--;
                writer.WriteLine('}');
            }

            #region Node Visit Methods
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node.Descendants.Any())
                {
                    foreach (var desc in node.Descendants)
                        queue.Enqueue(desc);
                }
                else
                {
                    writer.Write("public virtual {0} Visit{1}({2}.{3} node",
                        arity > 0 ? "TResult?" : "void",
                        node.TypeSymbol.Name.WithoutSuffix(tree.Suffix),
                        baseType.ContainingNamespace.ToCSharpString(false),
                        node.TypeSymbol.Name);
                    if (arity > 1) writer.Write(", T1 arg1");
                    if (arity > 2) writer.Write(", T2 arg2");
                    if (arity > 3) writer.Write(", T3 arg3");
                    writer.Write(") => this.DefaultVisit(node");
                    if (arity > 1) writer.Write(", arg1");
                    if (arity > 2) writer.Write(", arg2");
                    if (arity > 3) writer.Write(", arg3");
                    writer.WriteLine(");");
                }
            }
            #endregion Node Visit Methods

            writer.Write("protected virtual {0} DefaultVisit({1} node", arity > 0 ? "TResult?" : "void", baseType.ToCSharpString(false));
            if (arity > 1) writer.Write(", T1 arg1");
            if (arity > 2) writer.Write(", T2 arg2");
            if (arity > 3) writer.Write(", T3 arg3");
            writer.Write(")");
            if (arity == 0) writer.WriteLine(" { }");
            else writer.WriteLine(" => default;");
        }
        writer.Indent--;
        writer.WriteLine('}');
    }

    public static void WriteWalker(this IndentedTextWriter writer, Tree tree, INamedTypeSymbol baseType)
    {
        if (!tree.CreateVisitors)
        {
            writer.WriteVisitor(tree, baseType, 0);
            writer.WriteLineNoTabs("");
        }

        writer.WriteLines($$"""
            {{baseType.DeclaredAccessibility.ToCSharpString()}} abstract class {{tree.Suffix}}Walker : {{baseType.ContainingNamespace.ToCSharpString(false)}}.{{tree.Suffix}}Visitor
            {
                private int _recursionDepth;

                public override void Visit({{baseType.ToCSharpString()}}? node)
                {
                    if (node != null)
                    {
                        _recursionDepth++;
                        if (_recursionDepth > 30)
                        {
                            global::System.Runtime.CompilerServices.RuntimeHelpers.EnsureSufficientExecutionStack();
                        }

                        node.Accept(this);

                        _recursionDepth--;
                    }
                }

                protected override void DefaultVisit({{baseType.ToCSharpString()}} node)
                {
                    foreach (var child in node.ChildNodes())
                    {
                        Visit(child);
                    }
                }
            }
            """);
    }

    public static void WriteRewriter(this IndentedTextWriter writer, Tree tree, INamedTypeSymbol baseType)
    {
        if (!tree.CreateVisitors)
        {
            writer.WriteVisitor(tree, baseType, 1);
            writer.WriteLineNoTabs("");
        }

        writer.WriteLine("{0} partial class {1}Rewriter : {2}.{1}Visitor<{3}>",
            baseType.DeclaredAccessibility.ToCSharpString(),
            tree.Suffix,
            baseType.ContainingNamespace.ToCSharpString(true),
            baseType.ToCSharpString(false));
        writer.WriteLine('{');
        writer.Indent++;
        {
            var queue = new Queue<Node>();
            foreach (var desc in tree.Root.Descendants)
                queue.Enqueue(desc);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node.Descendants.Any())
                {
                    foreach (var desc in node.Descendants)
                        queue.Enqueue(desc);
                }
                else
                {
                    writer.WriteLine("public override {0} Visit{1}({2}.{3} node) =>",
                        baseType.ToCSharpString(),
                        node.TypeSymbol.Name.WithoutSuffix(tree.Suffix),
                        baseType.ContainingNamespace.ToCSharpString(false),
                        node.TypeSymbol.Name);
                    writer.Indent++;
                    {
                        writer.Write("node.Update(");
                        var first = true;
                        foreach (var component in node.RequiredComponents)
                        {
                            if (!first) writer.Write(", ");
                            first = false;

                            if (component.Type.DerivesFrom(tree.GreenBase))
                            {
                                writer.Write("({0}.{1}?)Visit(node.{2})",
                                    baseType.ContainingNamespace.ToCSharpString(false),
                                    component.Type.Name,
                                    component.PropertyName);
                                if (!component.IsOptional)
                                {
                                    writer.Write(" ?? throw new global::System.InvalidOperationException(\"{0} cannot be null.\")", component.PropertyName);
                                }
                            }
                            else
                            {
                                writer.Write("node.{0}", component.PropertyName);
                            }
                        }
                        writer.WriteLine(");");
                    }
                    writer.Indent--;
                }
            }
        }
        writer.Indent--;
        writer.WriteLine('}');
    }
}