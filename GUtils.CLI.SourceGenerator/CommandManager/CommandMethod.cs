using System;
using System.Collections.Immutable;
using System.Linq;
using GUtils.CLI.Commands;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GUtils.CLI.SourceGenerator.CommandManager
{
    /// <summary>
    /// A generated command.
    /// </summary>
    public class CommandMethod : ICommandClassOrMethod
    {
        public static Result<CommandMethod, Diagnostic> Create (
            CommandAttribute attribute,
            CommandClass commandClass,
            IMethodSymbol methodSymbol )
        {
            if ( methodSymbol.TypeParameters.Length > 0 )
            {
                return Result.Err<CommandMethod, Diagnostic> ( Diagnostic.Create (
                    DiagnosticDescriptors.TypeParametersNotSupportedForCommands,
                    commandClass.AttributeSyntax.GetLocation ( ) ) );
            }

            if ( methodSymbol.GetAttributes ( )
                             .Any ( attrData => SymbolEqualityComparer.Default.Equals ( attrData.AttributeClass, commandClass.CommandManager.CommonSymbols.GUtils_CLI_Commands_RawInputAttribute ) )
                 && ( methodSymbol.Parameters.Length != 1
                      || methodSymbol.Parameters[0].Type.SpecialType != SpecialType.System_String ) )
            {
                return Result.Err<CommandMethod, Diagnostic> ( Diagnostic.Create (
                    DiagnosticDescriptors.RawInputCommandMustHaveSingleStringParameter,
                    commandClass.AttributeSyntax.GetLocation ( ),
                    methodSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
            }

            ImmutableArray<IParameterSymbol> parameters = methodSymbol.Parameters;
            for ( var index = 0; index < parameters.Length; index++ )
            {
                IParameterSymbol parameterSymbol = methodSymbol.Parameters[index];

                if ( parameterSymbol.IsParams )
                {
                    if ( index < methodSymbol.Parameters.Length - 1 )
                    {
                        return Result.Err<CommandMethod, Diagnostic> ( Diagnostic.Create (
                            DiagnosticDescriptors.ParamsMustBeLastArgument,
                            commandClass.AttributeSyntax.GetLocation ( ),
                            parameterSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ),
                            methodSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
                    }
                    else if ( !( parameterSymbol.Type is IArrayTypeSymbol arrayTypeSymbol )
                              || arrayTypeSymbol.Rank != 1
                              || !Utilities.CanConvertToFromString ( ( INamedTypeSymbol ) arrayTypeSymbol.ElementType ) )
                    {
                        return Result.Err<CommandMethod, Diagnostic> ( Diagnostic.Create (
                            DiagnosticDescriptors.ParamsMustBeSingleRankArrayOfConvertibleType,
                            commandClass.AttributeSyntax.GetLocation ( ),
                            parameterSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ),
                            methodSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
                    }
                }
                else if ( parameterSymbol.Type is not INamedTypeSymbol namedParameterType
                          || !Utilities.CanConvertToFromString ( namedParameterType ) )
                {
                    return Result.Err<CommandMethod, Diagnostic> ( Diagnostic.Create (
                        DiagnosticDescriptors.NonConvertibleArgumentInCommandMethod,
                        commandClass.AttributeSyntax.GetLocation ( ),
                        parameterSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ),
                        methodSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
                }
            }

            return Result.Ok<CommandMethod, Diagnostic> ( new CommandMethod ( attribute, commandClass, methodSymbol ) );
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
        public CommonSymbols CommonSymbols => this.CommandClass.CommonSymbols;

        /// <summary>
        /// Initializes a new generated command
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="methodSymbol"></param>
        private CommandMethod ( CommandAttribute attribute, CommandClass commandClass, IMethodSymbol methodSymbol )
        {
            this.Attribute = attribute ?? throw new ArgumentNullException ( nameof ( attribute ) );
            this.CommandClass = commandClass ?? throw new ArgumentNullException ( nameof ( commandClass ) );
            this.MethodSymbol = methodSymbol ?? throw new ArgumentNullException ( nameof ( methodSymbol ) );
        }

        public Result<BlockSyntax, Diagnostic> ToSyntaxNode ( IdentifierNameSyntax inputIdentifier, IdentifierNameSyntax spaceIndexIdentifier )
        {
            throw new NotImplementedException ( );
            ImmutableArray<AttributeData> attributes = this.MethodSymbol.GetAttributes ( );

            if ( attributes.Any ( attribute => SymbolEqualityComparer.Default.Equals ( attribute.AttributeClass, this.CommonSymbols.GUtils_CLI_Commands_RawInputAttribute ) ) )
            {
            }
        }
    }
}