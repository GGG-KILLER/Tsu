using System;
using System.Text;

namespace GUtils.Pooling
{
    /// <summary>
    /// A <see cref="Pool{T}" /> of <see cref="StringBuilder" />
    /// </summary>
    public class StringBuilderPool : Pool<StringBuilder>
    {
        /// <summary>
        /// A shared pool
        /// </summary>
        public static readonly StringBuilderPool Shared = new StringBuilderPool ( 50 );

        /// <summary>
        /// Initializes a new <see cref="StringBuilder"/> <see cref="Pool{T}"/> with a custom <paramref name="capacity"/>
        /// </summary>
        /// <param name="capacity"></param>
        public StringBuilderPool ( Int32 capacity ) : base ( capacity, true )
        {
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        protected override StringBuilder InitializeItem ( ) => new StringBuilder ( );

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="item"></param>
        protected override void ResetItem ( StringBuilder item ) => item.Clear ( );
    }
}
