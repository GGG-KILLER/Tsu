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

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Tsu.CLI.SourceGenerator.CommandManager
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
        private static bool ContainsClassAttribute(ClassDeclarationSyntax classDeclarationSyntax)
        {
            foreach (var attributeList in classDeclarationSyntax.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    string name;
                    switch (attribute.Name)
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

                    if (name is CodeConstants.CommandManagerAttribute.SimplifiedName or CodeConstants.CommandManagerAttribute.Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private readonly List<ClassDeclarationSyntax> classes = new List<ClassDeclarationSyntax>();

        /// <summary>
        /// The classes that contain the command manager attribute.
        /// </summary>
        public IEnumerable<ClassDeclarationSyntax> CommandManagerClasses => classes.AsReadOnly();

        /// <inheritdoc />
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax && ContainsClassAttribute(classDeclarationSyntax))
            {
                classes.Add(classDeclarationSyntax);
            }
        }
    }
}