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

namespace GUtils.Parsing.BBCode.Lexing
{
    internal readonly struct BBToken : IEquatable<BBToken>
    {
        public readonly BBTokenType Type;
        public readonly String Value;

        public BBToken ( BBTokenType tokenType )
        {
            this.Type = tokenType;
            this.Value = null;
        }

        public BBToken ( String value )
        {
            this.Type = BBTokenType.Text;
            this.Value = value;
        }

        public override String ToString ( ) =>
            $"BBToken<{this.Type}, {this.Value}>";

        #region Generated Code

        public override Boolean Equals ( Object obj ) => obj is BBToken token && this.Equals ( token );

        public Boolean Equals ( BBToken other ) => this.Type == other.Type && this.Value == other.Value;

        public override Int32 GetHashCode ( )
        {
            var hashCode = 1265339359;
            hashCode = hashCode * -1521134295 + this.Type.GetHashCode ( );
            hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode ( this.Value );
            return hashCode;
        }

        public static Boolean operator == ( BBToken left, BBToken right ) => left.Equals ( right );

        public static Boolean operator != ( BBToken left, BBToken right ) => !( left == right );

        #endregion Generated Code
    }
}