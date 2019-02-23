using System;

namespace GUtils.Parsing.BBCode.Tree
{
    /// <summary>
    /// Defines a node of the tree
    /// </summary>
    public abstract class BBNode
    {
        /// <summary>
        /// The node's value
        /// </summary>
        public abstract String Value { get; }

        /// <summary>
        /// Converts the node back into BBCode
        /// </summary>
        /// <returns></returns>
        public abstract override String ToString ( );

        #region Visitor Pattern

        /// <summary>
        /// Accepts a visitor
        /// </summary>
        /// <param name="visitor"></param>
        public abstract void Accept ( IBBTreeVisitor visitor );

        /// <summary>
        /// Accepts a visitor
        /// </summary>
        /// <param name="visitor"></param>
        public abstract T Accept<T> ( IBBTreeVisitor<T> visitor );

        #endregion Visitor Pattern
    }
}
