// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tsu.Parsing.BBCode.Tree
{
    /// <summary>
    /// Represents a tag in the BBCode tree
    /// </summary>
    public class BBTagNode : BBNode
    {
        private readonly List<BBNode> _children;

        /// <summary>
        /// The tag's name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The value associated with this tag
        /// </summary>
        public string? Value { get; }

        /// <summary>
        /// Whether this is a self-closing tag
        /// </summary>
        public bool SelfClosing { get; }

        /// <summary>
        /// This tag's children
        /// </summary>
        public IReadOnlyList<BBNode> Children => _children;

        /// <summary>
        /// Initializes a new tag node
        /// </summary>
        /// <param name="selfClosing"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public BBTagNode(bool selfClosing, string name, string? value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name of tag cannot be null or composed only of whitespaces", nameof(name));

            SelfClosing = selfClosing;
            Name = name;
            Value = value;
            _children = new List<BBNode>();
        }

        /// <inheritdoc />
        public void AddChild(BBNode child)
        {
            if (SelfClosing)
                throw new InvalidOperationException("A self-closing tag cannot have children.");
            else
                _children.Add(child);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (SelfClosing)
            {
                if (Value is null)
                    return $"[{Name}/]";
                else
                    return $"[{Name}={Value}/]";
            }
            else
            {
                var concatenatedChildren = string.Join("", Children);
                if (Value is null)
                    return $"[{Name}]{concatenatedChildren}[/{Name}]";
                else
                    return $"[{Name}={Value}]{concatenatedChildren}[/{Name}]";
            }
        }

        /// <inheritdoc />
        public override bool StructurallyEquals(BBNode node)
        {
            return node is BBTagNode tagNode
                && SelfClosing == tagNode.SelfClosing
                && Name == tagNode.Name
                && Value == tagNode.Value
                && Children.Zip(tagNode.Children, (a, b) => a.StructurallyEquals(b)).All(x => x);
        }

        #region Visitor Pattern

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(IBBTreeVisitor visitor)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visitor"></param>
        /// <returns></returns>
        public override T Accept<T>(IBBTreeVisitor<T> visitor)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));

            return visitor.Visit(this);
        }

        #endregion Visitor Pattern
    }
}
