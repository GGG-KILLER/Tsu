using Microsoft.CodeAnalysis;

namespace GUtils.CLI.SourceGenerator
{
    internal static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor CommandManagerClassIsNotPartial =
            new DiagnosticDescriptor ( id: "GU0001",
                                       title: $"The '{CodeConstants.CommandManagerAttribute.Name}' attribute can only be used on partial classes.",
                                       messageFormat: "The class '{0}' must contain the partial modifier so that the source generator can generate the rest of the code for that class.",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true );

        public static readonly DiagnosticDescriptor ClassDoesNotContainCommandManagerAttribute =
            new DiagnosticDescriptor ( id: "GU0002",
                                       title: $"Class doesn't have the '{CodeConstants.CommandManagerAttribute.Name}' attribute.",
                                       messageFormat: "The class '{0}' does not contain the command manager attribute, and as such a command manager cannot be generated for it.",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true );

        public static readonly DiagnosticDescriptor ClassContainsTooManyCommandManagerAttributes =
            new DiagnosticDescriptor ( id: "GU0003",
                                       title: $"Class has more than one '{CodeConstants.CommandManagerAttribute.Name}' attribute.",
                                       messageFormat: $"The class '{{0}}' has more than one '{CodeConstants.CommandManagerAttribute.Name}' attribute.",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true );

        public static readonly DiagnosticDescriptor InvalidFirstArgumentPassedToAttributeConstructor =
            new DiagnosticDescriptor ( id: "GU0004",
                                       title: $"Invalid first argument to '{CodeConstants.CommandManagerAttribute.Name}' attribute.",
                                       messageFormat: $"The class '{{0}}' is passing an invalid first argument to the '{CodeConstants.CommandManagerAttribute.Name}' attribute. The argument should be a typeof expression (no null).",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true );

        public static readonly DiagnosticDescriptor InvalidSecondArgumentPassedToattributeConstructor =
            new DiagnosticDescriptor ( id: "GU0005",
                                       title: $"Invalid second argument to '{CodeConstants.CommandManagerAttribute.Name}' attribute.",
                                       messageFormat: $"The class '{{0}}' is passing an invalid second argument to the '{CodeConstants.CommandManagerAttribute.Name}' attribute. The argument should be a sequence or an array of typeof expression (no nulls).",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true );
    }
}