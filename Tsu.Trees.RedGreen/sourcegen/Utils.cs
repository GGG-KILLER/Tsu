using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Tsu.Trees.RedGreen.SourceGenerator.Model;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class Utils
{
    public static bool DerivesFrom(this INamedTypeSymbol symbol, INamedTypeSymbol parent)
    {
        if (symbol.BaseType is null)
            return false;

        for (var type = symbol.BaseType; type is not null; type = type.BaseType)
        {
            if (SymbolEqualityComparer.Default.Equals(type, parent))
                return true;
        }

        return false;
    }

    public static List<NodeInfo> ListAllNodes(TreeInfo root, IEnumerable<NodeInfo> knownNodes)
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

}