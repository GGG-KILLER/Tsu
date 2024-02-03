// Copyright © 2024 GGG KILLER <gggkiller2@gmail.com>
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

using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class Utils
{
    public static bool DerivesFrom(this ITypeSymbol symbol, INamedTypeSymbol parent)
    {
        for (var type = symbol; type is not null; type = type.BaseType)
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

    public static string ToCSharpString(this ITypeSymbol symbol, bool addNullable = true)
    {
        var str = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        if (addNullable && symbol.NullableAnnotation is NullableAnnotation.Annotated)
            str += '?';
        return str;
    }

    public static string ToCSharpString(this INamespaceSymbol symbol, bool noGlobal = true)
    {
        var str = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        return noGlobal ? str.Substring("global::".Length) : str;
    }

    public static SourceText ToSourceText(this StringBuilder builder) =>
        SourceText.From(new StringBuilderReader(builder), builder.Length, Encoding.UTF8);

    public static void WriteLines(this IndentedTextWriter writer, string text)
    {
        var initialIndent = writer.Indent;
        var lines = text.Split('\n');
        foreach (var l in lines)
        {
            var line = l.TrimEnd();
            if (line.Length == 0)
            {
                writer.WriteLineNoTabs("");
            }
            else
            {
                var indents = countIndents(line, 4);
                if (writer.Indent != initialIndent + indents)
                    writer.Indent = initialIndent + indents;
                writer.WriteLine(line.TrimStart());
            }
        }

        static int countIndents(string str, int spaces)
        {
            var n = 0;
            for (var idx = 0; idx < str.Length; idx++)
            {
                if (str[idx] == ' ')
                    n++;
                else
                    break;
            }
            return n / spaces;
        }
    }

    public static void WriteLines(this IndentedTextWriter writer, string text, params object[] args)
    {
        var initialIndent = writer.Indent;
        var lines = text.Split('\n');
        foreach (var l in lines)
        {
            var line = l.TrimEnd();
            if (line.Length == 0)
            {
                writer.WriteLineNoTabs("");
            }
            else
            {
                var indents = countIndents(line, 4);
                if (writer.Indent != initialIndent + indents)
                    writer.Indent = initialIndent + indents;
                writer.WriteLine(line.TrimStart(), args);
            }
        }

        static int countIndents(string str, int spaces)
        {
            var n = 0;
            for (var idx = 0; idx < str.Length; idx++)
            {
                if (str[idx] == ' ')
                    n++;
                else
                    break;
            }
            return n / spaces;
        }
    }
}