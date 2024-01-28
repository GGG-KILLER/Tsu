// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
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
using System.Text;

namespace Tsu.Text.Code
{
    /// <summary>
    /// Defines a code writer
    /// </summary>
    public class StringBuilderCodeWriter : CodeWriter
    {
        private readonly StringBuilder _builder;

        /// <summary>
        /// Initializes this class
        /// </summary>
        public StringBuilderCodeWriter(string indentationSequence) : base(indentationSequence)
        {
            _builder = new StringBuilder();
        }

        #region Write

        /// <inheritdoc/>
        public override void Write(object? value) => _builder.Append(value);

        /// <inheritdoc/>
        public override void Write(string? value) => _builder.Append(value);

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Another overload does that already.")]
        public override void Write(string format, params object?[] args) => _builder.AppendFormat(format, args);

        /// <inheritdoc/>
        public override void Write(IFormatProvider formatProvider, string format, params object?[] args) => _builder.AppendFormat(formatProvider, format, args);

        #endregion Write

        #region WriteLine

        /// <inheritdoc/>
        public override void WriteLine() => _builder.AppendLine();

        /// <inheritdoc/>
        public override void WriteLine(object? value) => _builder.Append(value).AppendLine();

        /// <inheritdoc/>
        public override void WriteLine(string? value) => _builder.AppendLine(value);

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Another overload does that already.")]
        public override void WriteLine(string format, params object?[] args) => _builder.AppendFormat(format, args).AppendLine();

        /// <inheritdoc/>
        public override void WriteLine(IFormatProvider formatProvider, string format, params object?[] args) => _builder.AppendFormat(formatProvider, format, args).AppendLine();

        #endregion WriteLine

        /// <summary>
        /// Resets the writer
        /// </summary>
        public void Reset()
        {
            _builder.Clear();
            Indentation = 0;
        }

        /// <inheritdoc />
        public override string ToString() => _builder.ToString();
    }
}