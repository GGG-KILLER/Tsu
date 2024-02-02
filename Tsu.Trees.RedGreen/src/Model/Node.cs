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
    INamedTypeSymbol? BaseSymbol,
    INamedTypeSymbol TypeSymbol,
    ImmutableArray<Node> Descendants,
    ImmutableArray<TypedConstant> Kinds,
    ImmutableArray<Component> Children,
    ImmutableArray<Component> ExtraData
)
{
    public IEnumerable<Component> Components => ExtraData.Concat(Children).OrderBy(x => x.SortOrder);

    public IEnumerable<Component> ParentComponents => Components.Where(x => x.PassToBase);

    public IEnumerable<Component> NodeComponents => Components.Where(x => !x.PassToBase);

    public IEnumerable<Component> ComponentsWithoutKind => Components.Where(x => x.FieldName != "_kind");

    public IEnumerable<Component> RequiredComponents => Kinds.Length == 1 ? ComponentsWithoutKind : Components;

    public bool Equals(Node? other) =>
        other is not null
        && SymbolEqualityComparer.Default.Equals(BaseSymbol, other.BaseSymbol)
        && SymbolEqualityComparer.Default.Equals(TypeSymbol, other.TypeSymbol)
        && Descendants.SequenceEqual(other.Descendants)
        && Kinds.SequenceEqual(other.Kinds)
        && Children.SequenceEqual(other.Children)
        && ExtraData.SequenceEqual(other.ExtraData);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(BaseSymbol, SymbolEqualityComparer.Default);
        hash.Add(TypeSymbol, SymbolEqualityComparer.Default);
        foreach (var node in Descendants)
            hash.Add(node);
        foreach (var child in Children)
            hash.Add(child);
        foreach (var kind in Kinds)
            hash.Add(kind);
        foreach (var data in ExtraData)
            hash.Add(data);
        return hash.ToHashCode();
    }
}
