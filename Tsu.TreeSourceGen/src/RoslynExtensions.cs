// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Sourced from https://github.com/dotnet/Open-XML-SDK/blob/35b91d3c18fc06379b74f0980e3498f2b04c1fc8/gen/DocumentFormat.OpenXml.Generator/RoslynExtensions.cs

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Tsu.TreeSourceGen;

internal static class RoslynExtensions
{
    public static string GetContainingNamespace(this ISymbol? symbol)
    {
        var ns = symbol?.ContainingNamespace;
        var stack = new Stack<string>();

        while (ns is not null)
        {
            if (!string.IsNullOrEmpty(ns.Name))
            {
                stack.Push(ns.Name);
            }

            ns = ns.ContainingNamespace;
        }

        return string.Join(".", stack);
    }

    public static ParentClass GetContainingTypes(this ClassDeclarationSyntax typeSyntax)
    {
        // Try and get the parent syntax. If it isn't a type like class/struct, this will be null
        TypeDeclarationSyntax? parentSyntax = typeSyntax.Parent as TypeDeclarationSyntax;
        var parentClassInfo = new ParentClass(
                typeSyntax.Keyword.ValueText,
                typeSyntax.Identifier.ToString(),
                TypeParams: typeSyntax.TypeParameterList?.ToString() ?? string.Empty,
                typeSyntax.ConstraintClauses.ToString(),
                null);

        // Keep looping while we're in a supported nested type
        while (parentSyntax != null && IsAllowedKind(parentSyntax.Kind()))
        {
            // Record the parent type keyword (class/struct etc), name, and constraints
            parentClassInfo = new(
                Keyword: parentSyntax.Keyword.ValueText,
                Name: parentSyntax.Identifier.ToString(),
                TypeParams: parentSyntax.TypeParameterList?.ToString() ?? string.Empty,
                Constraints: parentSyntax.ConstraintClauses.ToString(),
                Child: parentClassInfo); // set the child link (null initially)

            // Move to the next outer type
            parentSyntax = parentSyntax.Parent as TypeDeclarationSyntax;
        }

        // return a link to the outermost parent type
        return parentClassInfo;
    }

    // We can only be nested in class/struct/record
    private static bool IsAllowedKind(SyntaxKind kind) =>
        kind is SyntaxKind.ClassDeclaration or SyntaxKind.StructDeclaration or SyntaxKind.RecordDeclaration;
}
