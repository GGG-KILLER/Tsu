using System;
using System.Collections.Generic;

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
        /// <inheritdoc />
        /// </summary>
        public override String Value { get; }

        private readonly List<BBNode> _children;

        /// <summary>
        /// This tag's children
        /// </summary>
        public IReadOnlyList<BBNode> Children => this._children;

        /// <summary>
        /// Initializes a new tag node
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public BBTagNode ( String name, String value )
        {
            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "Name of tag cannot be null or composed only of whitespaces", nameof ( name ) );

            this.Name = name;
            this.Value = value;
            this._children = new List<BBNode> ( );
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="child"></param>
        public void AddChild ( BBNode child ) =>
            this._children.Add ( child );

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public override String ToString ( ) =>
            this.Value != null
                ? $"[{this.Name}={this.Value}]{String.Join ( "", this.Children )}[{this.Name}]"
                : $"[{this.Name}]{String.Join ( "", this.Children )}[/{this.Name}]";

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
