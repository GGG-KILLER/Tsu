using Microsoft.CodeAnalysis;

namespace Tsu.TreeSourceGen;

internal static class TsgDiagnostics
{
    public static DiagnosticDescriptor NodeDoesNotInheritFromRoot = new(
        id: "TSG0001",
        title: "Node does not inherit from root",
        messageFormat: "The node '{0}' does not inherit from the root type that was listed in the TreeNode attribute",
        category: "Tsu.TreeSourceGen",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        customTags: [WellKnownDiagnosticTags.NotConfigurable]
    );


    public static DiagnosticDescriptor NodeIsNotPartial = new(
        id: "TSG0002",
        title: "Node is not implemented as a partial class",
        messageFormat: "The node '{0}' is not implemented as a partial class. This means that code cannot be generated to extend the class with visitor features.",
        category: "Tsu.TreeSourceGen",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        customTags: [WellKnownDiagnosticTags.NotConfigurable]
    );

    public static Diagnostic Create(DiagnosticDescriptor descriptor, ISymbol symbol, params object[] messageArgs)
    {
        var locations = symbol.DeclaringSyntaxReferences.Select(s => s.SyntaxTree.GetLocation(s.Span));
        return Diagnostic.Create(
            descriptor: descriptor,
            location: locations.First(),
            additionalLocations: locations.Skip(1),
            messageArgs: messageArgs);
    }
}