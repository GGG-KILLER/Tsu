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

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Tsu.Trees.RedGreen.SourceGenerator.Model;

internal sealed record Node(
    INamedTypeSymbol TypeSymbol,
    ImmutableArray<Node> Descendants,
    ImmutableArray<Child> Children,
    ImmutableArray<IParameterSymbol> ExtraData
)
{
    public bool Equals(Node? other) =>
        other is not null
        && SymbolEqualityComparer.Default.Equals(TypeSymbol, other.TypeSymbol)
        && Descendants.SequenceEqual(other.Descendants)
        && Children.SequenceEqual(other.Children)
        && ExtraData.SequenceEqual(other.ExtraData, SymbolEqualityComparer.Default.Equals);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(TypeSymbol, SymbolEqualityComparer.Default);
        foreach (var node in Descendants)
            hash.Add(node);
        foreach (var child in Children)
            hash.Add(child);
        foreach (var data in ExtraData)
            hash.Add(data, SymbolEqualityComparer.Default);
        return hash.ToHashCode();
    }
}
