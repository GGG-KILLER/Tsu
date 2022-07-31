// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
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

namespace Tsu.Parsing.BBCode.Tree
{
    /// <summary>
    /// Defines a node of the tree
    /// </summary>
    public abstract class BBNode
    {
        /// <summary>
        /// Whether this node is structurally equivalent to another node
        /// </summary>
        /// <param name="node">The node to compare against</param>
        /// <returns></returns>
        public abstract bool StructurallyEquals(BBNode node);

        /// <summary>
        /// Converts the node back into BBCode
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();

        #region Visitor Pattern

        /// <summary>
        /// Accepts a visitor
        /// </summary>
        /// <param name="visitor"></param>
        public abstract void Accept(IBBTreeVisitor visitor);

        /// <summary>
        /// Accepts a visitor
        /// </summary>
        /// <param name="visitor"></param>
        public abstract T Accept<T>(IBBTreeVisitor<T> visitor);

        #endregion Visitor Pattern
    }
}
