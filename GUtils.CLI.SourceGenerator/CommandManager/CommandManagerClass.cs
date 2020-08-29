using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using GUtils.CLI.Commands;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GUtils.CLI.SourceGenerator.CommandManager
{
    public class CommandManagerClass
    {
        /// <summary>
        /// Checks whether the attribute is the GeneratedCommandManagerAttribute.
        /// </summary>
        /// <param name="attributeData">The <see cref="CommandManagerAttributeDatas" /> of an attribute.</param>
        /// <returns></returns>
        private static Boolean IsGeneratedCommandManagerAttribute ( AttributeData attributeData ) =>
            attributeData.AttributeClass!.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat )
                                         .Equals ( CodeConstants.CommandManagerAttribute.Name, StringComparison.Ordinal );

        /// <summary>
        /// Attempts to initialize a new <see cref="CommandManagerClass" /> with the provided
        /// <paramref name="semanticModel" />, <paramref name="classDeclaration" />, <paramref
        /// name="commandAttributeSymbol" /> and <paramref name="cancellationToken" />.
        /// </summary>
        /// <param name="semanticModel">The semantic model of the command manager class.</param>
        /// <param name="classDeclaration">The declaration syntax of the command manager class.</param>
        /// <param name="commandAttributeSymbol">The command manager attribute symbol.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Result<CommandManagerClass, Diagnostic> Initialize (
            Compilation compilation,
            SemanticModel semanticModel,
            ClassDeclarationSyntax classDeclaration,
            CommonSymbols commonSymbols,
            CancellationToken cancellationToken = default )
        {
            // Retrieves the symbol of the class
            INamedTypeSymbol? classSymbol = semanticModel.GetDeclaredSymbol ( classDeclaration, cancellationToken );

            if ( classSymbol is null )
            {
                throw new InvalidOperationException ( "Class declaration doesn't have a symbol." );
            }

            // Validates that the class is partial
            cancellationToken.ThrowIfCancellationRequested ( );
            if ( !classDeclaration.Modifiers.Any ( tok => tok.ValueText.Equals ( "partial", StringComparison.Ordinal ) ) )
            {
                return Result.Err<CommandManagerClass, Diagnostic> ( Diagnostic.Create (
                    DiagnosticDescriptors.CommandManagerClassIsNotPartial,
                    Location.Create ( classDeclaration.SyntaxTree, classDeclaration.Identifier.Span ),
                    classSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
            }
            if ( !( classDeclaration.Parent is NamespaceDeclarationSyntax ) )
            {
                return Result.Err<CommandManagerClass, Diagnostic> ( Diagnostic.Create (
                    DiagnosticDescriptors.CommandManagerMustBeInANamespace,
                    Location.Create ( classDeclaration.SyntaxTree, classDeclaration.Identifier.Span ),
                    classSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
            }

            // Retrieves the possible GeneratedCommandManager attributes
            cancellationToken.ThrowIfCancellationRequested ( );
            IEnumerable<AttributeData> attributeDatas = classSymbol.GetAttributes ( )
                                                                       .Where ( IsGeneratedCommandManagerAttribute );

            // Validates the number of attributes
            if ( !attributeDatas.Any ( ) )
            {
                return Result.Err<CommandManagerClass, Diagnostic> ( Diagnostic.Create (
                    DiagnosticDescriptors.ClassDoesNotContainCommandManagerAttribute,
                    Location.Create ( classDeclaration.SyntaxTree, classDeclaration.Identifier.Span ),
                    classSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
            }

            cancellationToken.ThrowIfCancellationRequested ( );
            return Result.Ok<CommandManagerClass, Diagnostic> ( new CommandManagerClass (
                compilation,
                semanticModel,
                classDeclaration,
                classSymbol,
                commonSymbols,
                attributeDatas.ToImmutableArray ( ) ) );
        }

        private SemanticModel SemanticModel { get; }

        /// <summary>
        /// The command manager class' compilation
        /// </summary>
        public Compilation Compilation { get; }

        /// <summary>
        /// The common types container.
        /// </summary>
        public CommonSymbols CommonSymbols { get; }

        /// <summary>
        /// The declaration syntax of the command manager class.
        /// </summary>
        public ClassDeclarationSyntax ClassDeclaration { get; }

        /// <summary>
        /// The symbol of the class declaration.
        /// </summary>
        public INamedTypeSymbol ClassSymbol { get; }

        /// <summary>
        /// The attribute data of the command manager attribute.
        /// </summary>
        public ImmutableArray<AttributeData> CommandManagerAttributeDatas { get; }

        public event Action<Diagnostic>? DiagnosticReported;

        private CommandManagerClass (
            Compilation compilation,
            SemanticModel semanticModel,
            ClassDeclarationSyntax classDeclaration,
            INamedTypeSymbol classSymbol,
            CommonSymbols commonSymbols,
            ImmutableArray<AttributeData> commandManagerAttributeDatas )
        {
            this.Compilation = compilation ?? throw new ArgumentNullException ( nameof ( compilation ) );
            this.SemanticModel = semanticModel ?? throw new ArgumentNullException ( nameof ( semanticModel ) );
            this.ClassDeclaration = classDeclaration ?? throw new ArgumentNullException ( nameof ( classDeclaration ) );
            this.ClassSymbol = classSymbol ?? throw new ArgumentNullException ( nameof ( classSymbol ) );
            this.CommonSymbols = commonSymbols ?? throw new ArgumentNullException ( nameof ( commonSymbols ) );
            this.CommandManagerAttributeDatas = commandManagerAttributeDatas;
        }

        /// <summary>
        /// Checks whether the provided symbol is the <see cref="Commands.CommandAttribute" />.
        /// </summary>
        /// <param name="namedTypeSymbol"></param>
        /// <returns></returns>
        public Boolean IsCommandAttributeSymbol ( INamedTypeSymbol namedTypeSymbol ) =>
            SymbolEqualityComparer.Default.Equals ( this.CommonSymbols.GUtils_CLI_Commands_CommandAttribute, namedTypeSymbol );

        /// <summary>
        /// Retrieves the symbols of the classes that contains the commands that this manager will manage.
        /// </summary>
        /// <returns></returns>
        public Result<ImmutableArray<CommandClass>, Diagnostic> GetCommandClasses (
            CancellationToken cancellationToken = default )
        {
            var verbs = new HashSet<String> ( );

            // Initializes the variables for the type list aggregation
            cancellationToken.ThrowIfCancellationRequested ( );
            ImmutableArray<CommandClass>.Builder commandClasses =
                ImmutableArray.CreateBuilder<CommandClass> ( );

            foreach ( AttributeData? attributeData in this.CommandManagerAttributeDatas )
            {
                cancellationToken.ThrowIfCancellationRequested ( );
                if ( !( attributeData.ApplicationSyntaxReference?.GetSyntax ( cancellationToken ) is AttributeSyntax attributeSyntax ) )
                {
                    throw new InvalidOperationException ( "Attribute doesn't have a syntax associated with it." );
                }

                INamedTypeSymbol? typeSymbol = attributeData.ConstructorArguments[0].Value as INamedTypeSymbol
                    ?? throw new InvalidOperationException ( "Invalid first argument to attribute" );

                Result<CommandClass, Diagnostic> commandClassResult = CommandClass.Create (
                    this,
                    typeSymbol,
                    attributeSyntax,
                    attributeData.NamedArguments.FirstOrDefault ( kv => kv.Key == "Verb" ).Value.Value as String );

                if ( commandClassResult.IsErr )
                {
                    return Result.Err<ImmutableArray<CommandClass>, Diagnostic> ( commandClassResult.Err.Value );
                }
                else
                {
                    CommandClass commandClass = commandClassResult.Ok.Value;
                    if ( commandClass.Verb is not null && verbs.Contains ( commandClass.Verb ) )
                    {
                        return Result.Err<ImmutableArray<CommandClass>, Diagnostic> ( Diagnostic.Create (
                            DiagnosticDescriptors.DuplicateVerbPassed,
                            commandClass.AttributeSyntax.GetLocation ( ) ) );
                    }
                    else
                    {
                        if ( commandClass.Verb is not null )
                            verbs.Add ( commandClass.Verb );

                        commandClasses.Add ( commandClass );
                    }
                }
            }

            return Result.Ok<ImmutableArray<CommandClass>, Diagnostic> ( commandClasses.MoveToImmutable ( ) );
        }

        private Result<ImmutableDictionary<String, ICommandClassOrMethod>, Diagnostic> GetCommands (
            ImmutableArray<CommandClass> commandClasses,
            CancellationToken cancellationToken = default )
        {
            var verbsAndCommands = new HashSet<String> ( );
            ImmutableDictionary<String, ICommandClassOrMethod>.Builder builder =
                ImmutableDictionary.CreateBuilder<String, ICommandClassOrMethod> ( );

            foreach ( CommandClass commandClass in commandClasses.OrderBy ( commandClass => commandClass.Verb ) )
            {
                if ( commandClass.Verb is not null )
                {
                    if ( builder.ContainsKey ( commandClass.Verb ) )
                    {
                        return Result.Err<ImmutableDictionary<String, ICommandClassOrMethod>, Diagnostic> ( Diagnostic.Create (
                            DiagnosticDescriptors.DuplicateVerbPassed,
                            commandClass.AttributeSyntax.GetLocation ( ) ) );
                    }
                    else
                    {
                        builder[commandClass.Verb] = commandClass;
                        continue;
                    }
                }

                Result<ImmutableDictionary<String, CommandMethod>, Diagnostic> commands =
                    commandClass.GetCommandMethods ( cancellationToken );

                if ( commands.IsErr )
                    return Result.Err<ImmutableDictionary<String, ICommandClassOrMethod>, Diagnostic> ( commands.Err.Value );

                foreach ( KeyValuePair<String, CommandMethod> command in commands.Ok.Value )
                {
                    if ( !builder.ContainsKey ( command.Key )
                        || command.Value.Attribute.Overwrite )
                    {
                        builder[command.Key] = command.Value;
                    }
                }
            }

            return Result.Ok<ImmutableDictionary<String, ICommandClassOrMethod>, Diagnostic> ( builder.ToImmutable ( ) );
        }

        /// <summary>
        /// Generates the code for the command manager class.
        /// </summary>
        /// <returns></returns>
        public Result<String, Diagnostic> GenerateCommandManager ( )
        {
            Result<ImmutableArray<CommandClass>, Diagnostic> typesRes = this.GetCommandClasses ( );
            Result<ImmutableDictionary<String, ICommandClassOrMethod>, Diagnostic> commands = typesRes.AndThen ( symbols => this.GetCommands ( symbols ) );

            return commands.AndThen ( commands =>
            {
                IdentifierNameSyntax inputIdentifier = IdentifierName ( "line" ),
                    spaceIndexIdentifier = IdentifierName ( "spaceIdx" ),
                    commandNameIdentifier = IdentifierName ( "commandName" );

                var commandsSwitchSections = new List<SwitchSectionSyntax> ( commands.Count );
                foreach ( KeyValuePair<String, ICommandClassOrMethod> command in commands )
                {
                    Result<BlockSyntax, Diagnostic> block = command.Value.ToSyntaxNode ( inputIdentifier, spaceIndexIdentifier );
                    if ( block.IsErr )
                        return Result.Err<String, Diagnostic> ( block.Err.Value );

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

                // System.CodeDom.Compiler.GeneratedCode
                return Result.Ok<String, Diagnostic> ( CompilationUnit (
                    new SyntaxList<ExternAliasDirectiveSyntax> ( ),
                    List ( new[]
                    {
                        UsingDirective ( ParseName (
                            typeof ( String ).Namespace ) ),
                        UsingDirective ( ParseName (
                            typeof ( GeneratedCodeAttribute ).Namespace ) ),
                        UsingDirective ( ParseName (
                            typeof ( InputLineParser ).Namespace ) ),
                    } ),
                    new SyntaxList<AttributeListSyntax> ( ),
                    List ( new MemberDeclarationSyntax[]
                    {
                        this.ClassDeclaration.WithAttributeLists (
                            List ( new[]
                            {
                                AttributeList (
                                    SeparatedList ( new[]
                                    {
                                        Attribute ( ParseName ( "GeneratedCode" ) )
                                    } )
                                )
                            } )
                        )
                        .WithMembers (
                            List ( new MemberDeclarationSyntax[]
                            {
                                /* public override void Execute ( String line ) */
                                MethodDeclaration (
                                    attributeLists: List<AttributeListSyntax> ( ),
                                    modifiers: TokenList (
                                        Token ( SyntaxKind.PublicKeyword ),
                                        Token ( SyntaxKind.OverrideKeyword )
                                    ),
                                    returnType: PredefinedType ( Token ( SyntaxKind.VoidKeyword ) ),
                                    explicitInterfaceSpecifier: null,
                                    identifier: Identifier ( nameof ( BaseCommandManager.Execute ) ),
                                    typeParameterList: null,
                                    parameterList: ParameterList (
                                        SeparatedList ( new[]
                                        {
                                            Parameter (
                                                List<AttributeListSyntax> ( ),
                                                TokenList ( ),
                                                Utilities.GetTypeSyntax ( this.CommonSymbols.System_String ),
                                                inputIdentifier.Identifier,
                                                null
                                            )
                                        } )
                                    ),
                                    constraintClauses: List<TypeParameterConstraintClauseSyntax> ( ),
                                    body: Block ( new StatementSyntax[]
                                    {
                                        /* if ( String.IsNullOrWhiteSpace ( input ) )
                                         *     return;
                                         */
                                        IfStatement (
                                            InvocationExpression (
                                                Utilities.GetMemberAccessExpression (
                                                    this.CommonSymbols.System_String__IsNullOrWhiteSpaceString
                                                ),
                                                ArgumentList (
                                                    SeparatedList ( new[]
                                                    {
                                                        Argument ( inputIdentifier )
                                                    } )
                                                )
                                            ),
                                            ReturnStatement ( )
                                        ),
                                        /* input = input.Trim ( ) */
                                        ExpressionStatement (
                                            AssignmentExpression (
                                                SyntaxKind.SimpleAssignmentExpression,
                                                inputIdentifier,
                                                ( ExpressionSyntax ) inputIdentifier.AsDynamic ( ).Trim ( )
                                            )
                                        ),
                                        /* Int32 spaceIndex = input.IndexOf ( ' ' ) */
                                        LocalDeclarationStatement (
                                            VariableDeclaration (
                                                /* Int32 */
                                                Utilities.GetTypeSyntax ( this.CommonSymbols.System_Int32 ),
                                                /* spaceIndex = input.IndexOf ( ' ' ) */
                                                SeparatedList ( new[]
                                                {
                                                    /* spaceIndex = input.IndexOf ( ' ' ) */
                                                    VariableDeclarator (
                                                        spaceIndexIdentifier.Identifier,
                                                        null,
                                                        /* = input.IndexOf ( ' ' ) */
                                                        EqualsValueClause (
                                                            ( ExpressionSyntax )
                                                            inputIdentifier.AsDynamic ( ).IndexOf ( ' ' )
                                                        )
                                                    )
                                                } )
                                            )
                                        ),
                                        /* String commandName = spaceIndex != -1 ? input.Substring ( 0, spaceIndex ) : input; */
                                        LocalDeclarationStatement(
                                            /* String commandName = spaceIndex != -1 ? input.Substring ( 0, spaceIndex ) : input; */
                                            VariableDeclaration (
                                                /* String */
                                                Utilities.GetTypeSyntax ( this.CommonSymbols.System_String ),
                                                /* commandName = spaceIndex != -1 ? input.Substring ( 0, spaceIndex ) : input; */
                                                SeparatedList ( new[]
                                                {
                                                    /* commandName = spaceIndex != -1 ? input.Substring ( 0, spaceIndex ) : input; */
                                                    VariableDeclarator (
                                                        commandNameIdentifier.Identifier,
                                                        null,
                                                        /* = spaceIndex != -1 ? input.Substring ( 0, spaceIndex ) : input; */
                                                        EqualsValueClause (
                                                            /* spaceIndex != -1 ? input.Substring ( 0, spaceIndex ) : input; */
                                                            ConditionalExpression (
                                                                ( ExpressionSyntax )
                                                                ( spaceIndexIdentifier.AsDynamic ( ) != -1 ),
                                                                ( ExpressionSyntax )
                                                                inputIdentifier.AsDynamic ( )
                                                                               .Substring ( 0, spaceIndexIdentifier ),
                                                                inputIdentifier
                                                            )
                                                            /* /spaceIndex != -1 ? input.Substring ( 0, spaceIndex ) : input; */
                                                        )
                                                        /* /= spaceIndex != -1 ? input.Substring ( 0, spaceIndex ) : input; */
                                                    )
                                                    /* /commandName = spaceIndex != -1 ? input.Substring ( 0, spaceIndex ) : input; */
                                                } )
                                            )
                                        ),
                                        /* /String commandName = spaceIndex != -1 ? input.Substring ( 0, spaceIndex ) : input; */
                                        /* switch ( commandName ) */
                                        SwitchStatement (
                                            commandNameIdentifier,
                                            List ( commandsSwitchSections )
                                        )
                                    } ),
                                    expressionBody: null
                                )
                            } )
                        )
                    } )
                )
                    .NormalizeWhitespace ( indentation: "    ", eol: Environment.NewLine )
                    .ToFullString ( ) );
            } );
        }
    }
}