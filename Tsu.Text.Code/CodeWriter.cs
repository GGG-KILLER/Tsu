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
using System.Text;

namespace Tsu.Text.Code
{
    /// <summary>
    /// A generic code writer base class.
    /// </summary>
    public abstract class CodeWriter
    {
        /// <summary>
        /// A struct that represents an indentation level.
        /// </summary>
        [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Only applicable to this type and wouldn't make sense outside of it.")]
        public ref struct IndentationDisposable
        {
            private readonly CodeWriter codeWriter;
            private bool disposed;

            internal IndentationDisposable(CodeWriter codeWriter)
            {
                this.codeWriter = codeWriter;
                disposed = false;
            }

            /// <inheritdoc />
            public void Dispose()
            {
                if (!disposed)
                {
                    codeWriter.Outdent();
                    disposed = true;
                }
            }
        }

        /// <summary>
        /// Repeats a an <paramref name="repetitions" /> string for the number of <paramref
        /// name="repetitions" /> provided.
        /// </summary>
        /// <param name="input">The string to be repeated.</param>
        /// <param name="repetitions">
        /// The amount of times the <paramref name="input" /> will be present on the output string.
        /// </param>
        /// <returns>The generated string.</returns>
        private static string RepeatString(string input, int repetitions)
        {
            var builder = new StringBuilder(input.Length * repetitions);
            builder.Insert(0, input, repetitions);
            return builder.ToString();
        }

        private readonly string _indentationSequence;
        private int _indentation;
        private string _cachedIndentation;

        /// <summary>
        /// The indentation level of the writer.
        /// </summary>
        public int Indentation
        {
            get => _indentation;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _indentation = value;
                _cachedIndentation = RepeatString(_indentationSequence, value);
            }
        }

        /// <summary>
        /// Initializes this code writer.
        /// </summary>
        /// <param name="indentationSequence">The sequence of characters to be used as indentation.</param>
        protected CodeWriter(string indentationSequence)
        {
            _indentationSequence = indentationSequence;
            Indentation = 0;
            _cachedIndentation = string.Empty;
        }

        /// <summary>
        /// Increases the indentation level.
        /// </summary>
        public void Indent() => Indentation++;

        /// <summary>
        /// Decreases the indentation level.
        /// </summary>
        public void Outdent() => Indentation--;

        #region Write(Indented|Indentation)

        /// <summary>
        /// Writes a value to the output.
        /// </summary>
        /// <param name="value">The value to be written.</param>
        public abstract void Write(object? value);

        /// <summary>
        /// Writes a string to the output.
        /// </summary>
        /// <param name="value">The string to be written to the output.</param>
        public abstract void Write(string? value);

        /// <summary>
        /// Writes a formatted string to the output.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The format arguments.</param>
        public abstract void Write(string format, params object?[] args);

        /// <summary>
        /// Writes a formatted string to the output.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The format arguments.</param>
        public abstract void Write(IFormatProvider formatProvider, string format, params object?[] args);

        /// <summary>
        /// Writes the indentation prefix
        /// </summary>
        public void WriteIndentation() =>
            Write(_cachedIndentation);

        /// <summary>
        /// Writes a value to the output preceded by indentation.
        /// </summary>
        /// <param name="value"><inheritdoc cref="Write(object?)"/></param>
        public void WriteIndented(object? value)
        {
            WriteIndentation();
            Write(value);
        }

        /// <summary>
        /// Writes a string to the output preceded by indentation.
        /// </summary>
        /// <param name="value"><inheritdoc cref="Write(string?)"/></param>
        public void WriteIndented(string? value)
        {
            WriteIndentation();
            Write(value);
        }


