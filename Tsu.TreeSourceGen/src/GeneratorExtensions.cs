using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Tsu.TreeSourceGen;

internal static class GeneratorExtensions
{
    public static IncrementalValuesProvider<Tree> GetTrees(this IncrementalGeneratorInitializationContext context)
    {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            $"{CodeConstants.Namespace}.{CodeConstants.TreeNodeAttribute.FullName}",
            (node, _) => node.IsKind(SyntaxKind.ClassDeclaration),
            (ctx, _) =>
            {
                var attr = ctx.Attributes.Single();
                if (attr.ConstructorArguments.Single().Value is not INamedTypeSymbol targetType || targetType.TypeKind != TypeKind.Class)
                {
                    return (null!, null!, null!, null);
                }

                string? name = null;
                if (attr.NamedArguments.SingleOrDefault(x => x.Key == "Name").Value.Value is string n
                    && !string.IsNullOrWhiteSpace(n))
                {
                    name = n;
                }

                return (
                    Root: targetType,
                    ParentClass: ((ClassDeclarationSyntax) ctx.TargetNode).GetContainingTypes(),
                    NodeSymbol: (INamedTypeSymbol) ctx.TargetSymbol,
                    Name: name
                );
            })
            .Where(x => x.NodeSymbol is not null && x.Root is not null)
            .Collect()
            .SelectMany((nodes, cancellationToken) =>
            {
                var groups = nodes.GroupBy(node => node.Root, SymbolEqualityComparer.Default);
                var builder = ImmutableArray.CreateBuilder<Tree>(groups.Count());
                foreach (var group in groups)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var treeNodes = group.Select(node => new Node(node.ParentClass, node.NodeSymbol, node.Name));

                    builder.Add(new Tree((INamedTypeSymbol) group.Key!, treeNodes));
                }
                return builder.MoveToImmutable();
            });
    }

    public static IncrementalValuesProvider<VisitorSet> GetVisitors(this IncrementalGeneratorInitializationContext context, string fullyQualifiedMetadataName)
    {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName,
            (node, _) => node.IsKind(SyntaxKind.ClassDeclaration),
            (ctx, _) =>
            {
                var attr = ctx.Attributes.Single();
                if (attr.ConstructorArguments.Single().Value is not INamedTypeSymbol targetType || targetType.TypeKind != TypeKind.Class)
                {
                    return (null!, null!);
                }

                return (
                    Root: targetType,
                    Visitor: new Visitor(
                        ctx.TargetSymbol.GetContainingNamespace(),
                        ((ClassDeclarationSyntax) ctx.TargetNode).GetContainingTypes(),
                        ((INamedTypeSymbol) ctx.TargetSymbol).Arity)
                );
            })
            .Where(x => x.Root is not null)
            .Collect()
            .SelectMany((visitors, cancellationToken) =>
            {
                var groups = visitors.GroupBy(visitor => visitor.Root, SymbolEqualityComparer.Default);

                var builder = ImmutableArray.CreateBuilder<VisitorSet>(groups.Count());
                foreach (var group in groups)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    builder.Add(new VisitorSet(
                        (INamedTypeSymbol) group.Key!,
                        group.Select(v => v.Visitor).ToImmutableArray()));
                }
                return builder.MoveToImmutable();
            });
    }

    public static IncrementalValuesProvider<(Tree Tree, VisitorSet Visitors)> MakeVisitorPairs(this IncrementalValuesProvider<Tree> trees, IncrementalValueProvider<ImmutableArray<VisitorSet>> visitors)
    {
        return trees.Combine(visitors)
            .Select((pair, cancellationToken) =>
            {
                var tree = pair.Left;
                var visitorSet = pair.Right.SingleOrDefault(s => SymbolEqualityComparer.Default.Equals(tree.Root, s.Root));

                return (Tree: tree, Visitors: visitorSet);
            })
            .Where(x => x.Visitors is not null)!;
    }

    public static IncrementalValuesProvider<(Tree Tree, VisitorSet Visitors, VisitorSet Walkers)> MakeWalkerSets(this IncrementalValuesProvider<Tree> trees, IncrementalValueProvider<ImmutableArray<VisitorSet>> visitors, IncrementalValuesProvider<VisitorSet> walkers)
    {
        return trees.Combine(visitors)
            .Combine(walkers.Collect())
            .Select((x, _) => (Tree: x.Left.Left, Visitors: x.Left.Right, Walkers: x.Right))
            .Select((pair, cancellationToken) =>
            {
                var tree = pair.Tree;
                var visitorSet = pair.Visitors.SingleOrDefault(s => SymbolEqualityComparer.Default.Equals(tree.Root, s.Root));
                var walkerSet = pair.Walkers.SingleOrDefault(s => SymbolEqualityComparer.Default.Equals(tree.Root, s.Root));

                return (Tree: tree, Visitor: visitorSet, Walkers: walkerSet);
            })
            .Where(x => x.Walkers is not null)!;
    }
}