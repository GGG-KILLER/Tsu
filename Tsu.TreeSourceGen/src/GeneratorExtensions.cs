using System.Collections.Immutable;
using System.Text;
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
                var attr = ctx.Attributes.SingleOrDefault();
                var nodeSymbol = (INamedTypeSymbol) ctx.TargetSymbol;

                // Only accept symbol arguments
                if (attr?.ConstructorArguments.Single().Value is not INamedTypeSymbol firstArg
                    || firstArg.TypeKind != TypeKind.Class
                    || (nodeSymbol.IsAbstract && !SymbolEqualityComparer.Default.Equals(firstArg, ctx.TargetSymbol)))
                {
                    return (null!, null!, null!, null);
                }

                string? name = null;
                if (attr!.NamedArguments.SingleOrDefault(x => x.Key == "Name").Value.Value is string n
                    && !string.IsNullOrWhiteSpace(n))
                {
                    name = n;
                }

                return (
                    Root: firstArg,
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

                    var treeNodes = group.Select(node => new Node(node.ParentClass, node.NodeSymbol, node.Name)).ToArray();

                    var root = treeNodes.Single(n => SymbolEqualityComparer.Default.Equals(n.TypeSymbol, group.Key));

                    builder.Add(new Tree(root, treeNodes.Except([root])));
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

                var attr = ctx.Attributes.SingleOrDefault();
                if (attr?.ConstructorArguments.Single().Value is not INamedTypeSymbol targetType || targetType.TypeKind != TypeKind.Class)
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
                var visitorSet = pair.Right.SingleOrDefault(s => SymbolEqualityComparer.Default.Equals(tree.Root.TypeSymbol, s.Root));
                return (Tree: tree, Visitors: visitorSet);
            })
            .Where(x => x.Visitors is not null)!;
    }

    public static IncrementalValuesProvider<(Tree Tree, VisitorSet Visitors, VisitorSet Walkers)> MakeWalkerSets(this IncrementalValuesProvider<(Tree Tree, VisitorSet Visitors)> pairs, IncrementalValuesProvider<VisitorSet> walkers)
    {
        return pairs.Combine(walkers.Collect())
            .Select((x, _) => (Tree: x.Left.Tree, Visitors: x.Left.Visitors, Walkers: x.Right))
            .Select((pair, cancellationToken) =>
            {
                var tree = pair.Tree;
                var visitorSet = pair.Visitors;
                var walkerSet = pair.Walkers.SingleOrDefault(s => SymbolEqualityComparer.Default.Equals(tree.Root.TypeSymbol, s.Root));

                return (Tree: tree, Visitor: visitorSet, Walkers: walkerSet);
            })
            .Where(x => x.Visitor is not null && x.Walkers is not null)!;
    }
}