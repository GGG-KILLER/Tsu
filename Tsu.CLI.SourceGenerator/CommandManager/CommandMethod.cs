// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Tsu.CLI.Commands;

namespace Tsu.CLI.SourceGenerator.CommandManager
{
    /// <summary>
    /// A generated command.
    /// </summary>
    public class CommandMethod : ICommandClassOrMethod
    {
        public static Result<CommandMethod, Diagnostic> Create(
            CommandAttribute attribute,
            CommandClass commandClass,
            IMethodSymbol methodSymbol)
        {
            if (methodSymbol.TypeParameters.Length > 0)
            {
                return Result.Err<CommandMethod, Diagnostic>(Diagnostic.Create(
                    DiagnosticDescriptors.TypeParametersNotSupportedForCommands,
                    commandClass.AttributeSyntax.GetLocation()));
            }

            if (methodSymbol.GetAttributes()
                            .Any(attrData => SymbolEqualityComparer.Default.Equals(attrData.AttributeClass, commandClass.CommandManager.CommonSymbols.Tsu_CLI_Commands_RawInputAttribute))
                && (methodSymbol.Parameters.Length != 1
                    || methodSymbol.Parameters[0].Type.SpecialType != SpecialType.System_String))
            {
                return Result.Err<CommandMethod, Diagnostic>(Diagnostic.Create(
                    DiagnosticDescriptors.RawInputCommandMustHaveSingleStringParameter,
                    commandClass.AttributeSyntax.GetLocation(),
                    methodSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
            }

            var parameters = methodSymbol.Parameters;
            for (var index = 0; index < parameters.Length; index++)
            {
                var parameterSymbol = methodSymbol.Parameters[index];

                if (parameterSymbol.IsParams)
                {
                    if (index < methodSymbol.Parameters.Length - 1)
                    {
                        return Result.Err<CommandMethod, Diagnostic>(Diagnostic.Create(
                            DiagnosticDescriptors.ParamsMustBeLastArgument,
                            commandClass.AttributeSyntax.GetLocation(),
                            parameterSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat),
                            methodSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
                    }
                    else if (!(parameterSymbol.Type is IArrayTypeSymbol arrayTypeSymbol)
                             || arrayTypeSymbol.Rank != 1
                             || !Utilities.CanConvertToFromString((INamedTypeSymbol) arrayTypeSymbol.ElementType))
                    {
                        return Result.Err<CommandMethod, Diagnostic>(Diagnostic.Create(
                            DiagnosticDescriptors.ParamsMustBeSingleRankArrayOfConvertibleType,
                            commandClass.AttributeSyntax.GetLocation(),
                            parameterSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat),
                            methodSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
                    }
                }
                else if (parameterSymbol.Type is not INamedTypeSymbol namedParameterType
                         || !Utilities.CanConvertToFromString(namedParameterType))
                {
                    return Result.Err<CommandMethod, Diagnostic>(Diagnostic.Create(
                        DiagnosticDescriptors.NonConvertibleArgumentInCommandMethod,
                        commandClass.AttributeSyntax.GetLocation(),
                        parameterSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat),
                        methodSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
                }
            }

            return Result.Ok<CommandMethod, Diagnostic>(new CommandMethod(attribute, commandClass, methodSymbol));
        }

        /// <summary>
        /// The attribute of this command method.
        /// </summary>
        public CommandAttribute Attribute { get; }

        /// <summary>
        /// The command class type.
        /// </summary>
        public CommandClass CommandClass { get; }

        /// <summary>
        /// The command method body.
        /// </summary>
        public IMethodSymbol MethodSymbol { get; }

        /// <inheritdoc cref="CommandClass.CommonSymbols" />
        public CommonSymbols CommonSymbols => CommandClass.CommonSymbols;

        /// <summary>
        /// Initializes a new generated command
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="methodSymbol"></param>
        private CommandMethod(CommandAttribute attribute, CommandClass commandClass, IMethodSymbol methodSymbol)
        {
            Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
            CommandClass = commandClass ?? throw new ArgumentNullException(nameof(commandClass));
            MethodSymbol = methodSymbol ?? throw new ArgumentNullException(nameof(methodSymbol));
        }

        public Result<BlockSyntax, Diagnostic> ToSyntaxNode(IdentifierNameSyntax inputIdentifier, IdentifierNameSyntax spaceIndexIdentifier)
        {
            throw new NotImplementedException();
            var attributes = MethodSymbol.GetAttributes();

            if (attributes.Any(attribute => SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, CommonSymbols.Tsu_CLI_Commands_RawInputAttribute)))
            {
            }
        }
    }
}