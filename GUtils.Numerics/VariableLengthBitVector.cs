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
using BitContainer = System.Byte;

namespace GUtils.Numerics
{
    public class VariableLengthBitVector : IEquatable<VariableLengthBitVector>
    {
        private const int elementBitCount = sizeof(BitContainer) * 8;

        private BitContainer[] containers;

        public int Length => this.containers.Length;

        public int Bits => this.Length * elementBitCount;

        public VariableLengthBitVector()
        {
            this.containers = new BitContainer[1];
        }

        public VariableLengthBitVector(int bits)
        {
            var size = Math.DivRem(bits, elementBitCount, out var rem);
            if ( rem > 0 )
            {
                size++;
            }
            this.containers = new BitContainer[size];
        }

        public VariableLengthBitVector(VariableLengthBitVector bitVector)
        {
            this.containers = (BitContainer[])bitVector.containers.Clone();
        }

        private void EnsureBitContainer(int index)
        {
            if (index >= this.containers.Length)
            {
                Array.Resize(ref this.containers, index + 1);
            }
        }

        public void Clear() =>
            Array.Clear(this.containers, 0, this.containers.Length);

        public bool this[int offset]
        {
            set
            {
                var index = Math.DivRem(offset, elementBitCount, out offset);
                this.EnsureBitContainer(index);
                var mask = 1U << offset;
                if (value)
                {
                    this.containers[index] |= (BitContainer)mask;
                }
                else
                {
                    this.containers[index] &= (BitContainer)~mask;
                }
            }
            get
            {
                var index = Math.DivRem(offset, elementBitCount, out offset);
                var mask = 1U << offset;
                return (this.containers[index] & mask) == mask;
            }
        }

        #region IEquatable<VariableLengthBitVector>

        public bool Equals(VariableLengthBitVector other) =>
            other != null
            && this.containers.SequenceEqual(other.containers);

        #endregion IEquatable<VariableLengthBitVector>

        #region Object

        public override string ToString() =>
            string.Join("", this.containers.Select(n => Convert.ToString(n, 2).PadLeft(elementBitCount, '0')).Reverse());

        public override bool Equals(object obj) =>
            this.Equals(obj as VariableLengthBitVector);

        public override int GetHashCode()
        {
            var hashCode = -1534987273;
            for (var i = 0; i < this.containers.Length; i++)
            {
                hashCode = unchecked(hashCode * -1521134295 + this.containers[i].GetHashCode());
            }
            return hashCode;
        }

        #endregion Object

        #region Operators

        public static bool operator ==(VariableLengthBitVector left, VariableLengthBitVector right) =>
            EqualityComparer<VariableLengthBitVector>.Default.Equals(left, right);

        public static bool operator !=(VariableLengthBitVector left, VariableLengthBitVector right) =>
            !(left == right);

        #endregion Operators
    }
}
