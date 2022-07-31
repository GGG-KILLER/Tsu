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

using System;
using Microsoft.CodeAnalysis;
using Tsu.CLI.Commands;

namespace Tsu.CLI.SourceGenerator.CommandManager
{
    public class CommonSymbols
    {
        /// <summary>
        /// The <see cref="CommandAttribute" /> type symbol.
        /// </summary>
        public INamedTypeSymbol Tsu_CLI_Commands_CommandAttribute { get; }

        /// <summary>
        /// The <see cref="HelpDescriptionAttribute" /> type symbol.
        /// </summary>
        public INamedTypeSymbol Tsu_CLI_Commands_HelpDescriptionAttribute { get; }

        /// <summary>
        /// The <see cref="HelpExampleAttribute" /> type symbol.
        /// </summary>
        public INamedTypeSymbol Tsu_CLI_Commands_HelpExampleAttribute { get; }

        /// <summary>
        /// The <see cref="JoinRestOfArgumentsAttribute" /> type symbol.
        /// </summary>
        public INamedTypeSymbol Tsu_CLI_Commands_JoinRestOfArgumentsAttribute { get; }

        /// <summary>
        /// The <see cref="RawInputAttribute" /> type symbol.
        /// </summary>
        public INamedTypeSymbol Tsu_CLI_Commands_RawInputAttribute { get; }

        /// <summary>
        /// The <see cref="Type" /> type symbol.
        /// </summary>
        public INamedTypeSymbol System_Type { get; }

        /// <summary>
        /// The <see cref="string" /> type symbol.
        /// </summary>
        public INamedTypeSymbol System_String { get; }

        /// <summary>
        /// The <see cref="string.IsNullOrWhiteSpace(string)" /> method symbol.
        /// </summary>
        public IMethodSymbol System_String__IsNullOrWhiteSpaceString { get; }

        /// <summary>
        /// The <see cref="string.Trim()" /> method symbol.
        /// </summary>
        public IMethodSymbol System_String__Trim { get; }

        /// <summary>
        /// The <see cref="string.IndexOf(char)" /> method symbol.
        /// </summary>
        public IMethodSymbol System_String__IndexOfChar { get; }

        /// <summary>
        /// The <see cref="string.IndexOf(char, int)" /> method symbol.
        /// </summary>
        public IMethodSymbol System_String__IndexOfCharInt32 { get; }

        /// <summary>
        /// The <see cref="string.Substring(int, int)" /> method symbol.
        /// </summary>
        public IMethodSymbol System_String__SubstringInt32Int32 { get; }

        /// <summary>
        /// The <see cref="Enum" /> type symbol.
        /// </summary>
        public INamedTypeSymbol System_Enum { get; }

        /// <summary>
        /// The <see cref="Enum.Parse(Type, string)" /> method symbol.
        /// </summary>
        public IMethodSymbol System_Enum__ParseTypeString { get; }

        /// <summary>
        /// The <see cref="int" /> type symbol.
        /// </summary>
        public INamedTypeSymbol System_Int32 { get; }

        public CommonSymbols(Compilation compilation)
        {
            Tsu_CLI_Commands_CommandAttribute = getSymbol(typeof(CommandAttribute));
            Tsu_CLI_Commands_HelpDescriptionAttribute = getSymbol(typeof(HelpDescriptionAttribute));
            Tsu_CLI_Commands_HelpExampleAttribute = getSymbol(typeof(HelpExampleAttribute));
#pragma warning disable CS0618 // Type or member is obsolete
            Tsu_CLI_Commands_JoinRestOfArgumentsAttribute = getSymbol(typeof(JoinRestOfArgumentsAttribute));
#pragma warning restore CS0618 // Type or member is obsolete
            Tsu_CLI_Commands_RawInputAttribute = getSymbol(typeof(RawInputAttribute));
            System_Type = getSymbol(typeof(Type));
            System_Int32 = compilation.GetSpecialType(SpecialType.System_Int32);
            System_String = compilation.GetSpecialType(SpecialType.System_String);
            System_String__IsNullOrWhiteSpaceString = getMethodSymbol(System_String, nameof(String.IsNullOrWhiteSpace), true, System_String);
            System_String__Trim = getMethodSymbol(System_String, nameof(String.Trim), false);
            System_String__IndexOfChar = getMethodSymbol(System_String, nameof(String.IndexOf), false, SpecialType.System_Char);
            System_String__IndexOfCharInt32 = getMethodSymbol(System_String, nameof(String.IndexOf), false, SpecialType.System_Char, System_Int32);
            System_String__SubstringInt32Int32 = getMethodSymbol(System_String, nameof(String.Substring), false, System_Int32, System_Int32);
            System_Enum = compilation.GetSpecialType(SpecialType.System_Enum);
            System_Enum__ParseTypeString = getMethodSymbol(System_Enum, "Parse", true, System_Type, System_String);

            INamedTypeSymbol getSymbol(Type type) =>
                compilation.GetTypeByMetadataName(type.FullName)
                ?? throw new InvalidOperationException($"{type.FullName} type symbol not found.");

            static IMethodSymbol getMethodSymbol(ITypeSymbol typeSymbol, string name, bool isStatic, params object[] paramsTypes) =>
                Utilities.GetMethodSymbol(typeSymbol, name, isStatic, paramsTypes)
                ?? throw new InvalidOperationException($"{typeSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)}.{name} method symbol not found.");
        }
    }
}