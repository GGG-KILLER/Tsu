using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GUtils.CLI.SourceGenerator.CommandManager;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GUtils.CLI.SourceGenerator
{
    /// <summary>
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// Returns whether the string is a valid command name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Boolean IsValidCommandName ( String name )
        {
            return String.IsNullOrWhiteSpace ( name )
                || name.Any ( ch => ( '0' <= ch && ch <= '9' )
                                    || ( 'a' <= ch && ch <= 'z' )
                                    || ( 'A' <= ch && ch <= 'Z' ) );
        }

        /// <summary>
        /// Attempts to get the method with the provided <paramref name="name" /> and <paramref
        /// name="paramsTypes" /> on the provided <paramref name="typeSymbol" />.
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <param name="name"></param>
        /// <param name="paramsTypes"></param>
        /// <returns></returns>
        public static IMethodSymbol? GetMethodSymbol ( ITypeSymbol typeSymbol, String name, Boolean isStatic, params Object[] paramsTypes )
        {
            return typeSymbol.GetMembers ( name )
                             .OfType<IMethodSymbol> ( )
                             .SingleOrDefault ( method => method.IsStatic == isStatic
                                 && method.Parameters.Length == paramsTypes.Length
                                 && method.Parameters.Zip ( paramsTypes, ( paramSymbol, paramType ) => validateParam ( paramSymbol, paramType ) ).All ( x => x ) );

            static Boolean validateParam ( IParameterSymbol parameterSymbol, Object expected ) =>
                expected switch
                {
                    ITypeSymbol typeSymbol => SymbolEqualityComparer.Default.Equals ( parameterSymbol.Type, typeSymbol ),
                    SpecialType specialType => parameterSymbol.Type.SpecialType == specialType,
                    _ => throw new InvalidOperationException ( "Parameter types must be ITypeSymbols or SpecialTypes" )
                };
        }

        /// <summary>
        /// Returns the <see cref="TypeSyntax" /> for the provided <paramref name="namedTypeSymbol" />.
        /// </summary>
        /// <param name="namedTypeSymbol"></param>
        /// <returns></returns>
        public static TypeSyntax GetTypeSyntax ( INamedTypeSymbol namedTypeSymbol )
        {
            if ( namedTypeSymbol is null )
                throw new ArgumentNullException ( nameof ( namedTypeSymbol ) );

            return SyntaxFactory.ParseTypeName ( namedTypeSymbol.ToDisplayString ( SymbolDisplayFormat.MinimallyQualifiedFormat ) );
        }

        /// <summary>
        /// Gets the <see cref="MemberAccessExpressionSyntax" /> for the provided instance <paramref
        /// name="methodSymbol" /> on the provided <paramref name="instance" />.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="methodSymbol"></param>
        /// <returns></returns>
        public static MemberAccessExpressionSyntax GetMemberAccessExpression ( ExpressionSyntax instance, IMethodSymbol methodSymbol )
        {
            if ( instance is null )
                throw new ArgumentNullException ( nameof ( instance ) );
            if ( methodSymbol is null )
                throw new ArgumentNullException ( nameof ( methodSymbol ) );
            if ( methodSymbol.IsStatic )
                throw new ArgumentException ( "Method is static.", nameof ( methodSymbol ) );
            if ( methodSymbol.MethodKind != MethodKind.Ordinary )
                throw new ArgumentException ( "Method is not an ordinary method.", nameof ( methodSymbol ) );

            return SyntaxFactory.MemberAccessExpression (
                SyntaxKind.SimpleMemberAccessExpression,
                instance,
                SyntaxFactory.IdentifierName ( methodSymbol.Name ) );
        }

        /// <summary>
        /// Gets the <see cref="MemberAccessExpressionSyntax" /> for the provided
        /// <b>static</b><paramref name="methodSymbol" />.
        /// </summary>
        /// <param name="methodSymbol"></param>
        /// <returns></returns>
        public static MemberAccessExpressionSyntax GetMemberAccessExpression ( IMethodSymbol methodSymbol )
        {
            if ( methodSymbol is null )
                throw new ArgumentNullException ( nameof ( methodSymbol ) );
            if ( !methodSymbol.IsStatic )
                throw new ArgumentException ( "Method is not static.", nameof ( methodSymbol ) );
            if ( methodSymbol.MethodKind != MethodKind.Ordinary )
                throw new ArgumentException ( "Method is not an ordinary method.", nameof ( methodSymbol ) );

            return SyntaxFactory.MemberAccessExpression (
                SyntaxKind.SimpleMemberAccessExpression,
                GetTypeSyntax ( methodSymbol.ContainingType ),
                SyntaxFactory.IdentifierName ( methodSymbol.Name ) );
        }

        /// <summary>
        /// Returns whether we know how to convert a string to a given type symbol.
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <returns></returns>
        public static Boolean CanConvertToFromString ( INamedTypeSymbol typeSymbol )
        {
            // Strings don't require conversion.
            if ( typeSymbol.SpecialType == SpecialType.System_String )
                return true;

            // Is Enum
            if ( typeSymbol.TypeKind == TypeKind.Enum )
                return true;

            // Has a constructor that accepts a single string
            if ( typeSymbol.Constructors.Any ( ctor =>
                ctor.Parameters.Length == 1 && ctor.Parameters[0].Type.SpecialType == SpecialType.System_String ) )
            {
                return true;
            }

            // Has a static Parse method that accepts a single string
            return GetMethodSymbol ( typeSymbol, "Parse", true, SpecialType.System_String ) is not null;
        }

        /// <summary>
        /// Returns a syntax node making the conversion of the input string to the desired type.
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <param name="inputNode"></param>
        /// <returns></returns>
        public static SyntaxNode GetConversionFromStringNode ( CommonSymbols commonSymbols, INamedTypeSymbol typeSymbol, SyntaxNode inputNode )
        {
            if ( typeSymbol.SpecialType == SpecialType.System_String )
                return inputNode;

            if ( typeSymbol.TypeKind == TypeKind.Enum )
            {
                return SyntaxFactory.InvocationExpression (
                    GetMemberAccessExpression ( commonSymbols.System_Enum_Parse_Type_String ),
                    SyntaxFactory.ArgumentList (
                        SyntaxFactory.SeparatedList ( new[]
                        {
                            GetTypeSyntax ( typeSymbol ),
                            inputNode
                        } ) ) );
            }

            if ( GetMethodSymbol ( typeSymbol, "Parse", true, SpecialType.System_String ) is IMethodSymbol parseMethodSymbol )
            {
                return SyntaxFactory.InvocationExpression (
                    GetMemberAccessExpression ( parseMethodSymbol ),
                    SyntaxFactory.ArgumentList (
                        SyntaxFactory.SeparatedList ( new[]
                        {
                            inputNode
                        } ) ) );
            }

            if ( typeSymbol.Constructors.Any ( ctor =>
                  ctor.Parameters.Length == 1 && ctor.Parameters[0].Type.SpecialType == SpecialType.System_String ) )
            {
                return SyntaxFactory.ObjectCreationExpression (
                    SyntaxFactory.ParseTypeName (
                        typeSymbol.ToDisplayString ( SymbolDisplayFormat.MinimallyQualifiedFormat ) ),
                    SyntaxFactory.ArgumentList ( SyntaxFactory.SeparatedList ( new[]
                    {
                        inputNode
                    } ) ),
                    null );
            }

            throw new InvalidOperationException ( $"Cannot convert a string to the type '{typeSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat )}'." );
        }

        /// <summary>
        /// Constructs an attribute from the provided <see cref="AttributeData" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attributeData"></param>
        /// <returns></returns>
        public static T AttributeFromAttributeData<T> ( AttributeData attributeData )
            where T : Attribute
        {
            Type type = typeof ( T );

            if ( attributeData.AttributeClass?.Name is not null && attributeData.AttributeClass?.Name != type.Name )
            {
                throw new InvalidOperationException ( "The attribute data type name does not match the attribute type name." );
            }

            // Build the instance
            var args = attributeData.ConstructorArguments.Select ( constVal => constVal.Value ).ToArray ( );
            var inst = Activator.CreateInstance ( type, args );

            // Set the property values from the named arguments
            foreach ( KeyValuePair<String, TypedConstant> namedArgument in attributeData.NamedArguments )
            {
                PropertyInfo? prop = type.GetProperty ( namedArgument.Key, BindingFlags.Public | BindingFlags.Instance );
                if ( prop is { } )
                {
                    prop.SetValue ( inst, namedArgument.Value );
                }
            }

            return ( T ) inst;
        }
    }
}