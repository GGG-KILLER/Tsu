using System;
using Tsu.CLI.Commands;
using Microsoft.CodeAnalysis;

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
        /// The <see cref="String" /> type symbol.
        /// </summary>
        public INamedTypeSymbol System_String { get; }

        /// <summary>
        /// The <see cref="String.IsNullOrWhiteSpace(String)" /> method symbol.
        /// </summary>
        public IMethodSymbol System_String__IsNullOrWhiteSpaceString { get; }

        /// <summary>
        /// The <see cref="String.Trim()" /> method symbol.
        /// </summary>
        public IMethodSymbol System_String__Trim { get; }

        /// <summary>
        /// The <see cref="String.IndexOf(Char)" /> method symbol.
        /// </summary>
        public IMethodSymbol System_String__IndexOfChar { get; }

        /// <summary>
        /// The <see cref="String.IndexOf(Char, Int32)" /> method symbol.
        /// </summary>
        public IMethodSymbol System_String__IndexOfCharInt32 { get; }

        /// <summary>
        /// The <see cref="String.Substring(Int32, Int32)" /> method symbol.
        /// </summary>
        public IMethodSymbol System_String__SubstringInt32Int32 { get; }

        /// <summary>
        /// The <see cref="Enum" /> type symbol.
        /// </summary>
        public INamedTypeSymbol System_Enum { get; }

        /// <summary>
        /// The <see cref="Enum.Parse(Type, String)" /> method symbol.
        /// </summary>
        public IMethodSymbol System_Enum__ParseTypeString { get; }

        /// <summary>
        /// The <see cref="Int32" /> type symbol.
        /// </summary>
        public INamedTypeSymbol System_Int32 { get; }

        public CommonSymbols ( Compilation compilation )
        {
            this.Tsu_CLI_Commands_CommandAttribute = getSymbol ( typeof ( CommandAttribute ) );
            this.Tsu_CLI_Commands_HelpDescriptionAttribute = getSymbol ( typeof ( HelpDescriptionAttribute ) );
            this.Tsu_CLI_Commands_HelpExampleAttribute = getSymbol ( typeof ( HelpExampleAttribute ) );
#pragma warning disable CS0618 // Type or member is obsolete
            this.Tsu_CLI_Commands_JoinRestOfArgumentsAttribute = getSymbol ( typeof ( JoinRestOfArgumentsAttribute ) );
#pragma warning restore CS0618 // Type or member is obsolete
            this.Tsu_CLI_Commands_RawInputAttribute = getSymbol ( typeof ( RawInputAttribute ) );
            this.System_Type = getSymbol ( typeof ( Type ) );
            this.System_String = compilation.GetSpecialType ( SpecialType.System_String );
            this.System_String__IsNullOrWhiteSpaceString = getMethodSymbol ( this.System_String, nameof ( String.IsNullOrWhiteSpace ), true, this.System_String );
            this.System_String__Trim = getMethodSymbol ( this.System_String, nameof ( String.Trim ), false );
            this.System_String__IndexOfChar = getMethodSymbol ( this.System_String, nameof ( String.IndexOf ), false, SpecialType.System_Char );
            this.System_String__IndexOfCharInt32 = getMethodSymbol ( this.System_String, nameof ( String.IndexOf ), false, SpecialType.System_Char, this.System_Int32 );
            this.System_String__SubstringInt32Int32 = getMethodSymbol ( this.System_String, nameof ( String.Substring ), false, this.System_Int32, this.System_Int32 );
            this.System_Enum = compilation.GetSpecialType ( SpecialType.System_Enum );
            this.System_Enum__ParseTypeString = getMethodSymbol ( this.System_Enum, "Parse", true, this.System_Type, this.System_String );
            this.System_Int32 = compilation.GetSpecialType ( SpecialType.System_Int32 );

            INamedTypeSymbol getSymbol ( Type type ) =>
                compilation.GetTypeByMetadataName ( type.FullName )
                ?? throw new InvalidOperationException ( $"{type.FullName} type symbol not found." );

            static IMethodSymbol getMethodSymbol ( ITypeSymbol typeSymbol, String name, Boolean isStatic, params Object[] paramsTypes ) =>
                Utilities.GetMethodSymbol ( typeSymbol, name, isStatic, paramsTypes )
                ?? throw new InvalidOperationException ( $"{typeSymbol.ToDisplayString ( SymbolDisplayFormat.CSharpErrorMessageFormat )}.{name} method symbol not found." );
        }
    }
}