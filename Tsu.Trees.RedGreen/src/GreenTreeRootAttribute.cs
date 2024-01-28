namespace Tsu.Trees.RedGreen;

/// <summary>
/// An attribute that marks the given class as the base class for all nodes in a green node tree.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class GreenTreeRootAttribute(Type redBase, string suffix, Type kindEnum) : Attribute
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

    // NOTE: Make cache configurable?
    // /// <summary>
    // /// Whether to enable the green cache or not.
    // /// </summary>
    // public bool CacheEnabled { get; set; } = true;

    // /// <summary>
    // /// The amount of bits from the hash to use for caching.
    // /// </summary>
    // public int CacheSizeBits { get; set; } = 16;
}