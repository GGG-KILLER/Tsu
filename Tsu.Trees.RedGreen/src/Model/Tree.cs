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

using Microsoft.CodeAnalysis;

namespace Tsu.Trees.RedGreen.SourceGenerator.Model;

internal sealed record Tree(
    INamedTypeSymbol GreenBase,
    INamedTypeSymbol RedBase,
    Node Root,
    string Suffix,
    INamedTypeSymbol KindEnum,
    bool CreateVisitors,
    bool CreateWalker,
    bool CreateRewriter,
    bool CreateLists,
    bool DebugDump
)
{
    public string ToRedCSharp(ITypeSymbol symbol, bool addNullable = false) =>
        symbol.DerivesFrom(GreenBase)
        ? symbol.ToCSharpString(addNullable)
                .Replace(GreenBase.ContainingNamespace.ToCSharpString(false), RedBase.ContainingNamespace.ToCSharpString(false))
        : symbol.ToCSharpString(addNullable);
}
