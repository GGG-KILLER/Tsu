using System.Diagnostics.CodeAnalysis;
using Tsu.Trees.RedGreen.Internal;

namespace Tsu.Trees.RedGreen;

/// <summary>
/// Base interface for a red tree node.
/// </summary>
/// <typeparam name="TRedRoot"></typeparam>
/// <typeparam name="TKind"></typeparam>
public interface IRedNode<TRedRoot, TKind>
    where TRedRoot : class, IRedNode<TRedRoot, TKind>
    where TKind : Enum
{
    /// <summary>
    /// The kind of this node.
    /// </summary>
    TKind Kind { get; }

    /// <summary>
    /// This node's parent.
    /// </summary>
    TRedRoot? Parent { get; }

    /// <summary>
    /// Checks whether the current node is structurally equivalent to another.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    bool IsEquivalentTo([NotNullWhen(true)] TRedRoot? other);

    /// <summary>
    /// Determines if the specified node is a descendant of this node.
    /// Returns true for current node.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    bool Contains(TRedRoot other);

    /// <summary>
    /// Gets a list of the child nodes in prefix document order.
    /// </summary>
    /// <returns></returns>
    IEnumerable<TRedRoot> ChildNodes();

    #region Tree Traversal

    /// <summary>
    /// Gets a list of ancestor nodes.
    /// </summary>
    /// <returns></returns>
    IEnumerable<TRedRoot> Ancestors();

    /// <summary>
    /// Gets a list of ancestor nodes (including this node).
    /// </summary>
    /// <returns></returns>
    IEnumerable<TRedRoot> AncestorsAndSelf();

    /// <summary>
    /// Gets the first node of type TNode that matches the predicate.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="predicate"></param>
    /// <returns></returns>
    TNode? FirstAncestorOrSelf<TNode>(Func<TNode, bool>? predicate = null)
            where TNode : TRedRoot;

    /// <summary>
    /// Gets the first node of type TNode that matches the predicate.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TArg"></typeparam>
    /// <param name="predicate"></param>
    /// <param name="argument"></param>
    /// <returns></returns>
    TNode? FirstAncestorOrSelf<TNode, TArg>(Func<TNode, TArg, bool> predicate, TArg argument)
            where TNode : TRedRoot;


    /// <summary>
    /// Gets a list of descendant nodes in prefix document order.
    /// </summary>
    /// <param name="descendIntoChildren"></param>
    /// <returns></returns>
    IEnumerable<TRedRoot> DescendantNodes(Func<TRedRoot, bool>? descendIntoChildren = null);

    /// <summary>
    /// Gets a list of descendant nodes (including this node) in prefix document order.
    /// </summary>
    /// <param name="descendIntoChildren"></param>
    /// <returns></returns>
    IEnumerable<TRedRoot> DescendantNodesAndSelf(Func<TRedRoot, bool>? descendIntoChildren = null);

    #endregion Tree Traversal

    // TODO: ReplaceCore
    // TODO: ReplaceNodeInListCore
    // TODO: InsertNodesInListCore
    // TODO: RemoveNodesCore
}