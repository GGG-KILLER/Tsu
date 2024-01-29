using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis;
using Tsu.Trees.RedGreen.SourceGenerator.Model;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class GreenTreeWriter
{
    public static void ValidateTrees(this IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<Tree> trees)
    {
        // TODO: Implement
        return;
    }

    public static void WriteGreenNodes(this IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<Tree> trees)
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

            writer.Write("namespace ");
            writer.WriteLine(tree.GreenBase.ContainingNamespace.ToCSharpString());
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

            writer.Indent--;
            writer.WriteLine('}');
            writer.WriteLineNoTabs("");

            writer.Flush();
            ctx.AddSource($"{tree.Suffix}.Internal.g.cs", builder.ToSourceText());
        });
    }

    public static void WriteGreenRoot(this IndentedTextWriter writer, Tree tree, Node root)
    {
        writer.WriteLine("abstract partial class {0} : global::Tsu.Trees.RedGreen.Internal.IGreenNode<{1}, {2}, {3}>",
            root.TypeSymbol.Name,
            tree.GreenBase.ToCSharpString(),
            tree.RedBase.ToCSharpString(),
            tree.KindEnum.ToCSharpString());
        writer.WriteLine('{');
        writer.Indent++;
        {
            writer.WriteLine("private readonly {0} _kind;", tree.KindEnum.ToCSharpString());
            writer.WriteLine("private byte _slotCount;");
            writer.WriteLineNoTabs("");

            writer.WriteGreenConstructor(root);
            writer.WriteLineNoTabs("");

            foreach (var extraData in root.ExtraData)
            {
                writer.WriteLine("public {0} {1} => this.{2};", extraData.Type.ToCSharpString(), extraData.PropertyName, extraData.FieldName);
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

            writer.Write("public abstract ");
            writer.Write(tree.GreenBase.ToCSharpString());
            writer.WriteLine("? GetSlot(int slot);");
            writer.WriteLineNoTabs("");

            #region TGreenRoot GetRequiredSlot(int index)
            writer.WriteLine("public {0} GetRequiredSlot(int slot)", tree.GreenBase.ToCSharpString());
            writer.WriteLine('{');
            writer.Indent++;
            {
                writer.WriteLine("var node = this.GetSlot(slot);");
                writer.WriteLine("Debug.Assert((object)node != null)");
                writer.WriteLine("return node;");
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
                writer.WriteLine("for (var slot = 0; slot < count; slot++)");
                writer.Indent++;
                writer.WriteLine("yield return this.GetRequiredSlot(slot);");
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

    public static void WriteGreenNode(this IndentedTextWriter writer, Tree tree, Node node)
    {
        if (node.TypeSymbol.IsAbstract)
            writer.Write("abstract ");
        writer.WriteLine("partial class {0} : {1}", node.TypeSymbol.Name, node.BaseSymbol!.ToCSharpString());
        writer.WriteLine('{');
        writer.Indent++;
        {
            writer.WriteGreenConstructor(node);
            writer.WriteLineNoTabs("");
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
        foreach (var component in node.ExtraData)
        {
            if (!first) writer.Write(", ");
            first = false;
            writer.Write(component.Type.ToCSharpString());
            writer.Write(' ');
            writer.Write(component.ParameterName);
        }
        foreach (var component in node.Children)
        {
            if (!first) writer.Write(", ");
            first = false;
            writer.Write(component.Type.ToCSharpString());
            writer.Write(' ');
            writer.Write(component.ParameterName);
        }
        writer.Write(')');
        if (node.ExtraData.Any(x => x.PassToBase) || node.Children.Any(x => x.PassToBase))
        {
            writer.Write(" : base(");
            first = true;
            foreach (var component in node.ExtraData.Where(x => x.PassToBase))
            {
                if (!first) writer.Write(", ");
                first = false;
                writer.Write(component.ParameterName);
            }
            foreach (var component in node.Children.Where(x => x.PassToBase))
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

        foreach (var component in node.ExtraData)
        {
            if (component.PassToBase) continue;
            writer.Write("this.");
            writer.Write(component.FieldName);
            writer.Write(" = ");
            writer.Write(component.ParameterName);
            writer.WriteLine(';');
        }
        foreach (var component in node.Children)
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

    public static void WriteGreenFactory(this IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<Tree> trees)
    {
    }
}