        /// <summary>
        /// Writes a formatted string to the output preceded by indentation.
        /// </summary>
        /// <param name="format"><inheritdoc cref="Write(string, object?[])"/></param>
        /// <param name="args"><inheritdoc cref="Write(string, object?[])"/></param>
        [SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Another overload does this already.")]
        public void WriteIndented(string format, params object?[] args)
        {
            WriteIndentation();
            Write(format, args);
        }


        /// <summary>
        /// Writes a formatted string to the output preceded by indentation.
        /// </summary>
        /// <param name="formatProvider"><inheritdoc cref="Write(IFormatProvider, string, object?[])"/></param>
        /// <param name="format"><inheritdoc cref="Write(IFormatProvider, string, object?[])"/></param>
        /// <param name="args"><inheritdoc cref="Write(IFormatProvider, string, object?[])"/></param>
        public void WriteIndented(IFormatProvider formatProvider, string format, params object?[] args)
        {
            WriteIndentation();
            Write(formatProvider, format, args);
        }

        #endregion Write(Indented|Indentation)

        #region WriteLine(Indented)

        /// <summary>
        /// Writes a line separator to the output.
        /// </summary>
        public abstract void WriteLine();

        /// <summary>
        /// Writes a value to the output followed by a line separator.
        /// </summary>
        /// <param name="value">The value to be written to the output.</param>
        public abstract void WriteLine(object? value);

        /// <summary>
        /// Writes a string to the output followed by a line separator.
        /// </summary>
        /// <param name="value">The string to be written to the output.</param>
        public abstract void WriteLine(string? value);

        /// <summary>
        /// Writes a formatted string to the output followed by a line separator.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The format arguments.</param>
        public abstract void WriteLine(string format, params object?[] args);

        /// <summary>
        /// Writes a formatted string to the output followed by a line separator.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The format arguments.</param>
        public abstract void WriteLine(IFormatProvider formatProvider, string format, params object?[] args);

        /// <summary>
        /// Writes a value to the output preceded by indentation and followed by a line separator.
        /// </summary>
        /// <param name="value"><inheritdoc cref="WriteLine(object?)" /></param>
        public void WriteLineIndented(object? value)
        {
            WriteIndentation();
            WriteLine(value);
        }

        /// <summary>
        /// Writes a string to the output preceded by indentation and followed by a line separator.
        /// </summary>
        /// <param name="value"><inheritdoc cref="WriteLine(string?)"/></param>
        public void WriteLineIndented(string? value)
        {
            WriteIndentation();
            WriteLine(value);
        }


        /// <summary>
        /// Writes a formatted string to the output preceded by indentation and followed by a line separator.
        /// </summary>
        /// <param name="format"><inheritdoc cref="WriteLine(string, object?[])"/></param>
        /// <param name="args"><inheritdoc cref="WriteLine(string, object?[])"/></param>
        [SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Another overload does this already.")]
        public void WriteLineIndented(string format, params object?[] args)
        {
            WriteIndentation();
            WriteLine(format, args);
        }


        /// <summary>
        /// Writes a formatted string to the output preceded by indentation and followed by a line separator.
        /// </summary>
        /// <param name="formatProvider"><inheritdoc cref="WriteLine(IFormatProvider, string, object?[])"/></param>
        /// <param name="format"><inheritdoc cref="WriteLine(IFormatProvider, string, object?[])"/></param>
        /// <param name="args"><inheritdoc cref="WriteLine(IFormatProvider, string, object?[])"/></param>
        public void WriteLineIndented(IFormatProvider formatProvider, string format, params object?[] args)
        {
            WriteIndentation();
            WriteLine(formatProvider, format, args);
        }

        #endregion WriteLine(Indented)

        /// <summary>
        /// Increases the indentation before the callback and decreases it after
        /// </summary>
        /// <param name="cb"></param>
        public void WithIndentation(Action cb)
        {
            if (cb == null)
                throw new ArgumentNullException(nameof(cb));

            Indent();
            cb();
            Outdent();
        }

        /// <summary>
        /// Increases the indentation and decreases it when the <see cref="IndentationDisposable" />
        /// is disposed.
        /// </summary>
        /// <returns></returns>
        public IndentationDisposable WithIndentation() =>
            new IndentationDisposable(this);
    }
}