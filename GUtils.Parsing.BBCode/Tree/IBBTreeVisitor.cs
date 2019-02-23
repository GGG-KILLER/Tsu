namespace GUtils.Parsing.BBCode.Tree
{
    /// <summary>
    /// Represents a <see cref="BBNode" /> tree visitor.
    /// </summary>
    public interface IBBTreeVisitor
    {
        /// <summary>
        /// Visits a <see cref="BBTagNode" />
        /// </summary>
        /// <param name="tagNode"></param>
        void Visit ( BBTagNode tagNode );

        /// <summary>
        /// Visits a <see cref="BBTextNode" />
        /// </summary>
        /// <param name="textNode"></param>
        void Visit ( BBTextNode textNode );
    }

    /// <summary>
    /// Represents a <see cref="BBNode" /> tree visitor.
    /// </summary>
    public interface IBBTreeVisitor<T>
    {
        /// <summary>
        /// Visits a <see cref="BBTagNode" />
        /// </summary>
        /// <param name="tagNode"></param>
        T Visit ( BBTagNode tagNode );

        /// <summary>
        /// Visits a <see cref="BBTextNode" />
        /// </summary>
        /// <param name="textNode"></param>
        T Visit ( BBTextNode textNode );
    }
}
