using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tsu.Trees.RedGreen.SourceGenerator.Model;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class GreenTreeGenerator
{
    public static void ValidateTrees(this IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<Tree> trees)
    {
        // TODO: Implement
        return;
    }

    public static void RegisterGreenOutput(this IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<Tree> trees)
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

            writer.WriteLine("namespace {0}", tree.GreenBase.ContainingNamespace.ToCSharpString());
            writer.WriteLine('{');
            writer.Indent++;

            writer.WriteGreenRoot(tree, tree.Root);

            var queue = new Queue<Node>();
            foreach (var node in tree.Root.Descendants)
                queue.Enqueue(node);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                foreach (var descendant in node.Descendants)
                    queue.Enqueue(descendant);
                writer.WriteLineNoTabs("");
                writer.WriteGreenNode(tree, node);
            }

            writer.WriteLineNoTabs("");
            writer.WriteGreenFactory(tree);

            writer.Indent--;
            writer.WriteLine('}');
            writer.WriteLineNoTabs("");

            writer.Flush();
            ctx.AddSource($"{tree.Suffix}.Internal.g.cs", builder.ToSourceText());
        });
    }

    private static void WriteGreenRoot(this IndentedTextWriter writer, Tree tree, Node root)
    {
        writer.WriteLine("abstract partial class {0} : global::Tsu.Trees.RedGreen.Internal.IGreenNode<{1}, {2}, {3}>",
            root.TypeSymbol.Name,
            tree.GreenBase.ToCSharpString(false),
            tree.RedBase.ToCSharpString(false),
            tree.KindEnum.ToCSharpString(false));
        writer.WriteLine('{');
        writer.Indent++;
        {
            writer.WriteLine("private readonly {0} _kind;", tree.KindEnum.ToCSharpString());
            writer.WriteLine("private byte _slotCount;");
            writer.WriteLineNoTabs("");

            writer.WriteGreenConstructor(root);
            writer.WriteLineNoTabs("");

            foreach (var component in root.Components)
            {
                writer.WriteLine("public {0} {1} => this.{2};", component.Type.ToCSharpString(), component.PropertyName, component.FieldName);
            }
            writer.WriteLineNoTabs("");

            #region int SlotCount
            writer.WriteLine("public int SlotCount");
            writer.WriteLine('{');
            writer.Indent++;
            {
                writer.WriteLine("get");
                writer.WriteLine('{');
                writer.Indent++;
                {
                    writer.WriteLine("int count = this._slotCount;");
                    writer.WriteLine("if (count == byte.MaxValue)");
                    writer.Indent++;
                    writer.WriteLine("count = this.GetSlotCount();");
                    writer.Indent--;
                    writer.WriteLine("return count;");
                }
                writer.Indent--;
                writer.WriteLine('}');
                writer.WriteLine("protected set => _slotCount = (byte) value;");
            }
            writer.Indent--;
            writer.WriteLine('}');
            writer.WriteLineNoTabs("");
            #endregion int SlotCount

            writer.WriteLine("public abstract {0}? GetSlot(int index);", tree.GreenBase.ToCSharpString());
            writer.WriteLineNoTabs("");

            #region TGreenRoot GetRequiredSlot(int index)
            writer.WriteLine("public {0} GetRequiredSlot(int index)", tree.GreenBase.ToCSharpString());
            writer.WriteLine('{');
            writer.Indent++;
            {
                writer.WriteLine("var node = this.GetSlot(index);");
                writer.WriteLine("Debug.Assert(node != null);");
                writer.WriteLine("return node!;");
            }
            writer.Indent--;
            writer.WriteLine('}');
            writer.WriteLineNoTabs("");
            #endregion TGreenRoot GetRequiredSlot(int index)

            writer.WriteLine("protected virtual int GetSlotCount() => _slotCount;");
            writer.WriteLineNoTabs("");

            #region IEnumerable<TGreenRoot> ChildNodes()
            writer.WriteLine("public global::System.Collections.Generic.IEnumerable<{0}> ChildNodes()", tree.GreenBase.ToCSharpString());
            writer.WriteLine('{');
            writer.Indent++;
            {
                writer.WriteLine("var count = this.SlotCount;");
                writer.WriteLine("for (var index = 0; index < count; index++)");
                writer.Indent++;
                writer.WriteLine("yield return this.GetRequiredSlot(index);");
                writer.Indent--;
            }
            writer.Indent--;
            writer.WriteLine('}');
            writer.WriteLineNoTabs("");
            #endregion IEnumerable<TGreenRoot> ChildNodes()

            #region IEnumerable<TGreenRoot> EnumerateDescendants()
            writer.WriteLine("public global::System.Collections.Generic.IEnumerable<{0}> EnumerateDescendants()", tree.GreenBase.ToCSharpString());
            writer.WriteLine('{');
            writer.Indent++;
            {
                writer.WriteLine("var stack = new Stack<{0}>(24);", tree.GreenBase.ToCSharpString());
                writer.WriteLine("stack.Push(this);");
                writer.WriteLineNoTabs("");
                writer.WriteLine("while (stack.Count > 0)");
                writer.WriteLine('{');
                writer.Indent++;
                {
                    writer.WriteLine("var current = stack.Pop();");
                    writer.WriteLineNoTabs("");
                    writer.WriteLine("yield return current;");
                    writer.WriteLineNoTabs("");
                    writer.WriteLine("foreach (var child in current.ChildNodes().Reverse())");
                    writer.WriteLine('{');
                    writer.Indent++;
                    writer.WriteLine("stack.Push(child);");
                    writer.Indent--;
                    writer.WriteLine('}');
                }
                writer.Indent--;
                writer.WriteLine('}');
            }
            writer.Indent--;
            writer.WriteLine('}');
            writer.WriteLineNoTabs("");
            #endregion IEnumerable<TGreenRoot> EnumerateDescendants()

            #region bool IsEquivalentTo(TGreenRoot? other)
            writer.WriteLine("public virtual bool IsEquivalentTo([NotNullWhen(true)] {0}? other)", tree.GreenBase.ToCSharpString());
            writer.WriteLine('{');
            writer.Indent++;
            {
                writer.WriteLine("if (this == other) return true;");
                writer.WriteLine("if (other == null) return false;");
                foreach (var extraData in root.ExtraData)
                {
                    writer.WriteLine("if (this.{0} != other.{0}) return false;", extraData.PropertyName);
                }
                writer.WriteLineNoTabs("");
                writer.WriteLine("var n = this.SlotCount;");
                writer.WriteLine("if (n != other.SlotCount) return false;");
                writer.WriteLineNoTabs("");
                writer.WriteLine("for (int i = 0; i < n; i++)");
                writer.WriteLine('{');
                writer.Indent++;
                {
                    writer.WriteLine("var thisChild = this.GetSlot(i);");
                    writer.WriteLine("var otherChild = other.GetSlot(i);");
                    writer.WriteLine("if (thisChild != null && otherChild != null && !thisChild.IsEquivalentTo(otherChild))");
                    writer.WriteLine('{');
                    writer.Indent++;
                    writer.WriteLine("return false;");
                    writer.Indent--;
                    writer.WriteLine('}');
                }
                writer.Indent--;
                writer.WriteLine('}');
                writer.WriteLineNoTabs("");
                writer.WriteLine("return true;");
            }
            writer.Indent--;
            writer.WriteLine('}');
            writer.WriteLineNoTabs("");
            #endregion bool IsEquivalentTo(TGreenRoot? other)

            #region TRedRoot CreateRed()
            writer.WriteLine("public {0} CreateRed() => this.CreateRed(null);", tree.RedBase.ToCSharpString());
            writer.WriteLine("public abstract {0} CreateRed({0}? parent);", tree.RedBase.ToCSharpString());
            #endregion TRedRoot CreateRed()
        }
        writer.Indent--;
        writer.WriteLine('}');
    }

    private static void WriteGreenNode(this IndentedTextWriter writer, Tree tree, Node node)
    {
        if (node.TypeSymbol.IsAbstract)
            writer.Write("abstract ");
        writer.WriteLine("partial class {0} : {1}", node.TypeSymbol.Name, node.BaseSymbol!.ToCSharpString(false));
        writer.WriteLine('{');
        writer.Indent++;
        {
            writer.WriteGreenConstructor(node);

            if (node.NodeComponents.Any())
            {
                writer.WriteLineNoTabs("");
                foreach (var component in node.NodeComponents)
                {
                    writer.WriteLine("public {0} {1} => this.{2};", component.Type.ToCSharpString(), component.PropertyName, component.FieldName);
                }
            }

            #region TGreenBase? GetSlot(int index)
            if (!node.TypeSymbol.IsAbstract)
            {
                writer.WriteLineNoTabs("");
                writer.WriteLine("public override {0}? GetSlot(int index) =>", tree.GreenBase.ToCSharpString());
                writer.Indent++;
                if (node.Children.Length == 0)
                {
                    writer.WriteLine("null;");
                }
                else if (node.Children.Length == 1)
                {
                    writer.WriteLine("index == 0 ? this.{0} : null;", node.Children[0].PropertyName);
                }
                else
                {
                    writer.WriteLine("index switch");
                    writer.WriteLine('{');
                    writer.Indent++;
                    for (var idx = 0; idx < node.Children.Length; idx++)
                    {
                        var child = node.Children[idx];
                        writer.WriteLine("{0} => this.{1},", idx, child.PropertyName);
                    }
                    writer.WriteLine("_ => null");
                    writer.Indent--;
                    writer.WriteLine("};");
                }
                writer.Indent--;
            }
            #endregion TGreenBase? GetSlot(int index)

            #region TRedBase CreateRed(TRedBase? parent)
            if (!node.TypeSymbol.IsAbstract)
            {
                writer.WriteLineNoTabs("");
                writer.WriteLine("public override {0} CreateRed({0}? parent) =>", tree.RedBase.ToCSharpString());
                writer.Indent++;
                writer.WriteLine("new global::{0}.{1}(this, parent);", tree.RedBase.ContainingNamespace.ToCSharpString(), node.TypeSymbol.Name);
                writer.Indent--;
            }
            #endregion TRedBase CreateRed(TRedBase? parent)

            #region TNode Update(...)
            if (!node.TypeSymbol.IsAbstract && node.RequiredComponents.Any())
            {
                writer.WriteLineNoTabs("");
                writer.Write("public {0} Update(", node.TypeSymbol.ToCSharpString());
                var first = true;
                foreach (var component in node.RequiredComponents)
                {
                    if (!first) writer.Write(", ");
                    first = false;
                    writer.Write("{0} {1}", component.Type.ToCSharpString(), component.ParameterName);
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
                            tree.GreenBase.ContainingNamespace.ToCSharpString(),
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
        }
        writer.Indent--;
        writer.WriteLine('}');
    }

    private static void WriteGreenConstructor(this IndentedTextWriter writer, Node node)
    {
        if (node.TypeSymbol.IsAbstract)
            writer.Write("protected ");
        else
            writer.Write("internal ");
        writer.Write(node.TypeSymbol.Name);
        writer.Write('(');
        var first = true;
        foreach (var component in node.Components)
        {
            if (!first) writer.Write(", ");
            first = false;
            writer.Write(component.Type.ToCSharpString());
            writer.Write(' ');
            writer.Write(component.ParameterName);
        }
        writer.Write(')');
        if (node.ParentComponents.Any())
        {
            writer.Write(" : base(");
            first = true;
            foreach (var component in node.ParentComponents)
            {
                if (!first) writer.Write(", ");
                first = false;
                writer.Write(component.ParameterName);
            }
            writer.Write(')');
        }
        writer.WriteLine();
        writer.WriteLine('{');
        writer.Indent++;

        if (node.Descendants.IsDefaultOrEmpty)
        {
            if (node.Children.Length > byte.MaxValue)
                writer.WriteLine("this.SlotCount = byte.MaxValue;");
            else
                writer.WriteLine($"this.SlotCount = {node.Children.Length};");
        }

        foreach (var component in node.NodeComponents)
        {
            if (component.PassToBase) continue;
            writer.Write("this.");
            writer.Write(component.FieldName);
            writer.Write(" = ");
            writer.Write(component.ParameterName);
            writer.WriteLine(';');
        }

        writer.Indent--;
        writer.WriteLine('}');
    }

    private static void WriteGreenFactory(this IndentedTextWriter writer, Tree tree)
    {
        writer.WriteLine("{0} static class {1}Factory", tree.GreenBase.DeclaredAccessibility.ToCSharpString(), tree.Suffix);
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
                    writer.WriteGreenFactoryMethods(tree, node);
                }
            }
        }
        writer.Indent--;
        writer.WriteLine('}');
    }

    private static void WriteGreenFactoryMethods(this IndentedTextWriter writer, Tree tree, Node node)
    {
        if (node.RequiredComponents.Any(x => x.IsOptional))
        {
            writeMethod(writer, tree, node, false);
            writer.WriteLineNoTabs("");
        }
        writeMethod(writer, tree, node, true);

        static void writeMethod(IndentedTextWriter writer, Tree tree, Node node, bool includeOptional)
        {
            writer.Write("public static {0} {1}(", node.TypeSymbol.ToCSharpString(), node.TypeSymbol.Name.WithoutSuffix(tree.Suffix));
            var first = true;
            foreach (var component in node.RequiredComponents)
            {
                if (!includeOptional && component.IsOptional) continue;
                if (!first) writer.Write(", ");
                first = false;
                writer.Write("{0} {1}", component.Type.ToCSharpString(), component.ParameterName);
            }
            writer.WriteLine(')');
            writer.WriteLine('{');
            writer.Indent++;
            {
                writer.WriteLineNoTabs("#if DEBUG");
                foreach (var component in node.RequiredComponents.Where(x => !x.Type.IsValueType))
                {
                    if (component.IsOptional) continue;
                    writer.WriteLine("if ({0} == null) throw new global::System.ArgumentNullException(nameof({0}));", component.ParameterName);
                }
                if (node.Kinds.Length != 1)
                {
                    writer.WriteLine("switch (kind)");
                    writer.WriteLine('{');
                    writer.Indent++;
                    {
                        foreach (var kind in node.Kinds)
                            writer.WriteLine("case {0}:", kind.ToCSharpString());
                        writer.Indent++;
                        writer.WriteLine("break;");
                        writer.Indent--;
                        writer.WriteLine("default:");
                        writer.Indent++;
                        writer.WriteLine("throw new global::System.ArgumentException(\"Kind not accepted for this node.\", nameof(kind));");
                        writer.Indent--;
                    }
                    writer.Indent--;
                    writer.WriteLine('}');
                }
                writer.WriteLineNoTabs("#endif // DEBUG");
                writer.WriteLineNoTabs("");

                writer.WriteLine("return new {0}(", node.TypeSymbol.ToCSharpString());
                first = true;
                if (node.Kinds.Length == 1)
                {
                    first = false;
                    writer.Write("global::{0}", node.Kinds[0].ToCSharpString());
                }
                foreach (var component in node.RequiredComponents)
                {
                    if (!first) writer.Write(", ");
                    first = false;
                    if (component.IsOptional && !includeOptional)
                        writer.Write("default");
                    else
                        writer.Write(component.ParameterName);
                }
                writer.WriteLine(");");
            }
            writer.Indent--;
            writer.WriteLine('}');
        }
    }
}