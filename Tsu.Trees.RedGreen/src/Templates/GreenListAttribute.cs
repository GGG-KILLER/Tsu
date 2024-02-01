namespace Tsu.Trees.RedGreen;

/// <summary>
/// Marks this struct as a green list base.
/// </summary>
/// <param name="kinds">
/// The kind that all lists will have.
/// This should be the value on the enum (e.g.: SyntaxKind.List)
/// </param>
[global::System.AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
internal sealed class GreenListAttribute(object kind) : global::System.Attribute
{
    /// <summary>
    /// The List Kind.
    /// </summary>
    public object Kind { get; } = kind;
}