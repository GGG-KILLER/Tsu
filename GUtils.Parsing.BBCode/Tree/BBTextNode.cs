using System;

namespace GUtils.Parsing.BBCode.Tree
{
    /// <summary>
    /// Represents a text node in the tree
    /// </summary>
    public class BBTextNode : BBNode
    {
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override String Value { get; }

        /// <summary>
        /// Initializes a new text node
        /// </summary>
        /// <param name="value"></param>
        public BBTextNode ( String value )
        {
            this.Value = value;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public override String ToString ( ) => this.Value;

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
