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
using System.Diagnostics.CodeAnalysis;

namespace Tsu
{
    /// <summary>
    /// A unit value.
    /// </summary>
    public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
    {
        /// <summary>
        /// The unit type instance.
        /// </summary>
        public static readonly Unit Value;

        /// <inheritdoc/>
        public int CompareTo(Unit other) => 0;

        /// <inheritdoc/>
        public int CompareTo(object? obj) => 0;

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Unit;

        /// <inheritdoc/>
        public bool Equals(Unit other) => true;

        /// <inheritdoc/>
        public override int GetHashCode() => 0;

        /// <inheritdoc/>
        public override string ToString() => "()";


        /// <summary>
        /// Checks if one <see cref="Unit"/> equals other <see cref="Unit"/>.
        /// Always true because <see cref="Unit"/> can only assume 1 value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "They are required.")]
        [SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "They are required.")]
        public static bool operator ==(Unit left, Unit right) => true;

        /// <summary>
        /// Checks if one <see cref="Unit"/> is not equal to other <see cref="Unit"/>.
        /// Always false because <see cref="Unit"/> can only assume 1 value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "They are required.")]
        [SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "They are required.")]
        public static bool operator !=(Unit left, Unit right) => false;

        /// <summary>
        /// Checks if one <see cref="Unit"/> is less than other <see cref="Unit"/>.
        /// Always false because <see cref="Unit"/> can only assume 1 value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "They are required.")]
        [SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "They are required.")]
        public static bool operator <(Unit left, Unit right) => false;

        /// <summary>
        /// Checks if one <see cref="Unit"/> is less than or equal other <see cref="Unit"/>.
        /// Always true because <see cref="Unit"/> can only assume 1 value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "They are required.")]
        [SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "They are required.")]
        public static bool operator <=(Unit left, Unit right) => true;

        /// <summary>
        /// Checks if one <see cref="Unit"/> is greater than other <see cref="Unit"/>.
        /// Always false because <see cref="Unit"/> can only assume 1 value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "They are required.")]
        [SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "They are required.")]
        public static bool operator >(Unit left, Unit right) => false;

        /// <summary>
        /// Checks if one <see cref="Unit"/> is greater than or equal other <see cref="Unit"/>.
        /// Always true because <see cref="Unit"/> can only assume 1 value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "They are required.")]
        [SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "They are required.")]
        public static bool operator >=(Unit left, Unit right) => true;
    }
}