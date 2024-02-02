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
using Microsoft.CodeAnalysis.CSharp;

namespace Tsu.Trees.RedGreen.SourceGenerator.Model;

internal sealed class ScriptNode(Node node)
{
    public ScriptTypeSymbol? Base { get; } = node.BaseSymbol is null ? null : new ScriptTypeSymbol(node.BaseSymbol);
    public ScriptTypeSymbol Type { get; } = new ScriptTypeSymbol(node.TypeSymbol);
    public ImmutableArray<ScriptNode> Descendants { get; } = node.Descendants.Select(x => new ScriptNode(x)).ToImmutableArray();
    public ImmutableArray<string> Kinds { get; } = node.Kinds.Select(x => x.ToCSharpString()).ToImmutableArray();
    public ImmutableArray<ScriptComponent> Children { get; } = node.Children.Select(x => new ScriptComponent(x)).ToImmutableArray();
    public ImmutableArray<ScriptComponent> ExtraData { get; } = node.ExtraData.Select(x => new ScriptComponent(x)).ToImmutableArray();

    public IEnumerable<ScriptComponent> Components => ExtraData.Concat(Children).OrderBy(x => x.SortOrder);

    public IEnumerable<ScriptComponent> ParentComponents => Components.Where(x => x.PassToBase);

    public IEnumerable<ScriptComponent> NodeComponents => Components.Where(x => !x.PassToBase);

    public IEnumerable<ScriptComponent> ComponentsWithoutKind => Components.Where(x => x.FieldName != "_kind");

    public IEnumerable<ScriptComponent> RequiredComponents => Kinds.Length == 1 ? ComponentsWithoutKind : Components;
}
