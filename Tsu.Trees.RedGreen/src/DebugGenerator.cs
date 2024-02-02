using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tsu.Trees.RedGreen.SourceGenerator.Model;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class DebugGenerator
{
    public static void RegisterDebugOutput(this IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<Tree> trees)
    {
        context.RegisterSourceOutput(trees, (ctx, tree) =>
        {
            if (!tree.DebugDump)
                return;

            var builder = new StringBuilder();

            builder.AppendLine($"// GreenBase = {tree.GreenBase.ToCSharpString()}");
            builder.AppendLine($"// RedBase = {tree.RedBase.ToCSharpString()}");
            builder.AppendLine($"// KindEnum = {tree.KindEnum.ToCSharpString()}");
            builder.AppendLine($"// CreateVisitors = {tree.CreateVisitors}");
            builder.AppendLine($"// CreateWalker = {tree.CreateWalker}");
            builder.AppendLine($"// CreateRewriter = {tree.CreateRewriter}");
            builder.AppendLine($"// Root = {tree.Root.TypeSymbol.ToCSharpString()}");

            var queue = new Stack<(int, Node)>();
            queue.Push((0, tree.Root));
            while (queue.Count > 0)
            {
                var node = queue.Pop();

                var indent = new string(' ', node.Item1 * 4);
                builder.AppendLine($"// {indent}{node.Item2.TypeSymbol.ToCSharpString()}");

                builder.AppendLine($"// {indent}    Kinds:");
                foreach (var kind in node.Item2.Kinds)
                    builder.AppendLine($"// {indent}        {kind.ToCSharpString()} (IsNull = {kind.IsNull}, Type = {kind.Type?.ToCSharpString() ?? "null"}, Value = {kind.Value})");

                builder.AppendLine($"// {indent}    Children:");
                foreach (var child in node.Item2.Children)
                    builder.AppendLine($"// {indent}        {child.Type.ToCSharpString()} (IsList = {child.IsList}, Name = {child.FieldName}, IsOptional = {child.IsOptional}, PassToBase = {child.PassToBase}, Order = {child.Order})");

                builder.AppendLine($"// {indent}    ExtraData:");
                foreach (var child in node.Item2.ExtraData)
                    builder.AppendLine($"// {indent}        {child.Type.ToCSharpString()} (IsList = {child.IsList}, Name = {child.FieldName}, IsOptional = {child.IsOptional}, PassToBase = {child.PassToBase}, Order = {child.Order})");

                builder.AppendLine($"// {indent}    Descendants:");

                foreach (var derived in node.Item2.Descendants)
                    queue.Push((node.Item1 + 2, derived));
            }

            ctx.AddSource($"{tree.Suffix}/Debug.g.cs", builder.ToSourceText());
        });
    }
}