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
using System.Diagnostics.CodeAnalysis;

namespace Tsu.Parsing.BBCode.Lexing
{
    /// <summary>
    /// A BBCode token.
    /// </summary>
    internal readonly struct BBToken : IEquatable<BBToken>
    {
        /// <summary>
        /// The token type.
        /// </summary>
        public readonly BBTokenType Type;

        /// <summary>
        /// The token's value.
        /// </summary>
        public readonly string? Value;

        /// <summary>
        /// Initializes a new token.
        /// </summary>
        /// <param name="tokenType"></param>
        public BBToken(BBTokenType tokenType)
        {
            Type = tokenType;
            Value = null;
        }

        /// <summary>
        /// Initializes a new token.
        /// </summary>
        /// <param name="value"></param>
        public BBToken(string value)
        {
            Type = BBTokenType.Text;
            Value = value;
        }

        public override string ToString() =>
            $"BBToken<{Type}, {Value}>";

        #region Generated Code

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is BBToken token && Equals(token);

        /// <inheritdoc/>
        public bool Equals(BBToken other) => Type == other.Type && Value == other.Value;

        /// <summary>
        /// Returns the hash code for the token.
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Style", "IDE0070:Use 'System.HashCode'", Justification = "Not available on all target frameworks.")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Applicable to some target frameworks.")]
        public override int GetHashCode()
        {
            var hashCode = 1265339359;
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
#pragma warning disable CS8604 // Possible null reference argument.
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
#pragma warning restore CS8604 // Possible null reference argument.
            return hashCode;
        }

        /// <summary>
        /// Checks whether two tokens are equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(BBToken left, BBToken right) => left.Equals(right);

        /// <summary>
        /// Checks whether two tokens are not equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(BBToken left, BBToken right) => !(left == right);

        #endregion Generated Code
    }
}