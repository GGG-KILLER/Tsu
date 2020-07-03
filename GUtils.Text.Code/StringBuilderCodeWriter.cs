using System;
using System.Text;

namespace GUtils.Text.Code
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
        public StringBuilderCodeWriter ( String indentationSequence ) : base ( indentationSequence )
        {
            this._builder = new StringBuilder ( );
        }

        #region Write

        /// <summary>
        /// Writes a value
        /// </summary>
        /// <param name="value"></param>
        public override void Write ( Object? value ) => this._builder.Append ( value );

        /// <summary>
        /// Writes a value
        /// </summary>
        /// <param name="value"></param>
        public override void Write ( String? value ) => this._builder.Append ( value );

        /// <summary>
        /// Writes a formatted value
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public override void Write ( String format, params Object?[] args ) => this._builder.AppendFormat ( format, args );

        #endregion Write

        #region WriteLine

        /// <summary>
        /// Writes an empty line
        /// </summary>
        public override void WriteLine ( ) => this._builder.AppendLine ( );

        /// <summary>
        /// Writes a value followed by the line terminator
        /// </summary>
        /// <param name="value"></param>
        public override void WriteLine ( Object? value ) => this._builder.Append ( value ).AppendLine ( );

        /// <summary>
        /// Writes a value followed by the line terminator
        /// </summary>
        /// <param name="value"></param>
        public override void WriteLine ( String? value ) => this._builder.AppendLine ( value );

        /// <summary>
        /// Writes a formatted value followed by the line terminator
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public override void WriteLine ( String format, params Object?[] args ) => this._builder.AppendFormat ( format, args ).AppendLine ( );

        #endregion WriteLine

        /// <summary>
        /// Resets the writer
        /// </summary>
        public void Reset ( )
        {
            this._builder.Clear ( );
            this.Indentation = 0;
        }

        /// <inheritdoc />
        public override String ToString ( ) => this._builder.ToString ( );
    }
}