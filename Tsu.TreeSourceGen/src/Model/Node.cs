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

namespace Tsu.TreeSourceGen;

internal sealed class Node(ParentClass parentClass, INamedTypeSymbol typeSymbol, string? name) : IEquatable<Node>
{
    public ParentClass ParentClass { get; } = parentClass;
    public INamedTypeSymbol TypeSymbol { get; } = typeSymbol;
    public string? Name { get; } = name;
    public string Namespace { get; } = typeSymbol.GetContainingNamespace();

    public override bool Equals(object obj) => Equals(obj as Node);
    public bool Equals(Node? other) =>
        SymbolEqualityComparer.Default.Equals(TypeSymbol, other?.TypeSymbol)
        && string.Equals(Name, other.Name);

    public override int GetHashCode() =>
        HashCode.Combine(SymbolEqualityComparer.Default.GetHashCode(TypeSymbol), Name);
}
