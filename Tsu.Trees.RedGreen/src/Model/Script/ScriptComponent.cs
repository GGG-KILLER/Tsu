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

internal sealed class ScriptComponent(Component component)
{
    public bool IsList => component.IsList;
    public ScriptTypeSymbol Type { get; } = new ScriptTypeSymbol((INamedTypeSymbol) component.Type);
    public string FieldName => component.FieldName;
    public string ParameterName => component.ParameterName;
    public string PropertyName => component.PropertyName;
    public bool IsOptional => component.IsOptional;
    public bool PassToBase => component.PassToBase;
    public int Order => component.Order;
    public int SortOrder => component.SortOrder;
}