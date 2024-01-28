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
            "Tsu.Trees.RedGreen.GreenNodeRootAttribute",
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

                return new TreeInfo(
                    (INamedTypeSymbol) ctx.TargetSymbol,
                    redBase,
                    kindEnum,
                    suffix,
                    createVisitors,
                    createWalker,
                    createRewriter
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

                return new NodeInfo(
                    type,
                    type.BaseType!,
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

            if (type.BaseType is not null)
                stack.Push(type.BaseType);
            visited.Add(type);
            nodes.Add(new NodeInfo(
                type.BaseType,
                type,
                ImmutableArray<TypedConstant>.Empty
            ));
        }

        return nodes;
    }

    private static Node ProcessNode(NodeInfo node, ILookup<ISymbol?, NodeInfo> nodes, INamedTypeSymbol greenRoot, IEnumerable<Component> parentNodes, IEnumerable<Component> parentExtraData)
    {
        var fields = node.NodeType.GetMembers().OfType<IFieldSymbol>().Where(f => f.IsReadOnly);
        var nodeChildren = fields.Where(x => x.Type.DerivesFrom(greenRoot)).Select(toComponent);
        var nodeExtraData = fields.Where(x => !x.Type.DerivesFrom(greenRoot)).Select(toComponent);

        var children = parentNodes.Select(x => x with { PassToBase = true })
                                  .Concat(nodeChildren)
                                  .ToImmutableArray();

        var extraData = parentExtraData.Select(x => x with { PassToBase = true })
                                     .Concat(nodeExtraData)
                                     .ToImmutableArray();

        return new Node(
            node.NodeType,
            nodes[node.NodeType].Select(x => ProcessNode(x, nodes, greenRoot, children, extraData))
                                .ToImmutableArray(),
            children,
            extraData);

        static Component toComponent(IFieldSymbol fieldSymbol) => new(fieldSymbol.Type, fieldSymbol.Name, false);
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
                var rootNode = ProcessNode(rootNodeInfo, subTrees, root.GreenBase, [], []);

                return new Tree(
                    root.GreenBase,
                    root.RedBase,
                    rootNode,
                    root.Suffix,
                    root.KindEnum,
                    root.CreateVisitors,
                    root.CreateWalker,
                    root.CreateRewriter
                );
            });
    }
}