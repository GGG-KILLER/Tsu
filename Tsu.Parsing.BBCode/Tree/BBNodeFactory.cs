using System;

namespace Tsu.Parsing.BBCode.Tree
{
    /// <summary>
    /// A factory for <see cref="BBTextNode" /> and <see cref="BBTagNode" />
    /// </summary>
    public static class BBNodeFactory
    {
        /// <summary>
        /// Initializes a new <see cref="BBTextNode" /> with the provided <paramref name="value" />.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BBTextNode Text ( String value ) =>
            new BBTextNode ( value );

        /// <summary>
        /// Initializes a new self-closing <see cref="BBTagNode" /> with the provided
        /// <paramref name="name" />.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BBTagNode SelfClosingTag ( String name ) =>
            new BBTagNode ( true, name, null );

        /// <summary>
        /// Initializes a new self-closing <see cref="BBTagNode" /> with the provided
        /// <paramref name="name" /> and <paramref name="value" />.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BBTagNode SelfClosingTag ( String name, String value ) =>
            new BBTagNode ( true, name, value );

        /// <summary>
        /// Initializes a new <see cref="BBTagNode" /> with the provided <paramref name="name" />.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="children"></param>
        /// <returns></returns>
        public static BBTagNode Tag ( String name, params BBNode[] children )
        {
            var tag = new BBTagNode ( false, name, null );
            foreach ( BBNode child in children )
                tag.AddChild ( child );
            return tag;
        }

        /// <summary>
        /// Initializes a new <see cref="BBTagNode" /> with the provided <paramref name="name" /> and
        /// <paramref name="value" />.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="children"></param>
        /// <returns></returns>
        public static BBTagNode Tag ( String name, String value, params BBNode[] children )
        {
            var tag = new BBTagNode ( false, name, value );
            foreach ( BBNode child in children )
                tag.AddChild ( child );
            return tag;
        }
    }
}
