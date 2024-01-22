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

internal sealed class Tree(INamedTypeSymbol root, IEnumerable<Node> nodes) : IEquatable<Tree?>
{
    public INamedTypeSymbol Root { get; } = root;
    public IEnumerable<Node> Nodes { get; } = nodes;

    public override bool Equals(object obj) => Equals(obj as Tree);
    public bool Equals(Tree? other) => SymbolEqualityComparer.Default.Equals(Root, other?.Root) && Nodes.SequenceEqual(other.Nodes);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Root, SymbolEqualityComparer.Default);
        foreach (var node in Nodes)
            hash.Add(node);
        return hash.ToHashCode();
    }
}
