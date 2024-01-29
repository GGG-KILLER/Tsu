namespace Tsu.Trees.RedGreen;

/// <summary>
/// An attribute that marks the given class as the base class for all nodes in a green node tree.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
internal sealed class GreenTreeRootAttribute(Type redBase, string suffix, Type kindEnum) : Attribute
{
    /// <summary>
    /// The suffix for nodes in this tree.
    /// </summary>
    public string Suffix { get; } = suffix;

    /// <summary>
    /// The base node type for all nodes in the red tree.
    /// </summary>
    public Type RedBase { get; } = redBase;

    /// <summary>
    /// The enum type that contains the definitions for the node kinds.
    /// </summary>
    public Type KindEnum { get; } = kindEnum;

    /// <summary>
    /// Whether to create base visitor implementations for this tree.
    /// </summary>
    public bool CreateVisitors { get; set; }

    /// <summary>
    /// Whether to create a base walker implementation for this tree.
    /// </summary>
    public bool CreateWalker { get; set; }

    /// <summary>
    /// Whether to generate a base rewriter implementation for this tree.
    /// </summary>
    public bool CreateRewriter { get; set; }

    /// <summary>
    /// Whether to generate a file with only comments dumping the entire tree structure.
    /// </summary>
    public bool DebugDump { get; set; }
}