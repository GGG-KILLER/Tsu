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
        private readonly Boolean _keepOpen;
        private Boolean disposedValue;

        /// <summary>
        /// Initializes a new <see cref="TextWriterCodeWriter" />.
        /// </summary>
        /// <param name="indentationSequence"><inheritdoc cref="CodeWriter(String)" /></param>
        /// <param name="writer">The <see cref="TextWriter" /> to be used by this code writer.</param>
        public TextWriterCodeWriter ( String indentationSequence, TextWriter writer ) : this ( indentationSequence, writer, false )
        {
        }

        /// <summary>
        /// <inheritdoc cref="TextWriterCodeWriter(String, TextWriter)" />
        /// </summary>
        /// <param name="indentationSequence">
        /// <inheritdoc cref="TextWriterCodeWriter(String, TextWriter)" />
        /// </param>
        /// <param name="writer"><inheritdoc cref="TextWriterCodeWriter(String, TextWriter)" /></param>
        /// <param name="keepOpen">Whether to keep the <paramref name="writer" /> open.</param>
        public TextWriterCodeWriter ( String indentationSequence, TextWriter writer, Boolean keepOpen ) : base ( indentationSequence )
        {
            this._writer = writer;
            this._keepOpen = keepOpen;
        }

        #region Write(Line)

        /// <inheritdoc />
        public override void Write ( Object? value ) => this._writer.Write ( value );

        /// <inheritdoc />
        public override void Write ( String? value ) => this._writer.Write ( value );

        /// <inheritdoc />
        public override void Write ( String format, params Object?[] args ) => this._writer.Write ( format, args );

        /// <inheritdoc/>
        public override void Write ( IFormatProvider formatProvider, String format, params Object?[] args ) => this._writer.Write ( String.Format ( formatProvider, format, args ) );

        /// <inheritdoc />
        public override void WriteLine ( ) => this._writer.WriteLine ( );

        /// <inheritdoc />
        public override void WriteLine ( Object? value ) => this._writer.WriteLine ( value );

        /// <inheritdoc />
        public override void WriteLine ( String? value ) => this._writer.WriteLine ( value );

        /// <inheritdoc />
        public override void WriteLine ( String format, params Object?[] args ) => this._writer.WriteLine ( format, args );

        /// <inheritdoc/>
        public override void WriteLine ( IFormatProvider formatProvider, String format, params Object?[] args ) => this._writer.WriteLine ( String.Format ( formatProvider, format, args ) );

        #endregion Write(Line)

        #region IDisposable

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        /// <param name="disposing">
        /// If this was triggered by <see cref="Dispose()" /> or the class' destructor.
        /// </param>
        protected virtual void Dispose ( Boolean disposing )
        {
            if ( !this.disposedValue )
            {
                if ( disposing && !this._keepOpen )
                {
                    this._writer.Dispose ( );
                }

                this.disposedValue = true;
            }
        }

        /// <inheritdoc />
        public void Dispose ( )
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose ( disposing: true );
            GC.SuppressFinalize ( this );
        }

        #endregion IDisposable
    }
}