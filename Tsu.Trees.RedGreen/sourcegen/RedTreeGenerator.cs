using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis;
using Tsu.Trees.RedGreen.SourceGenerator.Model;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class RedTreeGenerator
{
    public static void RegisterRedSyntaxOutput(this IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<Tree> trees)
    {
        context.RegisterSourceOutput(trees, (ctx, tree) =>
        {
            try
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
                }

                writer.Indent--;
                writer.WriteLine('}');
                writer.WriteLineNoTabs("");

                writer.Flush();
                ctx.AddSource($"{tree.Suffix}.Red.g.cs", builder.ToSourceText());
            }
            catch (Exception ex)
            {
                D.WriteLine(ex.ToString());
            }
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
    }
}