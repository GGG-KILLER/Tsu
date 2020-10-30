using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Tsu.CLI.Commands;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Tsu.CLI.SourceGenerator.CommandManager
{
    public class CommandClass : ICommandClassOrMethod
    {
        /// <summary>
        /// Attempts to create a new <see cref="CommandClass" />.
        /// </summary>
        /// <param name="commandManager"></param>
        /// <param name="classSymbol"></param>
        /// <param name="attributeSyntax"></param>
        /// <param name="verb"></param>
        /// <returns></returns>
        public static Result<CommandClass, Diagnostic> Create (
            CommandManagerClass commandManager,
            INamedTypeSymbol classSymbol,
            AttributeSyntax attributeSyntax,
            String? verb = null )
        {
            if ( verb is not null && String.IsNullOrWhiteSpace ( verb ) )
            {
                return Result.Err<CommandClass, Diagnostic> ( Diagnostic.Create (
                    DiagnosticDescriptors.InvalidVerbPassedToCommandManagerAttribute,
                    attributeSyntax.GetLocation ( ) ) );
            }

            return Result.Ok<CommandClass, Diagnostic> ( new CommandClass (
                commandManager,
                classSymbol,
                attributeSyntax,
                verb ) );
        }

        /// <summary>
        /// The command manager class.
        /// </summary>
        public CommandManagerClass CommandManager { get; }

        /// <summary>
        /// The command class symbol.
        /// </summary>
        public INamedTypeSymbol ClassSymbol { get; }

        /// <summary>
        /// The typeof syntax for this command class' class.
        /// </summary>
        public AttributeSyntax AttributeSyntax { get; }

        /// <summary>
        /// The command class verb.
        /// </summary>
        public String? Verb { get; }

        /// <inheritdoc cref="CommandManagerClass.CommonSymbols" />
        public CommonSymbols CommonSymbols => this.CommandManager.CommonSymbols;

        /// <summary>
        /// Initializes a new command class
        /// </summary>
        /// <param name="commandManager"></param>
        /// <param name="classSymbol"></param>
        /// <param name="attributeSyntax"></param>
        /// <param name="verb"></param>
        private CommandClass (
            CommandManagerClass commandManager,
            INamedTypeSymbol classSymbol,
            AttributeSyntax attributeSyntax,
            String? verb = null )
        {
            this.CommandManager = commandManager ?? throw new ArgumentNullException ( nameof ( commandManager ) );
            this.ClassSymbol = classSymbol ?? throw new ArgumentNullException ( nameof ( classSymbol ) );
            this.AttributeSyntax = attributeSyntax ?? throw new ArgumentNullException ( nameof ( attributeSyntax ) );
            this.Verb = verb;
        }

        /// <summary>
        /// Gets all command methods from this command class.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Result<ImmutableDictionary<String, CommandMethod>, Diagnostic> GetCommandMethods ( CancellationToken cancellationToken = default )
        {
            ImmutableDictionary<String, CommandMethod>.Builder dictionaryBuilder =
                ImmutableDictionary.CreateBuilder<String, CommandMethod> ( );

            foreach ( IMethodSymbol methodSymbol in this.ClassSymbol
                                                        .GetMembers ( )
                                                        .OfType<IMethodSymbol> ( )
                                                        .Where ( method => method.GetAttributes ( )
                                                                                 .Any ( attr => this.CommandManager.IsCommandAttributeSymbol ( attr.AttributeClass! ) ) ) )
            {
                cancellationToken.ThrowIfCancellationRequested ( );
                foreach ( AttributeData attrData in methodSymbol.GetAttributes ( )
                                                                .Where ( attr => this.CommandManager.IsCommandAttributeSymbol ( attr.AttributeClass! ) ) )
                {
                    cancellationToken.ThrowIfCancellationRequested ( );
                    CommandAttribute attr = Utilities.AttributeFromAttributeData<CommandAttribute> ( attrData );

                    if ( Utilities.IsValidCommandName ( attr.Name ) )
                    {
                        return Result.Err<ImmutableDictionary<String, CommandMethod>, Diagnostic> ( Diagnostic.Create (
                            DiagnosticDescriptors.InvalidNamePassedToCommandAttribute,
                            this.AttributeSyntax.GetLocation ( ),
                            methodSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
                    }

                    if ( !dictionaryBuilder.ContainsKey ( attr.Name )
                         || attr.Overwrite )
                    {
                        Result<CommandMethod, Diagnostic> commandMethod = CommandMethod.Create ( attr, this, methodSymbol );
                        if ( commandMethod.IsErr )
                            return Result.Err<ImmutableDictionary<String, CommandMethod>, Diagnostic> ( commandMethod.Err.Value );
                        else
                            dictionaryBuilder[attr.Name] = commandMethod.Ok.Value;
                    }
                }
            }

            return Result.Ok<ImmutableDictionary<String, CommandMethod>, Diagnostic> ( dictionaryBuilder.ToImmutable ( ) );
        }

        /// <inheritdoc />
        public Result<BlockSyntax, Diagnostic> ToSyntaxNode ( IdentifierNameSyntax inputIdentifier, IdentifierNameSyntax spaceIndexIdentifier )
        {
            return this.GetCommandMethods ( )
                       .AndThen ( commands =>
            {
                IdentifierNameSyntax secondSpaceIndexIdentifier = IdentifierName ( spaceIndexIdentifier.Identifier.ValueText + "2" ),
                    subCommandNameIdentifier = IdentifierName ( "subCommandName" );

                var commandsSwitchSections = new List<SwitchSectionSyntax> ( commands.Count );
                foreach ( KeyValuePair<String, CommandMethod> command in commands )
                {
                    Result<BlockSyntax, Diagnostic> block = command.Value.ToSyntaxNode ( inputIdentifier, secondSpaceIndexIdentifier );
                    if ( block.IsErr )
                        return block;

                    commandsSwitchSections.Add ( SwitchSection (
                        List ( new SwitchLabelSyntax[]
                        {
                            CaseSwitchLabel (
                                LiteralExpression (
                                    SyntaxKind.StringLiteralExpression,
                                    Literal ( command.Key )
                                )
                            )
                        } ),
                        List ( new StatementSyntax[]
                        {
                            block.Ok.Value,
                            BreakStatement ( )
                        } )
                    ) );
                }

                return ( Result<BlockSyntax, Diagnostic> ) Block ( new StatementSyntax[]
                {
                    /* Int32 secondSpaceIndex = input.IndexOf ( ' ', spaceIndex ) */
                    LocalDeclarationStatement (
                        /* Int32 secondSpaceIndex = input.IndexOf ( ' ', spaceIndex + 1 ) */
                        VariableDeclaration (
                            /* Int32 */
                            Utilities.GetTypeSyntax ( this.CommandManager.CommonSymbols.System_Int32 ),
                            /* secondSpaceIndex = input.IndexOf ( ' ', spaceIndex + 1 ) */
                            SeparatedList ( new[]
                            {
                                /* secondSpaceIndex = input.IndexOf ( ' ', spaceIndex + 1 ) */
                                VariableDeclarator (
                                    secondSpaceIndexIdentifier.Identifier,
                                    null,
                                    /* = input.IndexOf ( ' ', spaceIndex + 1 ) */
                                    EqualsValueClause (
                                        ( ExpressionSyntax )
                                        inputIdentifier.AsDynamic ( )
                                                       .IndexOf ( ' ', spaceIndexIdentifier.AsDynamic ( ) + 1 )
                                    )
                                    /* /= input.IndexOf ( ' ', spaceIndex ) */
                                )
                                /* /secondSpaceIndex = input.IndexOf ( ' ', spaceIndex ) */
                            } )
                        )
                        /* /Int32 secondSpaceIndex = input.IndexOf ( ' ', spaceIndex ) */
                    ),
                    /* /Int32 secondSpaceIndex = input.IndexOf ( ' ', spaceIndex ) */
                    /* String subCommandName = secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input; */
                    LocalDeclarationStatement (
                        VariableDeclaration (
                            /* String */
                            Utilities.GetTypeSyntax ( this.CommandManager.CommonSymbols.System_String ),
                            /* subCommandName = secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input; */
                            SeparatedList ( new []
                            {
                                /* subCommandName = secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input */
                                VariableDeclarator (
                                    subCommandNameIdentifier.Identifier,
                                    null,
                                    EqualsValueClause (
                                        /* secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input */
                                        ConditionalExpression (
                                            ( ExpressionSyntax )
                                            ( secondSpaceIndexIdentifier.AsDynamic ( ) != -1 ),
                                            ( ExpressionSyntax )
                                            inputIdentifier.AsDynamic ( )
                                                           .Substring ( spaceIndexIdentifier.AsDynamic ( ) + 1,
                                                                        secondSpaceIndexIdentifier),
                                            inputIdentifier
                                        )
                                        /* /secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input */
                                    )
                                )
                                /* /subCommandName = secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input */
                            } )
                        )
                    ),
                    /* /String subCommandName = secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input; */
                    SwitchStatement (
                        subCommandNameIdentifier,
                        List ( commandsSwitchSections )
                    ),
                } );
            } );
        }
    }
}