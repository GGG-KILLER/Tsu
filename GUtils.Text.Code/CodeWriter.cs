using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GUtils.Text.Code
{
    /// <summary>
    /// A generic code writer base class.
    /// </summary>
    public abstract class CodeWriter
    {
        /// <summary>
        /// A struct that represents an indentation level.
        /// </summary>
        [SuppressMessage ( "Design", "CA1034:Nested types should not be visible", Justification = "Only applicable to this type and wouldn't make sense outside of it." )]
        public ref struct IndentationDisposable
        {
            private readonly CodeWriter codeWriter;
            private Boolean disposed;

            internal IndentationDisposable ( CodeWriter codeWriter )
            {
                this.codeWriter = codeWriter;
                this.disposed = false;
            }

            /// <inheritdoc />
            public void Dispose ( )
            {
                if ( !this.disposed )
                {
                    this.codeWriter.Outdent ( );
                    this.disposed = true;
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
        private static String RepeatString ( String input, Int32 repetitions )
        {
            var builder = new StringBuilder ( input.Length * repetitions );
            builder.Insert ( 0, input, repetitions );
            return builder.ToString ( );
        }

        private readonly String _indentationSequence;
        private Int32 _indentation;
        private String _cachedIndentation;

        /// <summary>
        /// The indentation level of the writer.
        /// </summary>
        public Int32 Indentation
        {
            get => this._indentation;
            set
            {
                if ( value < 0 )
                    throw new ArgumentOutOfRangeException ( nameof ( value ) );

                this._indentation = value;
                this._cachedIndentation = RepeatString ( this._indentationSequence, value );
            }
        }

        /// <summary>
        /// Initializes this code writer.
        /// </summary>
        /// <param name="indentationSequence">The sequence of characters to be used as indentation.</param>
        protected CodeWriter ( String indentationSequence )
        {
            this._indentationSequence = indentationSequence;
            this.Indentation = 0;
            this._cachedIndentation = String.Empty;
        }

        /// <summary>
        /// Increases the indentation level.
        /// </summary>
        public void Indent ( ) => this.Indentation++;

        /// <summary>
        /// Decreases the indentation level.
        /// </summary>
        public void Outdent ( ) => this.Indentation--;

        #region Write(Indented|Indentation)

        /// <summary>
        /// Writes a value to the output.
        /// </summary>
        /// <param name="value">The value to be written.</param>
        public abstract void Write ( Object? value );

        /// <summary>
        /// Writes a string to the output.
        /// </summary>
        /// <param name="value">The string to be written to the output.</param>
        public abstract void Write ( String? value );

        /// <summary>
        /// Writes a formatted string to the output.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The format arguments.</param>
        public abstract void Write ( String format, params Object?[] args );

        /// <summary>
        /// Writes a formatted string to the output.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The format arguments.</param>
        public abstract void Write ( IFormatProvider formatProvider, String format, params Object?[] args );

        /// <summary>
        /// Writes the indentation prefix
        /// </summary>
        public void WriteIndentation ( ) =>
            this.Write ( this._cachedIndentation );

        /// <summary>
        /// Writes a value to the output preceded by indentation.
        /// </summary>
        /// <param name="value"><inheritdoc cref="Write(Object?)"/></param>
        public void WriteIndented ( Object? value )
        {
            this.WriteIndentation ( );
            this.Write ( value );
        }

        /// <summary>
        /// Writes a string to the output preceded by indentation.
        /// </summary>
        /// <param name="value"><inheritdoc cref="Write(String?)"/></param>
        public void WriteIndented ( String? value )
        {
            this.WriteIndentation ( );
            this.Write ( value );
        }


        /// <summary>
        /// Writes a formatted string to the output preceded by indentation.
        /// </summary>
        /// <param name="format"><inheritdoc cref="Write(String, Object?[])"/></param>
        /// <param name="args"><inheritdoc cref="Write(String, Object?[])"/></param>
        [SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "Another overload does this already." )]
        public void WriteIndented ( String format, params Object?[] args )
        {
            this.WriteIndentation ( );
            this.Write ( format, args );
        }


        /// <summary>
        /// Writes a formatted string to the output preceded by indentation.
        /// </summary>
        /// <param name="formatProvider"><inheritdoc cref="Write(IFormatProvider, String, Object?[])"/></param>
        /// <param name="format"><inheritdoc cref="Write(IFormatProvider, String, Object?[])"/></param>
        /// <param name="args"><inheritdoc cref="Write(IFormatProvider, String, Object?[])"/></param>
        public void WriteIndented ( IFormatProvider formatProvider, String format, params Object?[] args )
        {
            this.WriteIndentation ( );
            this.Write ( formatProvider, format, args );
        }

        #endregion Write(Indented|Indentation)

        #region WriteLine(Indented)

        /// <summary>
        /// Writes a line separator to the output.
        /// </summary>
        public abstract void WriteLine ( );

        /// <summary>
        /// Writes a value to the output followed by a line separator.
        /// </summary>
        /// <param name="value">The value to be written to the output.</param>
        public abstract void WriteLine ( Object? value );

        /// <summary>
        /// Writes a string to the output followed by a line separator.
        /// </summary>
        /// <param name="value">The string to be written to the output.</param>
        public abstract void WriteLine ( String? value );

        /// <summary>
        /// Writes a formatted string to the output followed by a line separator.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The format arguments.</param>
        public abstract void WriteLine ( String format, params Object?[] args );

        /// <summary>
        /// Writes a formatted string to the output followed by a line separator.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The format arguments.</param>
        public abstract void WriteLine ( IFormatProvider formatProvider, String format, params Object?[] args );

        /// <summary>
        /// Writes a value to the output preceded by indentation and followed by a line separator.
        /// </summary>
        /// <param name="value"><inheritdoc cref="WriteLine(Object?)" /></param>
        public void WriteLineIndented ( Object? value )
        {
            this.WriteIndentation ( );
            this.WriteLine ( value );
        }

        /// <summary>
        /// Writes a string to the output preceded by indentation and followed by a line separator.
        /// </summary>
        /// <param name="value"><inheritdoc cref="WriteLine(String?)"/></param>
        public void WriteLineIndented ( String? value )
        {
            this.WriteIndentation ( );
            this.WriteLine ( value );
        }


        /// <summary>
        /// Writes a formatted string to the output preceded by indentation and followed by a line separator.
        /// </summary>
        /// <param name="format"><inheritdoc cref="WriteLine(String, Object?[])"/></param>
        /// <param name="args"><inheritdoc cref="WriteLine(String, Object?[])"/></param>
        [SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "Another overload does this already." )]
        public void WriteLineIndented ( String format, params Object?[] args )
        {
            this.WriteIndentation ( );
            this.WriteLine ( format, args );
        }


        /// <summary>
        /// Writes a formatted string to the output preceded by indentation and followed by a line separator.
        /// </summary>
        /// <param name="formatProvider"><inheritdoc cref="WriteLine(IFormatProvider, String, Object?[])"/></param>
        /// <param name="format"><inheritdoc cref="WriteLine(IFormatProvider, String, Object?[])"/></param>
        /// <param name="args"><inheritdoc cref="WriteLine(IFormatProvider, String, Object?[])"/></param>
        public void WriteLineIndented ( IFormatProvider formatProvider, String format, params Object?[] args )
        {
            this.WriteIndentation ( );
            this.WriteLine ( formatProvider, format, args );
        }

        #endregion WriteLine(Indented)

        /// <summary>
        /// Increases the indentation before the callback and decreases it after
        /// </summary>
        /// <param name="cb"></param>
        public void WithIndentation ( Action cb )
        {
            if ( cb == null )
                throw new ArgumentNullException ( nameof ( cb ) );

            this.Indent ( );
            cb ( );
            this.Outdent ( );
        }

        /// <summary>
        /// Increases the indentation and decreases it when the <see cref="IndentationDisposable" />
        /// is disposed.
        /// </summary>
        /// <returns></returns>
        public IndentationDisposable WithIndentation ( ) =>
            new IndentationDisposable ( this );
    }
}