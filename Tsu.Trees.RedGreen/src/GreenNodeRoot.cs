namespace Tsu.Trees.RedGreen;

/// <summary>
/// An attribute that marks the given class as the base class for all nodes in a green node tree.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class GreenNodeRootAttribute(string listName, Type redRoot) : Attribute
{
    /// <summary>
    /// The name of the list type for this green tree.
    /// </summary>
    public string ListName { get; } = listName;

    /// <summary>
    /// The base node type for all nodes in the red tree.
    /// </summary>
    public Type RedBase { get; } = redRoot;

    /// <summary>
    /// The enum type that contains the definitions for the node kinds.
    /// </summary>
    public Type? KindEnum { get; set; }

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