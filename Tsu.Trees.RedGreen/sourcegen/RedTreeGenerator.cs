using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Tsu.Trees.RedGreen.SourceGenerator.Model;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class RedTreeGenerator
{
    public static void RegisterRedTreeOutput(this IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<Tree> trees)
    {
        context.RegisterSourceOutput(trees, (ctx, tree) =>
        {
            var builder = new StringBuilder();
            var writer = new IndentedTextWriter(new StringWriter(builder));

            writer.WriteLine("// <auto-generated />");
            writer.WriteLineNoTabs("");

            writer.WriteLine("#nullable enable");
            writer.WriteLineNoTabs("");

            writer.WriteLine("using System.Diagnostics;");
            writer.WriteLine("using System.Diagnostics.CodeAnalysis;");
            writer.WriteLine("using System.Linq;");
            writer.WriteLineNoTabs("");

            writer.WriteLine("namespace {0}", tree.RedBase.ContainingNamespace.ToCSharpString());
            writer.WriteLine('{');
            writer.Indent++;
            {
                writer.WriteRedRoot(tree, tree.Root);

                var stack = new Stack<Node>();
                foreach (var desc in tree.Root.Descendants)
                    stack.Push(desc);
                while (stack.Count > 0)
                {
                    var node = stack.Pop();
                    foreach (var desc in node.Descendants)
                        stack.Push(desc);

                    writer.WriteLineNoTabs("");
                    writer.WriteRedNode(tree, node);
                }
            }
            writer.Indent--;
            writer.WriteLine('}');
            writer.WriteLineNoTabs("");

            writer.Flush();
            ctx.AddSource($"{tree.Suffix}.Red.g.cs", builder.ToSourceText());
        });
    }

    public static void RegisterRedMainOutput(this IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<Tree> trees)
    {
        context.RegisterSourceOutput(trees, (ctx, tree) =>
        {
            var builder = new StringBuilder();
            var writer = new IndentedTextWriter(new StringWriter(builder));

            writer.WriteLine("// <auto-generated />");
            writer.WriteLineNoTabs("");

            writer.WriteLine("#nullable enable");
            writer.WriteLineNoTabs("");

            writer.WriteLine("using System.Diagnostics;");
            writer.WriteLine("using System.Diagnostics.CodeAnalysis;");
            writer.WriteLine("using System.Linq;");
            writer.WriteLineNoTabs("");

            writer.WriteLine("namespace {0}", tree.RedBase.ContainingNamespace.ToCSharpString());
            writer.WriteLine('{');
            writer.Indent++;
            {
                if (tree.CreateVisitors)
                {
                    writer.WriteVisitors(tree, tree.RedBase);
                    writer.WriteLineNoTabs("");
                }

                if (tree.CreateWalker)
                {
                    writer.WriteWalker(tree, tree.RedBase);
                    writer.WriteLineNoTabs("");
                }

                if (tree.CreateRewriter)
                {
                    writer.WriteLineNoTabs("");
                    writer.WriteRewriter(tree, tree.RedBase);
                }

                writer.WriteRedFactory(tree);
            }
            writer.Indent--;
            writer.WriteLine('}');
            writer.WriteLineNoTabs("");

            writer.Flush();
            ctx.AddSource($"{tree.Suffix}.Main.g.cs", builder.ToSourceText());
        });
    }

    private static void WriteRedRoot(this IndentedTextWriter writer, Tree tree, Node root)
    {
        writer.WriteLine("abstract partial class {0} : global::Tsu.Trees.RedGreen.IRedNode<{1}, {2}>",
            tree.RedBase.Name,
            tree.RedBase.ToCSharpString(),
            tree.KindEnum.ToCSharpString());
        writer.WriteLine('{');
        writer.Indent++;
        {
            writer.WriteLine("private readonly {0}? _parent;", tree.RedBase.ToCSharpString());
            writer.WriteLineNoTabs("");

            writer.WriteLines($$"""
                private protected {{tree.RedBase.Name}}({{tree.GreenBase.ToCSharpString()}} green, {{tree.RedBase.ToCSharpString()}}? parent)
                {
                    this._parent = parent;
                    this.Green = green;
                }

                """);

            foreach (var component in root.Components)
            {
                writer.WriteLine("public {0} {1} => this.Green.{1};", component.Type.ToCSharpString(), component.PropertyName);
            }
            writer.WriteLine("internal {0} Green {{ get; }}", tree.GreenBase.ToCSharpString());
            writer.WriteLine("public {0}? Parent => _parent;", tree.RedBase.ToCSharpString());
            writer.WriteLine("internal int SlotCount => this.Green.SlotCount;", tree.RedBase.ToCSharpString());
            writer.WriteLineNoTabs("");

            #region T GetRed<T>(ref T? field, int slot) where T : TRedRoot
            writer.WriteLines($$"""
                protected T? GetRed<T>(ref T? field, int index) where T : {{tree.RedBase.ToCSharpString()}}
                {
                    var result = field;

                    if (result == null)
                    {
                        var green = this.Green.GetSlot(index);
                        if (green != null)
                        {
                            global::System.Threading.Interlocked.CompareExchange(ref field, (T) green.CreateRed(this), null);
                            result = field;
                        }
                    }

                    return result;
                }

                """);
            #endregion T GetRed<T>(ref T? field, int slot) where T : TRedRoot

            #region bool IsEquivalentTo([NotNullWhen(true)] TRedRoot? other)
            writer.WriteLines($$"""
                public bool IsEquivalentTo({{tree.RedBase.ToCSharpString()}}? other)
                {
                    if (this == other) return true;
                    if (other == null) return false;

                    return this.Green.IsEquivalentTo(other.Green);
                }

                """);
            #endregion bool IsEquivalentTo([NotNullWhen(true)] TRedRoot? other)

            #region bool Contains(TRedRoot other)
            writer.WriteLines($$"""
                public bool Contains({{tree.RedBase.ToCSharpString()}} other)
                {
                    for (var node = other; node != null; node = node.Parent)
                    {
                        if (node == this)
                            return true;
                    }

                    return false;
                }

                """);
            #endregion bool Contains(TRedRoot other)

            writer.WriteLine("internal abstract {0}? GetNodeSlot(int index);", tree.RedBase.ToCSharpString());
            writer.WriteLineNoTabs("");

            #region TRedRoot GetRequiredNodeSlot(int slot)
            writer.WriteLines($$"""
                internal {{tree.RedBase.ToCSharpString()}} GetRequiredNodeSlot(int index)
                {
                    var node = this.GetNodeSlot(index);
                    Debug.Assert(node != null);
                    return node!;
                }

                """);
            #endregion TRedRoot GetRequiredNodeSlot(int slot)

            #region T Accept(Visitor visitor)
            writer.WriteAbstractAcceptMethods(tree, tree.RedBase.ContainingNamespace);
            #endregion T Accept(Visitor visitor)

            #region IEnumerable<TRedRoot> ChildNodes()
            writer.WriteLines($$"""
                public global::System.Collections.Generic.IEnumerable<{{tree.RedBase.ToCSharpString()}}> ChildNodes()
                {
                    var count = this.SlotCount;
                    for (var index = 0; index < count; index++)
                        yield return this.GetRequiredNodeSlot(index);
                }

                """);
            #endregion IEnumerable<TRedRoot> ChildNodes()

            #region IEnumerable<SyntaxNode> Ancestors
            writer.WriteLine("public global::System.Collections.Generic.IEnumerable<{0}> Ancestors() =>", tree.RedBase.ToCSharpString());
            writer.Indent++;
            writer.WriteLine("this.Parent?.AncestorsAndSelf() ?? global::System.Linq.Enumerable.Empty<{0}>();", tree.RedBase.ToCSharpString());
            writer.Indent--;
            writer.WriteLineNoTabs("");
            #endregion IEnumerable<SyntaxNode> Ancestors

            #region IEnumerable<TRedRoot> AncestorsAndSelf()
            writer.WriteLines($$"""
                public global::System.Collections.Generic.IEnumerable<{{tree.RedBase.ToCSharpString()}}> AncestorsAndSelf()
                {
                    for (var node = this; node != null; node = node.Parent)
                        yield return node;
                }

                """);
            #endregion IEnumerable<TRedRoot> AncestorsAndSelf()

            #region TNode? FirstAncestorOrSelf<TNode>(Func<TNode, bool>? predicate = null) where TNode : TRedRoot
            writer.WriteLines($$"""
                public TNode? FirstAncestorOrSelf<TNode>(Func<TNode, bool>? predicate = null) where TNode : {{tree.RedBase.ToCSharpString()}}
                {
                    for (var node = this; node != null; node = node.Parent)
                    {
                        if (node is TNode tnode && (predicate == null || predicate(tnode)))
                            return tnode;
                    }

                    return null;
                }

                """);
            #endregion TNode? FirstAncestorOrSelf<TNode>(Func<TNode, bool>? predicate = null) where TNode : TRedRoot

            #region TNode? FirstAncestorOrSelf<TNode, TArg>(Func<TNode, TArg, bool> predicate, TArg argument) where TNode : TRedRoot
            writer.WriteLines($$"""
                public TNode? FirstAncestorOrSelf<TNode, TArg>(Func<TNode, TArg, bool> predicate, TArg argument) where TNode : {{tree.RedBase.ToCSharpString()}}
                {
                    for (var node = this; node != null; node = node.Parent)
                    {
                        if (node is TNode tnode && (predicate == null || predicate(tnode, argument)))
                            return tnode;
                    }

                    return null;
                }

                """);
            #endregion TNode? FirstAncestorOrSelf<TNode, TArg>(Func<TNode, TArg, bool> predicate, TArg argument) where TNode : TRedRoot

            #region IEnumerable<TRedRoot> DescendantNodes(Func<TRedRoot, bool>? descendIntoChildren = null)
            writer.WriteLines($$"""
                public global::System.Collections.Generic.IEnumerable<{{tree.RedBase.ToCSharpString()}}> DescendantNodes(Func<{{tree.RedBase.ToCSharpString()}}, bool>? descendIntoChildren = null)
                {
                    var stack = new Stack<{{tree.RedBase.ToCSharpString()}}>(24);
                    foreach (var child in this.ChildNodes())
                        stack.Push(child);

                    while (stack.Count > 0)
                    {
                        var current = stack.Pop();

                        yield return current;

                        foreach (var child in current.ChildNodes().Reverse())
                        {
                            stack.Push(child);
                        }
                    }
                }

                """);
            #endregion IEnumerable<TRedRoot> DescendantNodes(Func<TRedRoot, bool>? descendIntoChildren = null)

            #region IEnumerable<TRedRoot> DescendantNodesAndSelf(Func<TRedRoot, bool>? descendIntoChildren = null)
            writer.WriteLines($$"""
                public global::System.Collections.Generic.IEnumerable<{{tree.RedBase.ToCSharpString()}}> DescendantNodesAndSelf(Func<{{tree.RedBase.ToCSharpString()}}, bool>? descendIntoChildren = null)
                {
                    var stack = new Stack<{{tree.RedBase.ToCSharpString()}}>(24);
                    stack.Push(this);

                    while (stack.Count > 0)
                    {
                        var current = stack.Pop();

                        yield return current;

                        foreach (var child in current.ChildNodes().Reverse())
                        {
                            stack.Push(child);
                        }
                    }
                }
                """);
            #endregion IEnumerable<TRedRoot> DescendantNodesAndSelf(Func<TRedRoot, bool>? descendIntoChildren = null)
        }
        writer.Indent--;
        writer.WriteLine('}');
    }

    private static void WriteRedNode(this IndentedTextWriter writer, Tree tree, Node node)
    {
        var nodeBase = node.BaseSymbol;
        if (SymbolEqualityComparer.Default.Equals(nodeBase, tree.GreenBase))
            nodeBase = tree.RedBase;

        writer.WriteLine("{0} {1} partial class {2} : {3}.{4}",
            tree.RedBase.DeclaredAccessibility.ToCSharpString(),
            node.Descendants.Length > 0 ? "abstract" : "sealed",
            node.TypeSymbol.Name,
            tree.RedBase.ContainingNamespace.ToCSharpString(false),
            nodeBase!.Name);
        writer.WriteLine('{');
        writer.Indent++;
        {
            foreach (var child in node.Children)
            {
                writer.WriteLine("private {0}.{1}? {2};",
                    tree.RedBase.ContainingNamespace.ToCSharpString(false),
                    child.Type.Name,
                    child.FieldName);
            }
            writer.WriteLineNoTabs("");

            writer.WriteLines($$"""
                internal {{node.TypeSymbol.Name}}({{tree.GreenBase.ToCSharpString()}} green, {{tree.RedBase.ToCSharpString()}}? parent)
                    : base(green, parent)
                {
                }

                """);

            #region Components
            if (node.Descendants.Length > 0)
            {
                foreach (var component in node.NodeComponents)
                {
                    writer.WriteLine($"public abstract {component.Type.ToCSharpString()} {component.PropertyName};");
                }
            }
            else
            {
                foreach (var extra in node.ExtraData)
                {
                    if (extra.FieldName == "_kind") continue;

                    writer.WriteLine("public {0}{1} {2} => (({3})this.Green).{2};",
                        extra.PassToBase ? " override" : "",
                        extra.Type.ToCSharpString(),
                        extra.PropertyName,
                        node.TypeSymbol.ToCSharpString(false));
                }
                for (var idx = 0; idx < node.Children.Length; idx++)
                {
                    var child = node.Children[idx];
                    writer.WriteLine("public {0}{1}.{2} {3} => GetRed(ref this.{4}, {5}){6};",
                        child.PassToBase ? " override" : "",
                        tree.RedBase.ContainingNamespace.ToCSharpString(false),
                        child.Type.Name + (child.IsOptional ? "?" : ""),
                        child.PropertyName,
                        child.FieldName,
                        idx,
                        child.IsOptional ? "" : "!");
                }
            }
            writer.WriteLineNoTabs("");
            #endregion Components

            #region TRedRoot? GetNodeSlot(int index)
            if (node.Descendants.Length == 0)
            {
                writer.WriteLine("internal override {0}? GetNodeSlot(int index) =>", tree.RedBase.ToCSharpString());
                writer.Indent++;
                if (node.Children.Length == 1)
                {
                    writer.Indent++;
                    writer.WriteLine("index == 1 ? GetRed(ref this.{0}, 1){1} : null;",
                        node.Children[0].FieldName,
                        node.Children[0].IsOptional ? "" : "!");
                    writer.Indent--;
                }
                else
                {
                    writer.WriteLine("index switch");
                    writer.WriteLine('{');
                    writer.Indent++;
                    {
                        for (var idx = 0; idx < node.Children.Length; idx++)
                        {
                            var child = node.Children[idx];
                            writer.WriteLine("{0} => GetRed(ref this.{1}, {0}){2},", idx, child.FieldName, child.IsOptional ? "" : "!");
                        }
                        writer.WriteLine("_ => null");
                    }
                    writer.Indent--;
                    writer.WriteLine("};");
                }
                writer.Indent--;
            }
            #endregion TRedRoot? GetNodeSlot(int index)

            #region T Accept(Visitor visitor)
            if (node.Descendants.Length == 0)
            {
                writer.WriteOverrideAcceptMethods(tree, tree.RedBase.ContainingNamespace, node);
            }
            #endregion T Accept(Visitor visitor)

            #region TNode Update(...)
            if (node.Descendants.Length == 0 && node.RequiredComponents.Any())
            {
                writer.WriteLineNoTabs("");
                writer.Write("public {0}.{1} Update(", tree.RedBase.ContainingNamespace.ToCSharpString(false), node.TypeSymbol.Name);
                var first = true;
                foreach (var component in node.RequiredComponents)
                {
                    if (!first) writer.Write(", ");
                    first = false;

                    var ns = component.Type.ContainingNamespace;
                    var name = component.Type.Name;
                    if (component.Type.DerivesFrom(tree.GreenBase))
                    {
                        ns = tree.RedBase.ContainingNamespace;
                    }

                    writer.Write("{0}.{1}{2} {3}",
                        ns.ToCSharpString(false),
                        name,
                        component.IsOptional ? "?" : "",
                        component.ParameterName);
                }
                writer.WriteLine(')');
                writer.WriteLine('{');
                writer.Indent++;
                {
                    writer.Write("if (");
                    first = true;
                    foreach (var component in node.RequiredComponents)
                    {
                        if (!first) writer.Write(" && ");
                        first = false;
                        writer.Write("{0} != this.{1}", component.ParameterName, component.PropertyName);
                    }
                    writer.WriteLine(')');
                    writer.WriteLine('{');
                    writer.Indent++;
                    {
                        writer.Write("return global::{0}.{1}Factory.{2}(",
                            tree.RedBase.ContainingNamespace.ToCSharpString(),
                            tree.Suffix,
                            node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));
                        first = true;
                        foreach (var component in node.RequiredComponents)
                        {
                            if (!first) writer.Write(", ");
                            first = false;
                            writer.Write(component.ParameterName);
                        }
                        writer.WriteLine(");");
                    }
                    writer.Indent--;
                    writer.WriteLine('}');
                    writer.WriteLineNoTabs("");
                    writer.WriteLine("return this;");
                }
                writer.Indent--;
                writer.WriteLine('}');
            }
            #endregion TNode Update(...)

            #region TNode WithX(...)
            if (node.Descendants.Length == 0 && node.RequiredComponents.Any())
            {
                var components = node.RequiredComponents.ToImmutableArray();
                for (var targetIdx = 0; targetIdx < components.Length; targetIdx++)
                {
                    var target = components[targetIdx];
                    var isNode = target.Type.DerivesFrom(tree.GreenBase);

                    writer.WriteLineNoTabs("");
                    writer.Write("public {0}.{1} With{2}({3}.{4}{5} {6}) => this.Update(",
                        tree.RedBase.ContainingNamespace.ToCSharpString(false),
                        node.TypeSymbol.Name,
                        target.PropertyName,
                        (isNode ? tree.RedBase.ContainingNamespace : target.Type.ContainingNamespace).ToCSharpString(false),
                        target.Type.Name,
                        target.IsOptional ? "?" : "",
                        target.ParameterName);
                    var first = true;
                    for (var idx = 0; idx < components.Length; idx++)
                    {
                        var component = components[idx];
                        if (!first) writer.Write(", ");
                        first = false;

                        if (targetIdx == idx) writer.Write(component.ParameterName);
                        else writer.Write("this.{0}", component.PropertyName);
                    }
                    writer.WriteLine(");");
                }
            }
            #endregion TNode WithX(...)
        }
        writer.Indent--;
        writer.WriteLine('}');
    }

    private static void WriteRedFactory(this IndentedTextWriter writer, Tree tree)
    {
        writer.WriteLine("{0} static class {1}Factory", tree.RedBase.DeclaredAccessibility.ToCSharpString(), tree.Suffix);
        writer.WriteLine('{');
        writer.Indent++;
        {
            var queue = new Queue<Node>();
            foreach (var desc in tree.Root.Descendants)
                queue.Enqueue(desc);

            var first = true;
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
                    if (!first) writer.WriteLineNoTabs("");
                    first = false;
                    writer.WriteRedFactoryMethods(tree, node);
                }
            }
        }
        writer.Indent--;
        writer.WriteLine('}');
    }

    private static void WriteRedFactoryMethods(this IndentedTextWriter writer, Tree tree, Node node)
    {
        if (node.RequiredComponents.Any(x => x.IsOptional))
        {
            writeMethod(writer, tree, node, false);
            writer.WriteLineNoTabs("");
        }
        writeMethod(writer, tree, node, true);

        static void writeMethod(IndentedTextWriter writer, Tree tree, Node node, bool includeOptional)
        {
            writer.Write("public static {0}.{1} {2}(",
                tree.RedBase.ContainingNamespace.ToCSharpString(false),
                node.TypeSymbol.Name,
                node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));

            var first = true;
            foreach (var component in node.RequiredComponents)
            {
                if (!includeOptional && component.IsOptional) continue;
                if (!first) writer.Write(", ");
                first = false;

                var ns = component.Type.ContainingNamespace;
                if (component.Type.DerivesFrom(tree.GreenBase))
                    ns = tree.RedBase.ContainingNamespace;

                writer.Write("{0}.{1}{2} {3}",
                    ns.ToCSharpString(false),
                    component.Type.Name,
                    component.IsOptional ? "?" : "",
                    component.ParameterName);
            }
            writer.WriteLine(") =>");
            writer.Indent++;
            writer.Write("({0}.{1}) {2}.{3}Factory.{4}(",
                tree.RedBase.ContainingNamespace.ToCSharpString(false),
                node.TypeSymbol.Name,
                tree.GreenBase.ContainingNamespace.ToCSharpString(false),
                tree.Suffix,
                node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));

            first = true;
            foreach (var component in node.RequiredComponents)
            {
                if (!includeOptional && component.IsOptional) continue;
                if (!first) writer.Write(", ");
                first = false;

                if (component.Type.DerivesFrom(tree.GreenBase))
                {
                    if (component.IsOptional)
                    {
                        writer.Write("{0} == null ? null : ({1}){0}.Green",
                            component.ParameterName,
                            component.Type.ToCSharpString(false));
                    }
                    else
                    {
                        writer.Write("({0}){1}.Green",
                            component.Type.ToCSharpString(false),
                            component.ParameterName);
                    }
                }
                else
                {
                    writer.Write(component.ParameterName);
                }
            }
            writer.WriteLine(").CreateRed();");
            writer.Indent--;
        }
    }
}