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

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Tsu.Trees.RedGreen.SourceGenerator.Model;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class TreeCreator
{
    public static IncrementalValuesProvider<TreeInfo> GetTreeInfos(this IncrementalGeneratorInitializationContext context)
    {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            "Tsu.Trees.RedGreen.GreenTreeRootAttribute",
            (x, _) => x is ClassDeclarationSyntax,
            (ctx, _) =>
            {
                var attr = ctx.Attributes.Single();

                var redBase = (INamedTypeSymbol) attr.ConstructorArguments[0].Value!;
                var suffix = (string) attr.ConstructorArguments[1].Value!;
                var kindEnum = (INamedTypeSymbol) attr.ConstructorArguments[2].Value!;
                var createVisitors = attr.NamedArguments.SingleOrDefault(x => x.Key == "CreateVisitors").Value.Value is true;
                var createWalker = attr.NamedArguments.SingleOrDefault(x => x.Key == "CreateWalker").Value.Value is true;
                var createRewriter = attr.NamedArguments.SingleOrDefault(x => x.Key == "CreateRewriter").Value.Value is true;
                var createLists = attr.NamedArguments.SingleOrDefault(x => x.Key == "CreateLists").Value.Value is true;
                var debugDump = attr.NamedArguments.SingleOrDefault(x => x.Key == "DebugDump").Value.Value is true;

                return new TreeInfo(
                    (INamedTypeSymbol) ctx.TargetSymbol,
                    redBase,
                    kindEnum,
                    suffix,
                    createVisitors,
                    createWalker,
                    createRewriter,
                    createLists,
                    debugDump
                );
            });
    }

    public static IncrementalValuesProvider<NodeInfo> GetNodeInfos(this IncrementalGeneratorInitializationContext context)
    {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            "Tsu.Trees.RedGreen.GreenNodeAttribute",
            (x, _) => x is ClassDeclarationSyntax,
            (ctx, _) =>
            {
                var attr = ctx.Attributes.Single();

                var type = (INamedTypeSymbol) ctx.TargetSymbol;
                var kinds = attr.ConstructorArguments.SingleOrDefault().Values;
                if (kinds.IsDefault)
                    kinds = ImmutableArray<TypedConstant>.Empty;

                return new NodeInfo(
                    type.BaseType!,
                    type,
                    kinds
                );
            });
    }

    private static List<NodeInfo> ListAllNodes(TreeInfo root, IEnumerable<NodeInfo> knownNodes)
    {
        var visited = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        var nodes = new List<NodeInfo>(knownNodes.Where(x => x.NodeType.DerivesFrom(root.GreenBase)));

        var stack = new Stack<INamedTypeSymbol>();
        foreach (var node in nodes)
        {
            if (node.BaseType is not null)
                stack.Push(node.BaseType);
            visited.Add(node.NodeType);
        }

        while (stack.Count > 0)
        {
            var type = stack.Pop();
            if (visited.Contains(type))
                continue;

            INamedTypeSymbol? baseType = null;
            if (type.BaseType is not null && type.BaseType.SpecialType == SpecialType.None)
                stack.Push(baseType = type.BaseType);
            visited.Add(type);
            nodes.Add(new NodeInfo(
                baseType,
                type,
                ImmutableArray<TypedConstant>.Empty
            ));
        }

        return nodes;
    }

    private static Node ProcessNode(
        TreeInfo tree,
        NodeInfo node,
        ILookup<ISymbol?, NodeInfo> nodes,
        IEnumerable<Component> parentNodes,
        IEnumerable<Component> parentExtraData)
    {
        var fields = node.NodeType.GetMembers()
            .OfType<IFieldSymbol>()
            .Where(f => f.CanBeReferencedByName && f.IsDefinition && !f.IsImplicitlyDeclared && !f.IsStatic && !f.IsConst && f.IsReadOnly);

        // Starting order for this node
        var order = 1;
        if (parentNodes.Any())
            order = Math.Max(order, parentNodes.Select(x => x.Order).Max() + 1);
        if (parentExtraData.Any())
            order = Math.Max(order, parentExtraData.Select(x => x.Order).Max() + 1);

        var nodeChildren = fields.Where(x => x.Type.DerivesFrom(tree.GreenBase))
                                 .Select(x =>
                                 {
                                     var ret = toComponent(x, order);
                                     if (ret.Order == order)
                                         order++;
                                     return ret;
                                 })
                                 .ToArray();
        var nodeExtraData = fields.Where(x => !x.Type.DerivesFrom(tree.GreenBase))
                                 .Select(x =>
                                 {
                                     var ret = toComponent(x, order);
                                     if (ret.Order == order)
                                         order++;
                                     return ret;
                                 })
                                 .ToArray();

        var children = parentNodes.Select(x => x with { PassToBase = true })
                                  .Concat(nodeChildren)
                                  .OrderBy(x => x.SortOrder)
                                  .ToImmutableArray();

        var extraData = parentExtraData.Select(x => x with { PassToBase = true })
                                     .Concat(nodeExtraData)
                                     .OrderBy(x => x.SortOrder)
                                     .ToImmutableArray();

        if (SymbolEqualityComparer.Default.Equals(node.NodeType, tree.GreenBase))
            extraData = extraData.Add(new Component(false, tree.KindEnum, "_kind", false, false, 0));

        return new Node(
            node.BaseType,
            node.NodeType,
            nodes[node.NodeType].Select(x => ProcessNode(tree, x, nodes, children, extraData))
                                .ToImmutableArray(),
            node.Kinds,
            children,
            extraData);

        static Component toComponent(IFieldSymbol fieldSymbol, int order)
        {
            var isList = false;
            var type = fieldSymbol.Type;

            if (fieldSymbol.GetAttributes().SingleOrDefault(x => x.AttributeClass?.ToCSharpString(false) == "global::Tsu.Trees.RedGreen.GreenListAttribute") is AttributeData listAttr)
            {
                isList = true;
                type = (INamedTypeSymbol) listAttr.ConstructorArguments.Single().Value!;
            }

            if (fieldSymbol.GetAttributes().SingleOrDefault(x => x.AttributeClass?.ToCSharpString(false) == "global::Tsu.Trees.RedGreen.NodeComponentAttribute") is AttributeData custAttr)
            {
                var val = custAttr.NamedArguments.SingleOrDefault(x => x.Key == "Order").Value;
                if (val.Value is int num)
                    order = num;
            }

            return new(
                isList,
                type,
                fieldSymbol.Name,
                fieldSymbol.Type.NullableAnnotation == NullableAnnotation.Annotated,
                false,
                order);
        }
    }

    public static IncrementalValuesProvider<Tree> BuildTree(IncrementalValuesProvider<TreeInfo> roots, IncrementalValuesProvider<NodeInfo> nodes)
    {
        return roots.Combine(nodes.Collect())
            .Select((x, _) =>
            {
                var (root, initialNodes) = x;
                var nodes = ListAllNodes(root, initialNodes.Where(x => x.NodeType.DerivesFrom(root.GreenBase)));
                var subTrees = nodes.ToLookup(node => node.BaseType, SymbolEqualityComparer.Default);
                var rootNodeInfo = subTrees[null].Single();
                var rootNode = ProcessNode(root, rootNodeInfo, subTrees, [], []);

                return new Tree(
                    root.GreenBase,
                    root.RedBase,
                    rootNode,
                    root.Suffix,
                    root.KindEnum,
                    root.CreateVisitors,
                    root.CreateWalker,
                    root.CreateRewriter,
                    root.CreateLists,
                    root.DebugDump
                );
            });
    }
}