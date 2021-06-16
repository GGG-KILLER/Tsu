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
using System.IO;

namespace Tsu.Text.Code
{
    /// <summary>
    /// A <see cref="TextWriter" /> based <see cref="CodeWriter" />.
    /// </summary>
    public class TextWriterCodeWriter : CodeWriter, IDisposable
    {
        private readonly TextWriter _writer;
        private readonly bool _keepOpen;
        private bool _disposedValue;

        /// <summary>
        /// Initializes a new <see cref="TextWriterCodeWriter" />.
        /// </summary>
        /// <param name="indentationSequence"><inheritdoc cref="CodeWriter(string)" /></param>
        /// <param name="writer">The <see cref="TextWriter" /> to be used by this code writer.</param>
        public TextWriterCodeWriter(string indentationSequence, TextWriter writer) : this(indentationSequence, writer, false)
        {
        }

        /// <summary>
        /// <inheritdoc cref="TextWriterCodeWriter(string, TextWriter)" />
        /// </summary>
        /// <param name="indentationSequence">
        /// <inheritdoc cref="TextWriterCodeWriter(string, TextWriter)" />
        /// </param>
        /// <param name="writer"><inheritdoc cref="TextWriterCodeWriter(string, TextWriter)" /></param>
        /// <param name="keepOpen">Whether to keep the <paramref name="writer" /> open.</param>
        public TextWriterCodeWriter(string indentationSequence, TextWriter writer, bool keepOpen) : base(indentationSequence)
        {
            _writer = writer;
            _keepOpen = keepOpen;
        }

        #region Write(Line)

        /// <inheritdoc />
        public override void Write(object? value) => _writer.Write(value);

        /// <inheritdoc />
        public override void Write(string? value) => _writer.Write(value);

        /// <inheritdoc />
        public override void Write(string format, params object?[] args) => _writer.Write(format, args);

        /// <inheritdoc/>
        public override void Write(IFormatProvider formatProvider, string format, params object?[] args) => _writer.Write(string.Format(formatProvider, format, args));

        /// <inheritdoc />
        public override void WriteLine() => _writer.WriteLine();

        /// <inheritdoc />
        public override void WriteLine(object? value) => _writer.WriteLine(value);

        /// <inheritdoc />
        public override void WriteLine(string? value) => _writer.WriteLine(value);

        /// <inheritdoc />
        public override void WriteLine(string format, params object?[] args) => _writer.WriteLine(format, args);

        /// <inheritdoc/>
        public override void WriteLine(IFormatProvider formatProvider, string format, params object?[] args) => _writer.WriteLine(string.Format(formatProvider, format, args));

        #endregion Write(Line)

        #region IDisposable

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        /// <param name="disposing">
        /// If this was triggered by <see cref="Dispose()" /> or the class' destructor.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing && !_keepOpen)
                {
                    _writer.Dispose();
                }

                _disposedValue = true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}