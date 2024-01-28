namespace Tsu.Trees.RedGreen;

/// <summary>
/// Indicates this field/property stores a child node.
/// </summary>
/// <remarks>
/// This is used in generation of GetChildren and GetSlot.
/// </remarks>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ChildAttribute : Attribute
{
    /// <summary>
    /// The override for the slot of this child.
    /// </summary>
    public int? Slot { get; }
}