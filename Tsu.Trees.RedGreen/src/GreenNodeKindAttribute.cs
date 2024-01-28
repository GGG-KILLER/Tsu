namespace Tsu.Trees.RedGreen;

/// <summary>
/// Overrides the kind for a given green node.
/// </summary>
/// <param name="kind">
/// The value of the kind this node should have.
/// This should be the value on the enum (e.g.: SyntaxKind.ClassDeclarationSyntax)
/// </param>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class GreenNodeKindAttribute(object kind) : Attribute
{
    /// <summary>
    /// This node's Kind.
    /// </summary>
    public object Kind { get; } = kind;
}