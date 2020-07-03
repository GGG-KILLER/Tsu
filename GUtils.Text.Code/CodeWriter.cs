using System;
using System.Text;

namespace GUtils.Text.Code
{
    /// <summary>
    /// A generic code writer base class.
    /// </summary>
    public abstract class CodeWriter
    {
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
            protected set
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
        /// Writes the indentation prefix
        /// </summary>
        public void WriteIndentation ( ) =>
            this.Write ( this._cachedIndentation );

        /// <summary>
        /// Writes a value to the output preceded by indentation.
        /// </summary>
        /// <param name="value">The value to be written to the output.</param>
        public void WriteIndented ( Object? value )
        {
            this.WriteIndentation ( );
            this.Write ( value );
        }

        /// <summary>
        /// Writes a string to the output preceded by indentation.
        /// </summary>
        /// <param name="value">The string to be written to the output.</param>
        public void WriteIndented ( String? value )
        {
            this.WriteIndentation ( );
            this.Write ( value );
        }

        /// <summary>
        /// Writes a formatted string to the output preceded by indentation.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The format arguments.</param>
        public void WriteIndented ( String format, params Object?[] args )
        {
            this.WriteIndentation ( );
            this.Write ( format, args );
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
        /// <param name="value"></param>
        public void WriteLineIndented ( String? value )
        {
            this.WriteIndentation ( );
            this.WriteLine ( value );
        }

        /// <summary>
        /// Writes a formatted string to the output preceded by indentation and followed by a line separator.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void WriteLineIndented ( String format, params Object?[] args )
        {
            this.WriteIndentation ( );
            this.WriteLine ( format, args );
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
    }
}