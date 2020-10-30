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
        public StringBuilderCodeWriter ( String indentationSequence ) : base ( indentationSequence )
        {
            this._builder = new StringBuilder ( );
        }

        #region Write

        /// <inheritdoc/>
        public override void Write ( Object? value ) => this._builder.Append ( value );

        /// <inheritdoc/>
        public override void Write ( String? value ) => this._builder.Append ( value );

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "Another overload does that already." )]
        public override void Write ( String format, params Object?[] args ) => this._builder.AppendFormat ( format, args );

        /// <inheritdoc/>
        public override void Write ( IFormatProvider formatProvider, String format, params Object?[] args ) => this._builder.AppendFormat ( formatProvider, format, args );

        #endregion Write

        #region WriteLine

        /// <inheritdoc/>
        public override void WriteLine ( ) => this._builder.AppendLine ( );

        /// <inheritdoc/>
        public override void WriteLine ( Object? value ) => this._builder.Append ( value ).AppendLine ( );

        /// <inheritdoc/>
        public override void WriteLine ( String? value ) => this._builder.AppendLine ( value );

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "Another overload does that already." )]
        public override void WriteLine ( String format, params Object?[] args ) => this._builder.AppendFormat ( format, args ).AppendLine ( );

        /// <inheritdoc/>
        public override void WriteLine ( IFormatProvider formatProvider, String format, params Object?[] args ) => this._builder.AppendFormat ( formatProvider, format, args ).AppendLine ( );

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