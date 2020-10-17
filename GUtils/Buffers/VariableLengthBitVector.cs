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

namespace GUtils.Numerics
{
    /// <summary>
    /// A variable length bit vector
    /// </summary>
    public class VariableLengthBitVector : IEquatable<VariableLengthBitVector>
    {
        /// <summary>
        /// The amount of bits that fit on one container.
        /// </summary>
        private const Int32 ElementBitCount = sizeof ( Byte ) * 8;

        /// <summary>
        /// The amount we must shift by to convert to/from bits.
        /// </summary>
        private const Int32 ShiftAmount = 3;

        /// <summary>
        /// The mask for getting the remainder of bits.
        /// </summary>
        private const Int32 RemainderMask = ( 1 << ElementBitCount ) - 1;

        /// <summary>
        /// The containers that back this bit vector
        /// </summary>
        private Byte[] containers;

        /// <summary>
        /// The length of the inner array of this bit vector
        /// </summary>
        public Int32 Length => this.containers.Length;

        /// <summary>
        /// The amount of bits that this bit vector contains
        /// </summary>
        public Int32 Bits => this.Length >> ShiftAmount;

        /// <summary>
        /// Initializes this <see cref="VariableLengthBitVector"/>
        /// </summary>
        public VariableLengthBitVector ( )
        {
            this.containers = new Byte[1];
        }

        /// <summary>
        /// Initializes this vector with the required amount of bits rounded up
        /// </summary>
        /// <param name="bits">The amount of bits required</param>
        public VariableLengthBitVector ( Int32 bits )
        {
            if ( bits < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bits ) );

            var size = bits >> ShiftAmount;
            if ( ( bits & RemainderMask ) != 0 )
                size++;
            this.containers = new Byte[size];
        }

        /// <summary>
        /// Initializes this vector with the bits of another vector
        /// </summary>
        /// <param name="bitVector">The bit vector to copy from</param>
        public VariableLengthBitVector ( VariableLengthBitVector bitVector )
        {
            if ( bitVector is null )
                throw new ArgumentNullException ( nameof ( bitVector ) );

            this.containers = ( Byte[] ) bitVector.containers.Clone ( );
        }

        /// <summary>
        /// Ensures we have the required amount of containers to be able to access the provided
        /// <paramref name="index"/>
        /// </summary>
        /// <param name="index">The index being accessed</param>
        private void EnsureBitContainer ( Int32 index )
        {
            if ( index >= this.containers.Length )
            {
                Array.Resize ( ref this.containers, index + 1 );
            }
        }

        /// <summary>
        /// Clears this variable length bit vector
        /// </summary>
        public void Clear ( ) =>
            Array.Clear ( this.containers, 0, this.containers.Length );

        /// <summary>
        /// Accesses a bit in this vector
        /// </summary>
        /// <param name="offset">The 0-based offset</param>
        /// <returns></returns>
        public Boolean this[Int32 offset]
        {
            set
            {
                var index = offset >> ShiftAmount;
                offset &= RemainderMask;
                this.EnsureBitContainer ( index );
                var mask = 1 << offset;
                if ( value )
                {
                    this.containers[index] |= ( Byte ) mask;
                }
                else
                {
                    this.containers[index] &= ( Byte ) ~mask;
                }
            }
            get
            {
                var index = offset >> ShiftAmount;
                offset &= RemainderMask;
                var mask = 1U << offset;
                return ( this.containers[index] & mask ) != 0;
            }
        }

        #region IEquatable<VariableLengthBitVector>

        /// <inheritdoc/>
        public Boolean Equals ( VariableLengthBitVector? other ) =>
            other is VariableLengthBitVector
            && this.containers.SequenceEqual ( other.containers );

        #endregion IEquatable<VariableLengthBitVector>

        #region Object

        /// <inheritdoc/>
        public override String ToString ( ) =>
            String.Join ( "", this.containers.Select ( n => Convert.ToString ( n, 2 ).PadLeft ( ElementBitCount, '0' ) ).Reverse ( ) );

        /// <inheritdoc/>
        public override Boolean Equals ( Object? obj ) =>
            this.Equals ( obj as VariableLengthBitVector );

        /// <inheritdoc/>
        public override Int32 GetHashCode ( )
        {
            var hashCode = -1534987273;
            var containers = this.containers;
            for ( var i = 0; i < containers.Length; i++ )
            {
                hashCode = unchecked(hashCode * -1521134295 + containers[i].GetHashCode ( ));
            }
            return hashCode;
        }

        #endregion Object

        #region Operators

        /// <inheritdoc/>
        public static Boolean operator == ( VariableLengthBitVector left, VariableLengthBitVector right ) =>
            EqualityComparer<VariableLengthBitVector>.Default.Equals ( left, right );

        /// <inheritdoc/>
        public static Boolean operator != ( VariableLengthBitVector left, VariableLengthBitVector right ) =>
            !( left == right );

        #endregion Operators
    }
}