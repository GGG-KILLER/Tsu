using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GUtils.CLI.SourceGenerator.CommandManager
{
    public class ClassGenerator
    {
        /// <summary>
        /// Checks whether the attribute is the GeneratedCommandManagerAttribute.
        /// </summary>
        /// <param name="attributeData">The <see cref="AttributeData" /> of an attribute.</param>
        /// <returns></returns>
        private static Boolean IsGeneratedCommandManagerAttribute ( AttributeData attributeData ) =>
            attributeData.AttributeClass.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat )
                                        .Equals ( CodeConstants.CommandManagerAttribute.Name, StringComparison.Ordinal );

        private SemanticModel SemanticModel { get; }
        private ClassDeclarationSyntax ClassDeclaration { get; }
        private INamedTypeSymbol CommandAttribute { get; }
        private CancellationToken CancellationToken { get; }

        public event Action<Diagnostic> DiagnosticReported;

        public ClassGenerator ( SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, INamedTypeSymbol commandAttribute, CancellationToken cancellationToken = default )
        {
            this.SemanticModel = semanticModel ?? throw new ArgumentNullException ( nameof ( semanticModel ) );
            this.ClassDeclaration = classDeclaration ?? throw new ArgumentNullException ( nameof ( classDeclaration ) );
            this.CommandAttribute = commandAttribute ?? throw new ArgumentNullException ( nameof ( commandAttribute ) );
            this.CancellationToken = cancellationToken;
        }

        /// <summary>
        /// Checks whether the provided symbol is the <see cref="Commands.CommandAttribute" />.
        /// </summary>
        /// <param name="namedTypeSymbol"></param>
        /// <returns></returns>
        private Boolean IsCommandAttributeSymbol ( INamedTypeSymbol namedTypeSymbol ) =>
            SymbolEqualityComparer.Default.Equals ( this.CommandAttribute, namedTypeSymbol );

        /// <summary>
        /// Retrieves the symbols of the classes that contains the commands that this manager will manage.
        /// </summary>
        /// <returns></returns>
        private Result<ImmutableArray<INamedTypeSymbol>, Diagnostic> GetCommandClasses ( )
        {
            // Retrieves the symbol of the class
            INamedTypeSymbol classSymbol = this.SemanticModel.GetDeclaredSymbol ( this.ClassDeclaration, this.CancellationToken );

            // Validates that the class is partial
            this.CancellationToken.ThrowIfCancellationRequested ( );
            if ( !this.ClassDeclaration.Modifiers.Any ( tok => tok.ValueText.Equals ( "partial", StringComparison.Ordinal ) ) )
            {
                return Result.Err<ImmutableArray<INamedTypeSymbol>, Diagnostic> ( Diagnostic.Create ( DiagnosticDescriptors.CommandManagerClassIsNotPartial,
                                                                                                      Location.Create ( this.ClassDeclaration.SyntaxTree, this.ClassDeclaration.Identifier.Span ),
                                                                                                      classSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
            }

            // Retrieves the possible GeneratedCommandManager attributes
            this.CancellationToken.ThrowIfCancellationRequested ( );
            IEnumerable<AttributeData> possibleAttributes = classSymbol.GetAttributes ( )
                                                                       .Where ( IsGeneratedCommandManagerAttribute );

            // Validates the number of attributes
            if ( !possibleAttributes.Any ( ) )
            {
                return Result.Err<ImmutableArray<INamedTypeSymbol>, Diagnostic> ( Diagnostic.Create ( DiagnosticDescriptors.ClassDoesNotContainCommandManagerAttribute,
                                                                                                      Location.Create ( this.ClassDeclaration.SyntaxTree, this.ClassDeclaration.Identifier.Span ),
                                                                                                      classSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
            }
            else if ( possibleAttributes.Count ( ) > 1 )
            {
                return Result.Err<ImmutableArray<INamedTypeSymbol>, Diagnostic> ( Diagnostic.Create ( DiagnosticDescriptors.ClassContainsTooManyCommandManagerAttributes,
                                                                                                      Location.Create ( this.ClassDeclaration.SyntaxTree, this.ClassDeclaration.Identifier.Span ),
                                                                                                      classSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
            }

            // Initializes the variables for the type list aggregation
            this.CancellationToken.ThrowIfCancellationRequested ( );
            ImmutableArray<INamedTypeSymbol>.Builder commandClasses = ImmutableArray.CreateBuilder<INamedTypeSymbol> ( );
            AttributeData attribute = possibleAttributes.Single ( );

            // Get the first type
            this.CancellationToken.ThrowIfCancellationRequested ( );
            TypedConstant firstType = attribute.ConstructorArguments[0];
            if ( firstType.Kind == TypedConstantKind.Type && firstType.Value is INamedTypeSymbol firstTypeSymbol )
            {
                commandClasses.Add ( firstTypeSymbol );
            }
            else
            {
                Location location = attribute.ApplicationSyntaxReference switch
                {
                    SyntaxReference syntaxRef => Location.Create ( syntaxRef.SyntaxTree, syntaxRef.Span ),
                    _ => Location.Create ( this.ClassDeclaration.SyntaxTree, this.ClassDeclaration.Identifier.Span )
                };
                return Result.Err<ImmutableArray<INamedTypeSymbol>, Diagnostic> ( Diagnostic.Create ( DiagnosticDescriptors.InvalidFirstArgumentPassedToAttributeConstructor,
                                                                                                      location,
                                                                                                      classSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
            }

            // Retrieves the types from the params array
            this.CancellationToken.ThrowIfCancellationRequested ( );
            TypedConstant otherTypesArray = attribute.ConstructorArguments[1];
            if ( otherTypesArray.Kind == TypedConstantKind.Array && otherTypesArray.Value is ImmutableArray<TypedConstant> otherTypes && !otherTypes.IsDefault )
            {
                foreach ( TypedConstant type in otherTypes )
                {
                    // Validates the array element
                    this.CancellationToken.ThrowIfCancellationRequested ( );
                    if ( type.Kind == TypedConstantKind.Type && type.Value is INamedTypeSymbol typeSymbol )
                    {
                        commandClasses.Add ( typeSymbol );
                    }
                    else
                    {
                        Location location = attribute.ApplicationSyntaxReference switch
                        {
                            SyntaxReference syntaxRef => Location.Create ( syntaxRef.SyntaxTree, syntaxRef.Span ),
                            _ => Location.Create ( this.ClassDeclaration.SyntaxTree, this.ClassDeclaration.Identifier.Span )
                        };
                        return Result.Err<ImmutableArray<INamedTypeSymbol>, Diagnostic> ( Diagnostic.Create ( DiagnosticDescriptors.InvalidSecondArgumentPassedToattributeConstructor,
                                                                                                              location,
                                                                                                              classSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
                    }
                }
            }
            else
            {
                Location location = attribute.ApplicationSyntaxReference switch
                {
                    SyntaxReference syntaxRef => Location.Create ( syntaxRef.SyntaxTree, syntaxRef.Span ),
                    _ => Location.Create ( this.ClassDeclaration.SyntaxTree, this.ClassDeclaration.Identifier.Span )
                };
                return Result.Err<ImmutableArray<INamedTypeSymbol>, Diagnostic> ( Diagnostic.Create ( DiagnosticDescriptors.InvalidSecondArgumentPassedToattributeConstructor,
                                                                                                      location,
                                                                                                      classSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat ) ) );
            }

            return Result.Ok<ImmutableArray<INamedTypeSymbol>, Diagnostic> ( commandClasses.MoveToImmutable ( ) );
        }

        private ImmutableDictionary<String, IMethodSymbol> GetCommandMethods ( ImmutableArray<INamedTypeSymbol> commandTypes )
        {
            ImmutableDictionary<String, IMethodSymbol>.Builder dictionaryBuilder =
                ImmutableDictionary.CreateBuilder<String, IMethodSymbol> ( );
            foreach ( INamedTypeSymbol commandType in commandTypes )
            {
                IEnumerable<IMethodSymbol> methods = commandType.GetMembers ( )
                                                                .OfType<IMethodSymbol> ( )
                                                                .Where ( method => method.GetAttributes ( )
                                                                                         .Any ( attr => this.IsCommandAttributeSymbol ( attr.AttributeClass ) ) );

                foreach ( IMethodSymbol method in methods )
                {
                    foreach ( AttributeData attr in method.GetAttributes ( ).Where ( attr => this.IsCommandAttributeSymbol ( attr.AttributeClass ) ) )
                    {
                    }
                }
            }
        }

        public Result<String, Diagnostic> GenerateCommandManager ( )
        {
            Result<ImmutableArray<INamedTypeSymbol>, Diagnostic> typesRes = this.GetCommandClasses ( );
        }
    }
}