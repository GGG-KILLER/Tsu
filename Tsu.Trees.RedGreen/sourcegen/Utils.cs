using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class Utils
{
    public static bool DerivesFrom(this ITypeSymbol symbol, INamedTypeSymbol parent)
    {
        if (symbol.BaseType is null)
            return false;

        for (var type = symbol.BaseType; type is not null; type = type.BaseType)
        {
            if (SymbolEqualityComparer.Default.Equals(type, parent))
                return true;
        }

        return false;
    }

    public static string ToCamelCase(this string str) =>
        string.Concat(char.ToLowerInvariant(str[0]), str.Substring(1));

    public static string ToPascalCase(this string str) =>
        string.Concat(char.ToUpperInvariant(str[0]), str.Substring(1));

    public static string WithoutSuffix(this string name, string suffix)
    {
        if (name.EndsWith(suffix, StringComparison.Ordinal))
            return name.Substring(0, name.Length - suffix.Length);
        else
            return name;
    }

    public static string ToCSharpString(this Accessibility accessibility) =>
        accessibility switch
        {
            Accessibility.Private => "private",
            Accessibility.ProtectedAndInternal => "private protected",
            Accessibility.Protected => "protected",
            Accessibility.Internal => "internal",
            Accessibility.ProtectedOrInternal => "protected internal",
            Accessibility.Public => "public",
            _ => throw new NotImplementedException()
        };

    public static string ToCSharpString(this ISymbol symbol) => symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

    public static string ToCSharpString(this INamespaceSymbol symbol) => symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Substring("global::".Length);

    public static SourceText ToSourceText(this StringBuilder builder) =>
        SourceText.From(new StringBuilderReader(builder), builder.Length, Encoding.UTF8);
}