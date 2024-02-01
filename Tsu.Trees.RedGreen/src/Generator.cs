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

using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tsu.Trees.RedGreen.SourceGenerator.Model;

namespace Tsu.Trees.RedGreen.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public sealed class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterResourcesToCopy([
            "GreenTreeRootAttribute.cs",
            "GreenNodeAttribute.cs",
        ]);

        var roots = context.GetTreeInfos();
        var nodes = context.GetNodeInfos();

        var trees = TreeCreator.BuildTree(roots, nodes);

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
                    builder.AppendLine($"// {indent}        {child.Type.ToCSharpString()} (Name = {child.FieldName}, IsOptional = {child.IsOptional}, PassToBase = {child.PassToBase})");
                builder.AppendLine($"// {indent}    ExtraData:");
                foreach (var child in node.Item2.ExtraData)
                    builder.AppendLine($"// {indent}        {child.Type.ToCSharpString()} (Name = {child.FieldName}, IsOptional = {child.IsOptional}, PassToBase = {child.PassToBase})");
                foreach (var derived in node.Item2.Descendants)
                    queue.Push((node.Item1 + 1, derived));
            }

            ctx.AddSource($"{tree.Suffix}.Debug.g.cs", builder.ToSourceText());
        });

        context.RegisterTemplateOutput(trees);
        context.RegisterGreenOutput(trees);
        context.RegisterRedMainOutput(trees);
    }
}
