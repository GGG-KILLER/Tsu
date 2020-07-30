using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GUtils.CLI.SourceGenerator
{
    /// <summary>
    /// The command manager syntax receiver that accummulates the class declarations that contain an
    /// attribute with the same name as the command manager attribute.
    /// </summary>
    public class SyntaxReceiver : ISyntaxReceiver
    {
        /// <summary>
        /// Checks if the class contains the command manager attribute (only checks names, can't
        /// check the symbol itself).
        /// </summary>
        /// <param name="classDeclarationSyntax">The class declaration to check for attributes.</param>
        /// <returns>Whether the provided class declaration contains the command manager attribute.</returns>
        private static Boolean ContainsClassAttribute ( ClassDeclarationSyntax classDeclarationSyntax )
        {
            foreach ( AttributeListSyntax attributeList in classDeclarationSyntax.AttributeLists )
            {
                foreach ( AttributeSyntax attribute in attributeList.Attributes )
                {
                    String name;
                    switch ( attribute.Name )
                    {
                        case SimpleNameSyntax simpleNameSyntax:
                            name = simpleNameSyntax.Identifier.ValueText;
                            break;

                        case QualifiedNameSyntax qualifiedNameSyntax:
                            name = qualifiedNameSyntax.Right.Identifier.ValueText;
                            break;

                        case AliasQualifiedNameSyntax aliasQualifiedNameSyntax:
                            name = aliasQualifiedNameSyntax.Name.Identifier.ValueText;
                            break;

                        default:
                            continue;
                    }

                    if ( name is CodeConstants.CommandManagerAttribute.SimplifiedName or CodeConstants.CommandManagerAttribute.Name )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private readonly List<ClassDeclarationSyntax> classes = new List<ClassDeclarationSyntax> ( );

        /// <summary>
        /// The classes that contain the command manager attribute.
        /// </summary>
        public IEnumerable<ClassDeclarationSyntax> CommandManagerClasses => this.classes.AsReadOnly ( );

        /// <inheritdoc />
        public void OnVisitSyntaxNode ( SyntaxNode syntaxNode )
        {
            if ( syntaxNode is ClassDeclarationSyntax classDeclarationSyntax && ContainsClassAttribute ( classDeclarationSyntax ) )
            {
                this.classes.Add ( classDeclarationSyntax );
            }
        }
    }
}