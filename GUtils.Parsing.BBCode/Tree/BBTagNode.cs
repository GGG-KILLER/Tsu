/*
 * Copyright © 2019 GGG KILLER <gggkiller2@gmail.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the “Software”), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
 * the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GUtils.Parsing.BBCode.Tree
{
    /// <summary>
    /// Represents a tag in the BBCode tree
    /// </summary>
    public class BBTagNode : BBNode
    {
        /// <summary>
        /// The tag's name
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// The value associated with this tag
        /// </summary>
        public String Value { get; }

        /// <summary>
        /// Whether this is a self-closing tag
        /// </summary>
        public Boolean SelfClosing { get; }

        private readonly List<BBNode> _children;

        /// <summary>
        /// This tag's children
        /// </summary>
        public IReadOnlyList<BBNode> Children => this._children;

        /// <summary>
        /// Initializes a new self-contained tag node
        /// </summary>
        /// <param name="name"></param>
        public BBTagNode ( String name )
        {
            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "Name of tag cannot be null or composed only of whitespaces", nameof ( name ) );

            this.SelfClosing = true;
            this.Name = name;
            this.Value = null;
            this._children = null;
        }

        /// <summary>
        /// Initializes a new tag node
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public BBTagNode ( String name, String value )
        {
            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "Name of tag cannot be null or composed only of whitespaces", nameof ( name ) );

            this.SelfClosing = false;
            this.Name = name;
            this.Value = value;
            this._children = new List<BBNode> ( );
        }

        /// <inheritdoc />
        public void AddChild ( BBNode child )
        {
            if ( this.SelfClosing )
                this._children.Add ( child );
            else
                throw new InvalidOperationException ( "A self-closing tag cannot have children." );
        }

        /// <inheritdoc />
        public override String ToString ( )
        {
            if ( this.SelfClosing )
                return $"[{this.Name}/]";
            else if ( this.Value is null )
                return $"[{this.Name}]{String.Join ( "", this.Children )}[/{this.Name}]";
            else
                return $"[{this.Name}={this.Value}]{String.Join ( "", this.Children )}[{this.Name}]";
        }

        /// <inheritdoc />
        public override Boolean StructurallyEquals ( BBNode node )
        {
            return node is BBTagNode tagNode
                && this.SelfClosing == tagNode.SelfClosing
                && this.Name == tagNode.Name
                && this.Value == tagNode.Value
                && this.Children.Zip ( tagNode.Children, ( a, b ) => a.StructurallyEquals ( b ) ).All ( x => x );
        }

        #region Visitor Pattern

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept ( IBBTreeVisitor visitor ) => visitor.Visit ( this );

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visitor"></param>
        /// <returns></returns>
        public override T Accept<T> ( IBBTreeVisitor<T> visitor ) => visitor.Visit ( this );

        #endregion Visitor Pattern
    }
}
