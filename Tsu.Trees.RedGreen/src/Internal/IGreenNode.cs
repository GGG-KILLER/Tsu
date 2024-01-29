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

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Tsu.Trees.RedGreen.Internal;

/// <summary>
/// The base class for a green tree node.
/// </summary>
/// <remarks>
/// This is strictly for use by the generated code and libraries, you should never access green nodes yourself.
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IGreenNode<TGreenRoot, TRedRoot, TKind>
    where TGreenRoot : class, IGreenNode<TGreenRoot, TRedRoot, TKind>
    where TRedRoot : class, IRedNode<TGreenRoot, TRedRoot, TKind>
    where TKind : Enum
{
    /// <summary>
    /// The kind of node this is.
    /// </summary>
    TKind Kind { get; }

    /// <summary>
    /// The number of slots this node contains.
    /// </summary>
    int SlotCount { get; }

    /// <summary>
    /// Obtains the node at the given slot.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    TGreenRoot? GetSlot(int index);

    /// <summary>
    /// Ensures the node at the provided slot will not be null and returns it.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    TGreenRoot GetRequiredSlot(int index);

    /// <summary>
    /// Enumerates the direct children that are under this node.
    /// </summary>
    /// <returns></returns>
    IEnumerable<TGreenRoot> ChildNodes();

    /// <summary>
    /// Enumerate all descendants of this tree in depth-first order.
    /// </summary>
    /// <returns></returns>
    IEnumerable<TGreenRoot> EnumerateDescendants();

    /// <summary>
    /// Returns whether this given node is equivalent to another.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    bool IsEquivalentTo([NotNullWhen(true)] TGreenRoot? other);

    /// <summary>
    /// Creates a red version of this node without a parent.
    /// </summary>
    /// <returns></returns>
    TRedRoot CreateRed();

    /// <summary>
    /// Creates a red version of this node with the specified parent.
    /// </summary>
    /// <param name="parent">
    /// The node to specify as the parent for this node.
    /// </param>
    /// <returns></returns>
    TRedRoot CreateRed(TRedRoot? parent);
}
