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

namespace Tsu.Buffers
{
    /// <summary>
    /// A variable length bit vector
    /// </summary>
    public class VariableLengthBitVector : IEquatable<VariableLengthBitVector>
    {
        /// <summary>
        /// The containers that back this bit vector
        /// </summary>
        private byte[] _containers;

        /// <summary>
        /// The length of the inner array of this bit vector
        /// </summary>
        public int Length => _containers.Length;

        /// <summary>
        /// The amount of bits that this bit vector contains
        /// </summary>
        public int Bits => Length >> BitVectorHelpers.ByteShiftAmount;

        /// <summary>
        /// Initializes this <see cref="VariableLengthBitVector"/>
        /// </summary>
        public VariableLengthBitVector()
        {
            _containers = new byte[1];
        }

        /// <summary>
        /// Initializes this vector with the required amount of bits rounded up
        /// </summary>
        /// <param name="bits">The amount of bits required</param>
        public VariableLengthBitVector(int bits)
        {
            if (bits < 0)
                throw new ArgumentOutOfRangeException(nameof(bits));

            var size = bits >> BitVectorHelpers.ByteShiftAmount;
            if ((bits & BitVectorHelpers.ByteRemainderMask) != 0)
                size++;
            _containers = new byte[size];
        }

        /// <summary>
        /// Initializes this vector with the bits of another vector
        /// </summary>
        /// <param name="bitVector">The bit vector to copy from</param>
        public VariableLengthBitVector(VariableLengthBitVector bitVector)
        {
            if (bitVector is null)
                throw new ArgumentNullException(nameof(bitVector));

            _containers = (byte[]) bitVector._containers.Clone();
        }

        /// <summary>
        /// Ensures we have the required amount of containers to be able to access the provided
        /// <paramref name="index"/>
        /// </summary>
        /// <param name="index">The index being accessed</param>
        private void EnsureBitContainer(int index)
        {
            if (index >= _containers.Length)
            {
                Array.Resize(ref _containers, index + 1);
            }
        }

        /// <summary>
        /// Clears this variable length bit vector
        /// </summary>
        public void Clear() =>
            Array.Clear(_containers, 0, _containers.Length);

        /// <summary>
        /// Accesses a bit in this vector.
        /// </summary>
        /// <param name="bitIndex">The 0-based bit index</param>
        /// <returns></returns>
        public bool this[int bitIndex]
        {
            set
            {
                EnsureBitContainer(bitIndex >> BitVectorHelpers.ByteShiftAmount);
#if HAS_SPAN
                BitVectorHelpers.SetByteVectorBitValue((Span<byte>) _containers, bitIndex, value);
#else
                BitVectorHelpers.SetByteVectorBitValue(_containers, bitIndex, value);
#endif
            }
            get =>
#if HAS_SPAN
                BitVectorHelpers.GetByteVectorBitValue((ReadOnlySpan<byte>) _containers, bitIndex);
#else
                BitVectorHelpers.GetByteVectorBitValue(_containers, bitIndex);
#endif

        }

        #region IEquatable<VariableLengthBitVector>

        /// <inheritdoc/>
        public bool Equals(VariableLengthBitVector? other) =>
            other is VariableLengthBitVector
            && _containers.SequenceEqual(other._containers);

        #endregion IEquatable<VariableLengthBitVector>

        #region Object

        /// <inheritdoc/>
        public override string ToString() =>
            string.Join("", _containers.Select(n => Convert.ToString(n, 2)
                                                          .PadLeft(8, '0'))
                                      .Reverse());

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            Equals(obj as VariableLengthBitVector);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = -1534987273;
            var containers = this._containers;
            for (var i = 0; i < containers.Length; i++)
            {
                hashCode = unchecked(hashCode * -1521134295 + containers[i].GetHashCode());
            }
            return hashCode;
        }

        #endregion Object

        #region Operators

        /// <inheritdoc/>
        public static bool operator ==(VariableLengthBitVector left, VariableLengthBitVector right) =>
            EqualityComparer<VariableLengthBitVector>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(VariableLengthBitVector left, VariableLengthBitVector right) =>
            !(left == right);

        #endregion Operators
    }
}