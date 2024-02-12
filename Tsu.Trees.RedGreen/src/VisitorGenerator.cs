// Copyright © 2024 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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
                ns.ToCSharpString(false),
                tree.Suffix);
        }
        if (tree.CreateVisitors || tree.CreateRewriter)
        {
            writer.WriteLine("public abstract TResult? Accept<TResult>({0}.{1}Visitor<TResult> visitor);",
                ns.ToCSharpString(false),
                tree.Suffix);
        }
        if (tree.CreateVisitors)
        {
            writer.WriteLine("public abstract TResult? Accept<T1, TResult>({0}.{1}Visitor<T1, TResult> visitor, T1 arg1);",
                ns.ToCSharpString(false),
                tree.Suffix);
            writer.WriteLine("public abstract TResult? Accept<T1, T2, TResult>({0}.{1}Visitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2);",
                ns.ToCSharpString(false),
                tree.Suffix);
            writer.WriteLine("public abstract TResult? Accept<T1, T2, T3, TResult>({0}.{1}Visitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3);",
                ns.ToCSharpString(false),
                tree.Suffix);
        }
    }

    public static void WriteOverrideAcceptMethods(this IndentedTextWriter writer, Tree tree, INamespaceSymbol ns, Node node)
    {
        if (tree.CreateVisitors || tree.CreateWalker)
        {
            writer.WriteLine("[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]");
            writer.WriteLine("public override void Accept({0}.{1}Visitor visitor) => visitor.Visit{2}(this);",
                ns.ToCSharpString(false),
                tree.Suffix,
                node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));
        }
        if (tree.CreateVisitors || tree.CreateRewriter)
        {
            writer.WriteLine("[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]");
            writer.WriteLine("public override TResult Accept<TResult>({0}.{1}Visitor<TResult> visitor) => visitor.Visit{2}(this);",
                ns.ToCSharpString(false),
                tree.Suffix,
                node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));
        }
        if (tree.CreateVisitors)
        {
            writer.WriteLine("[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]");
            writer.WriteLine("public override TResult Accept<T1, TResult>({0}.{1}Visitor<T1, TResult> visitor, T1 arg1) => visitor.Visit{2}(this, arg1);",
                ns.ToCSharpString(false),
                tree.Suffix,
                node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));
            writer.WriteLine("[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]");
            writer.WriteLine("public override TResult Accept<T1, T2, TResult>({0}.{1}Visitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) => visitor.Visit{2}(this, arg1, arg2);",
                ns.ToCSharpString(false),
                tree.Suffix,
                node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));
            writer.WriteLine("[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]");
            writer.WriteLine("public override TResult Accept<T1, T2, T3, TResult>({0}.{1}Visitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) => visitor.Visit{2}(this, arg1, arg2, arg3);",
                ns.ToCSharpString(false),
                tree.Suffix,
                node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));
        }
    }

    public static void WriteVisitors(this IndentedTextWriter writer, Tree tree, bool isGreen)
    {
        for (var arity = 0; arity < 5; arity++)
        {
            writer.WriteLineNoTabs("");
            writer.WriteVisitor(tree, isGreen, arity);
        }
    }

    public static void WriteVisitor(this IndentedTextWriter writer, Tree tree, bool isGreen, int arity)
    {
        var baseType = isGreen ? tree.GreenBase : tree.RedBase;
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
                    writer.Write("public virtual {0} Visit{1}({2} node",
                        arity > 0 ? "TResult?" : "void",
                        node.TypeSymbol.Name.WithoutSuffix(tree.Suffix),
                        isGreen ? node.TypeSymbol.ToCSharpString(false) : tree.ToRedCSharp(node.TypeSymbol, false));
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

    public static void WriteWalker(this IndentedTextWriter writer, Tree tree, bool isGreen)
    {
        if (!tree.CreateVisitors)
        {
            writer.WriteVisitor(tree, isGreen, 0);
        }
    }

    public static void WriteRewriter(this IndentedTextWriter writer, Tree tree, bool isGreen)
    {
        if (!tree.CreateVisitors)
        {
            writer.WriteVisitor(tree, isGreen, 1);
            writer.WriteLineNoTabs("");
        }

        var baseType = isGreen ? tree.GreenBase : tree.RedBase;
        writer.WriteLine("{0} partial class {1}Rewriter : {2}.{1}Visitor<{3}>",
            baseType.DeclaredAccessibility.ToCSharpString(),
            tree.Suffix,
            baseType.ContainingNamespace.ToCSharpString(false),
            baseType.ToCSharpString(false));
        writer.WriteLine('{');
        writer.Indent++;
        {
            var baseTypeNs = baseType.ContainingNamespace.ToCSharpString(false);
            writer.WriteLines($$"""
            public {{baseTypeNs}}.{{tree.Suffix}}List<TNode> VisitList<TNode>({{baseTypeNs}}.{{tree.Suffix}}List<TNode> list) where TNode : {{baseType.ToCSharpString(false)}}
            {
                {{baseTypeNs}}.{{tree.Suffix}}ListBuilder? alternate = null;
                for (int i = 0, n = list.Count; i < n; i++)
                {
                    var item = list[i];
                    var visited = Visit(item);
                    if (item != visited && alternate == null)
                    {
                        alternate = new {{baseTypeNs}}.{{tree.Suffix}}ListBuilder(n);
                        alternate.AddRange(list, 0, i);
                    }

                    if (alternate != null && visited != null && visited.Kind != {{tree.KindEnum.ToCSharpString(false)}}.None)
                    {
                        alternate.Add(visited);
                    }
                }

                if (alternate != null)
                {
                    return alternate.ToList();
                }

                return list;
            }

            """);
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
                    writer.WriteLine("public override {0} Visit{1}({2} node) =>",
                        baseType.ToCSharpString(),
                        node.TypeSymbol.Name.WithoutSuffix(tree.Suffix),
                        isGreen ? node.TypeSymbol.ToCSharpString(false) : tree.ToRedCSharp(node.TypeSymbol, false));
                    writer.Indent++;
                    {
                        if (!node.RequiredComponents.Any())
                        {
                            writer.WriteLine("node;");
                        }
                        else
                        {
                            writer.Write("node.Update(");
                            var first = true;
                            foreach (var component in node.RequiredComponents)
                            {
                                if (!first) writer.Write(", ");
                                first = false;

                                if (component.Type.DerivesFrom(tree.GreenBase))
                                {
                                    if (component.IsList)
                                    {
                                        writer.Write("VisitList(node.{0})", component.PropertyName);
                                    }
                                    else
                                    {
                                        writer.Write("({0}?)Visit(node.{1})",
                                        isGreen ? component.Type.ToCSharpString(false) : tree.ToRedCSharp(component.Type, false),
                                        component.PropertyName);
                                        if (!component.IsOptional)
                                        {
                                            writer.Write(" ?? throw new global::System.InvalidOperationException(\"{0} cannot be null.\")", component.PropertyName);
                                        }
                                    }
                                }
                                else
                                {
                                    writer.Write("node.{0}", component.PropertyName);
                                }
                            }
                            writer.WriteLine(");");
                        }
                    }
                    writer.Indent--;
                }
            }
        }
        writer.Indent--;
        writer.WriteLine('}');
    }
}