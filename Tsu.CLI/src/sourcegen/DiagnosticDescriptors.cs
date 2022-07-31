// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Microsoft.CodeAnalysis;

namespace Tsu.CLI.SourceGenerator
{
    internal static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor CommandManagerClassIsNotPartial =
            new DiagnosticDescriptor(id: "GU0001",
                                       title: $"The '{CodeConstants.CommandManagerAttribute.Name}' attribute can only be used on partial classes",
                                       messageFormat: "The class '{0}' must contain the partial modifier so that the source generator can generate the rest of the code for that class.",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true,
                                       customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        public static readonly DiagnosticDescriptor CommandManagerMustBeInANamespace =
            new DiagnosticDescriptor(id: "GU0002",
                                       title: "The command manager must be contained directly in a namespace",
                                       messageFormat: "The command manager class '{0}' is not directly contained by a namespace",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true,
                                       customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        public static readonly DiagnosticDescriptor ClassDoesNotContainCommandManagerAttribute =
            new DiagnosticDescriptor(id: "GU0003",
                                       title: $"Class doesn't have the '{CodeConstants.CommandManagerAttribute.Name}' attribute",
                                       messageFormat: "The class '{0}' does not contain the command manager attribute, and as such a command manager cannot be generated for it",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true,
                                       customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        public static readonly DiagnosticDescriptor ClassContainsTooManyCommandManagerAttributes =
            new DiagnosticDescriptor(id: "GU0004",
                                       title: $"Class has more than one '{CodeConstants.CommandManagerAttribute.Name}' attribute",
                                       messageFormat: $"The class '{{0}}' has more than one '{CodeConstants.CommandManagerAttribute.Name}' attribute",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true,
                                       customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        public static readonly DiagnosticDescriptor TypeParametersNotSupportedForCommands =
            new DiagnosticDescriptor(id: "GU0005",
                                       title: "Type parameters are not supported on command methods",
                                       messageFormat: "The class '{0}' has a command method with type parameters: '{1}'",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true,
                                       customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        public static readonly DiagnosticDescriptor InvalidNamePassedToCommandAttribute =
            new DiagnosticDescriptor(id: "GU0006",
                                       title: "The name passed to the Command attribute on a method command is invalid",
                                       messageFormat: "The name passed to the Command attribute on the method '{0}' is invalid. A command name can be composed only of ASCII leters and numbers.",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true,
                                       customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        public static readonly DiagnosticDescriptor NonConvertibleArgumentInCommandMethod =
            new DiagnosticDescriptor(id: "GU0007",
                                       title: "A command method has a non-convertible parameter",
                                       messageFormat: "The parameter '{0}' on the command method '{1}' can't be converted from a string",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true,
                                       customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        public static readonly DiagnosticDescriptor InvalidVerbPassedToCommandManagerAttribute =
            new DiagnosticDescriptor(id: "GU0008",
                                       title: "The verb cannot be an empty string or contain whitespaces",
                                       messageFormat: "The provided verb is an empty string or contains whitespaces",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true,
                                       customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        public static readonly DiagnosticDescriptor DuplicateVerbPassed =
            new DiagnosticDescriptor(id: "GU0009",
                                       title: "There is a command or verb with this name registered already",
                                       messageFormat: "There is a command or verb with this name registered already",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true,
                                       customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        public static readonly DiagnosticDescriptor RawInputCommandMustHaveSingleStringParameter =
            new DiagnosticDescriptor(id: "GU0010",
                                       title: "Raw input commands must have a single string parameter",
                                       messageFormat: "The command method '{0}' is a raw input command but does not have a single string parameter",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true,
                                       customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        public static readonly DiagnosticDescriptor ParamsMustBeLastArgument =
            new DiagnosticDescriptor(id: "GU0011",
                                       title: "Params parameter must be the last parameter in the method's parameters",
                                       messageFormat: "The parameter '{0}' on the command method '{1}' has a params parameter that is not the last parameter in the list",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true,
                                       customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        public static readonly DiagnosticDescriptor ParamsMustBeSingleRankArrayOfConvertibleType =
            new DiagnosticDescriptor(id: "GU0012",
                                       title: "Params parameter must be a single-dimensional array of a convertible type",
                                       messageFormat: "The parameter '{0}' on the command method '{1}' is not a single-dimensional array or of a type who can be converted from a string",
                                       category: "CLI",
                                       defaultSeverity: DiagnosticSeverity.Error,
                                       isEnabledByDefault: true,
                                       customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });
    }
}