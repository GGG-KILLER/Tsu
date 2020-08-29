using System;
using GUtils.Text.Code;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GUtils.CLI.SourceGenerator.CommandManager
{
    /// <summary>
    /// A <see cref="CommandClass" /> or <see cref="CommandMethod" />.
    /// </summary>
    internal interface ICommandClassOrMethod
    {
        /// <summary>
        /// Converts this command class or method to a syntax node.
        /// </summary>
        /// <param name="inputIdentifier"></param>
        /// <param name="spaceIndexIdentifier"></param>
        /// <param name="codeWriter"></param>
        /// <returns></returns>
        public Result<BlockSyntax, Diagnostic> ToSyntaxNode ( IdentifierNameSyntax inputIdentifier, IdentifierNameSyntax spaceIndexIdentifier );
    }
}