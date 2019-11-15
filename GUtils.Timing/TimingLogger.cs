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
using System.Diagnostics;

namespace GUtils.Timing
{
    /// <summary>
    /// The level of the logged information
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// The message does not have a log level
        /// </summary>
        None = -1,

        /// <summary>
        /// The message is a debug message
        /// </summary>
        Debug,

        /// <summary>
        /// The message is an information message
        /// </summary>
        Information,

        /// <summary>
        /// The message is a warning message
        /// </summary>
        Warning,

        /// <summary>
        /// The message is an error message
        /// </summary>
        Error,
    }

    /// <summary>
    /// A logger with embbeded timing information in every line.
    /// </summary>
    public abstract class TimingLogger
    {
        /// <summary>
        /// Represents a scope that was entered
        /// </summary>
        private class Scope : IDisposable
        {
            /// <summary>
            /// The <see cref="TimingLogger"/> that owns this scope
            /// </summary>
            private readonly TimingLogger _owner;

            /// <summary>
            /// The user provided name of the scope
            /// </summary>
            public String Name { get; }

            /// <summary>
            /// The <see cref="Elapsed"/> at which this scope was entered
            /// </summary>
            public TimeSpan StartedAt { get; }

            /// <summary>
            /// Whether the amount of time elapsed on this scope should be printed at the end.
            /// </summary>
            public Boolean ShouldPrintElapsedTime { get; }

            /// <summary>
            /// Initializes a scope
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="name"></param>
            /// <param name="startedAt"></param>
            /// <param name="shouldPrintElapsedTime"></param>
            /// <param name="parent"></param>
            public Scope ( TimingLogger owner, String name, TimeSpan startedAt, Boolean shouldPrintElapsedTime )
            {
                this._owner = owner;
                this.Name = name;
                this.StartedAt = startedAt;
                this.ShouldPrintElapsedTime = shouldPrintElapsedTime;
            }

            private Boolean disposed;

            void IDisposable.Dispose ( )
            {
                if ( !this.disposed )
                {
                    this._owner.EndScope ( this );
                    this.disposed = true;
                }
            }
        }

        private class Operation : IDisposable
        {
            /// <summary>
            /// The <see cref="TimingLogger"/> that owns this scope
            /// </summary>
            private readonly TimingLogger _owner;

            /// <summary>
            /// The user provided name of the scope
            /// </summary>
            public String Name { get; }

            /// <summary>
            /// The <see cref="Elapsed"/> at which this scope was entered
            /// </summary>
            public TimeSpan StartedAt { get; }

            /// <summary>
            /// Whether the amount of time elapsed on this scope should be printed at the end.
            /// </summary>
            public Boolean ShouldPrintElapsedTime { get; }

            /// <summary>
            /// Initializes a scope
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="name"></param>
            /// <param name="startedAt"></param>
            /// <param name="shouldPrintElapsedTime"></param>
            public Operation ( TimingLogger owner, String name, TimeSpan startedAt, Boolean shouldPrintElapsedTime )
            {
                this._owner = owner;
                this.Name = name;
                this.StartedAt = startedAt;
                this.ShouldPrintElapsedTime = shouldPrintElapsedTime;
            }

            private Boolean disposed;

            void IDisposable.Dispose ( )
            {
                if ( !this.disposed )
                {
                    this._owner.EndScope ( this );
                    this.disposed = true;
                }
            }
        }

        private readonly Stopwatch stopwatch;
        private readonly Stack<Scope> scopes;

        /// <summary>
        /// Whether the current line has already been prefixed
        /// </summary>
        protected Boolean hasLineBeenPrefixed;

        #region Logging Level Colors

        /// <summary>
        /// Color used for the prefix of <see cref="LogLevel.Debug"/> messages
        /// </summary>
        protected ConsoleColor DebugColor { get; set; } = ConsoleColor.DarkGray;

        /// <summary>
        /// Color used for the prefix of <see cref="LogLevel.Information"/> messages
        /// </summary>
        protected ConsoleColor InformationColor { get; set; } = ConsoleColor.DarkBlue;

        /// <summary>
        /// Color used for the prefix of <see cref="LogLevel.Warning"/> messages
        /// </summary>
        protected ConsoleColor WarningColor { get; set; } = ConsoleColor.Yellow;

        /// <summary>
        /// Color used for the prefix of <see cref="LogLevel.Error"/> messages
        /// </summary>
        protected ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;

        #endregion Logging Level Colors

        /// <summary>
        /// The minimum log level of the messages that should be printed
        /// </summary>
        public LogLevel MinimumLogLevel { get; set; }

        /// <summary>
        /// Whether to pring
        /// <code>
        ///[INFO]
        /// </code>
        /// ,
        /// <code>
        ///[DBUG]
        /// </code>
        /// ,
        /// <code>
        ///[WARN]
        /// </code>
        /// and
        /// <code>
        ///[FAIL]
        /// </code>
        /// prefixes according to the message's <see cref="LogLevel"/>
        /// </summary>
        public Boolean PrintLevelPrefixes { get; set; }

        /// <summary>
        /// The amount of time elapsed since this logger started
        /// </summary>
        public TimeSpan Elapsed => this.stopwatch.Elapsed;

        /// <summary>
        /// Initializes a TimingLogger
        /// </summary>
        protected TimingLogger ( )
        {
            this.stopwatch = Stopwatch.StartNew ( );
            this.scopes = new Stack<Scope> ( );
            this.MinimumLogLevel = LogLevel.Debug;
            this.PrintLevelPrefixes = true;
        }

        #region I/O

        /// <summary>
        /// Writes a value to the output
        /// </summary>
        /// <param name="str"></param>
        protected abstract void WriteInternal ( String str );

        /// <summary>
        /// Writes a value to the output with a given color
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        protected abstract void WriteInternal ( String str, ConsoleColor color );

        /// <summary>
        /// Writes a value to the output followed by a line break
        /// </summary>
        /// <param name="line"></param>
        protected abstract void WriteLineInternal ( String line );

        /// <summary>
        /// Writes a value to the output followed by a line break with a given color
        /// </summary>
        /// <param name="line"></param>
        /// <param name="color"></param>
        protected abstract void WriteLineInternal ( String line, ConsoleColor color );

        #endregion I/O

        #region Input message processing

        /// <summary>
        /// Writes the elapsed time and the <see cref="LogLevel"/> prefix, if enabled through <see cref="PrintLevelPrefixes"/>
        /// </summary>
        /// <param name="level"></param>
        protected virtual void WriteLinePrefix ( LogLevel level )
        {
            this.WriteInternal ( $"[{this.Elapsed:hh\\:mm\\:ss\\.ffffff}]{new String ( ' ', this.scopes.Count * 4 )}" );
            if ( !this.PrintLevelPrefixes || level < LogLevel.Debug || level > LogLevel.Error )
                return;

            this.WriteInternal ( "[" );
            switch ( level )
            {
                case LogLevel.Debug:
                    this.WriteInternal ( "DBUG", this.DebugColor );
                    break;

                case LogLevel.Information:
                    this.WriteInternal ( "INFO", this.InformationColor );
                    break;

                case LogLevel.Warning:
                    this.WriteInternal ( "WARN", this.WarningColor );
                    break;

                case LogLevel.Error:
                    this.WriteInternal ( "FAIL", this.ErrorColor );
                    break;

                default:
                    break;
            }
            this.WriteInternal ( "]" );
        }

        /// <summary>
        /// Processes a call to write a value to the output
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        private void ProcessWrite ( LogLevel level, String message )
        {
            if ( !this.hasLineBeenPrefixed )
            {
                this.WriteLinePrefix ( level );
                this.hasLineBeenPrefixed = true;
            }

            if ( message.Contains ( "\n" ) )
            {
                var lines = message.Split ( new[] { "\r\n", "\n" }, StringSplitOptions.None );
                this.WriteLineInternal ( lines[0] );
                for ( var i = 1; i < lines.Length - 1; i++ )
                {
                    this.WriteLinePrefix ( level );
                    this.WriteLineInternal ( lines[i] );
                }

                if ( lines.Length > 1 && !String.IsNullOrEmpty ( lines[lines.Length - 1] ) )
                {
                    var lastLine = lines[lines.Length - 1];
                    this.WriteLinePrefix ( level );
                    this.WriteInternal ( lastLine );
                }
            }
            else
            {
                this.WriteInternal ( message );
            }
        }

        /// <summary>
        /// Processes a WriteLine request with a specified log level and message
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        protected virtual void ProcessWriteLine ( LogLevel level, String message )
        {
            this.ProcessWrite ( level, message );
            this.WriteLineInternal ( "" );
            this.hasLineBeenPrefixed = false;
        }

        #endregion Input message processing

        #region Public Simple I/O

        /// <summary>
        /// Writes a value to the output
        /// </summary>
        /// <param name="value"></param>
        public void Write ( Object value ) =>
            this.ProcessWrite ( LogLevel.None, value?.ToString ( ) );

        /// <summary>
        /// Writes a value to the output
        /// </summary>
        /// <param name="value"></param>
        public void Write ( String value ) =>
            this.ProcessWrite ( LogLevel.None, value );

        /// <summary>
        /// Writes a value to the output
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "As designed." )]
        public void Write ( String format, params Object[] args ) =>
            this.ProcessWrite ( LogLevel.None, String.Format ( format, args ) );

        /// <summary>
        /// Writes a value to the output followed by a new line
        /// </summary>
        /// <param name="value"></param>
        public void WriteLine ( Object value ) =>
            this.ProcessWriteLine ( LogLevel.None, value?.ToString ( ) );

        /// <summary>
        /// Writes a value to the output followed by a new line
        /// </summary>
        /// <param name="value"></param>
        public void WriteLine ( String value ) =>
            this.ProcessWriteLine ( LogLevel.None, value );

        /// <summary>
        /// Writes a value to the output followed by a new line
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "As designed." )]
        public void WriteLine ( String format, params Object[] args ) =>
            this.ProcessWriteLine ( LogLevel.None, String.Format ( format, args ) );

        #endregion Public Simple I/O

        #region Message Logging

        /// <summary>
        /// Logs a message to the output
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public virtual void LogMessage ( LogLevel level, String message )
        {
            if ( level < this.MinimumLogLevel )
                return;

            this.ProcessWriteLine ( level, message );
        }

        /// <summary>
        /// Logs a message to the output
        /// </summary>
        /// <param name="level"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "As designed." )]
        public virtual void LogMessage ( LogLevel level, String format, params Object[] args )
        {
            if ( level < this.MinimumLogLevel )
                return;

            this.ProcessWriteLine ( level, String.Format ( format, args ) );
        }

        /// <summary>
        /// Logs a debug message to the output
        /// </summary>
        /// <param name="message"></param>
        public void LogDebug ( String message ) =>
            this.LogMessage ( LogLevel.Debug, message );

        /// <summary>
        /// Logs a debug message to the output
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogDebug ( String format, params Object[] args ) =>
            this.LogMessage ( LogLevel.Debug, format, args );

        /// <summary>
        /// Logs an information message to the output
        /// </summary>
        /// <param name="message"></param>
        public void LogInformation ( String message ) =>
            this.LogMessage ( LogLevel.Information, message );

        /// <summary>
        /// Logs an information message to the output
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogInformation ( String format, params Object[] args ) =>
            this.LogMessage ( LogLevel.Information, format, args );

        /// <summary>
        /// Logs a warning message to the outout
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning ( String message ) =>
            this.LogMessage ( LogLevel.Warning, message );

        /// <summary>
        /// Logs a warning message to the outout
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogWarning ( String format, params Object[] args ) =>
            this.LogMessage ( LogLevel.Warning, format, args );

        /// <summary>
        /// Logs an error message to the output
        /// </summary>
        /// <param name="message"></param>
        public void LogError ( String message ) =>
            this.LogMessage ( LogLevel.Error, message );

        /// <summary>
        /// Logs an error message to the output
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogError ( String format, params Object[] args ) =>
            this.LogMessage ( LogLevel.Error, format, args );

        #endregion Message Logging

        #region Scope Management

        /// <summary>
        /// Begins a logging scope
        /// </summary>
        /// <param name="name"></param>
        /// <param name="shouldPrintElapsedTime">
        /// Whether the elapsed time on this scope should be printed at it's disposal
        /// </param>
        /// <returns></returns>
        public IDisposable BeginScope ( String name, Boolean shouldPrintElapsedTime = true )
        {
            this.WriteLine ( name );
            this.WriteLine ( "{" );
            var scope = new Scope ( this, name, this.Elapsed, shouldPrintElapsedTime );
            this.scopes.Push ( scope );
            return scope;
        }

        private void EndScope ( Scope scope )
        {
            if ( this.scopes.Count < 1 )
                throw new InvalidOperationException ( "No scopes to end." );

            if ( !this.scopes.Peek ( ).Equals ( scope ) )
                throw new InvalidOperationException ( "Attempt to end outer scope without ending inner scope(s)." );

            scope = this.scopes.Pop ( );
            if ( scope.ShouldPrintElapsedTime )
                this.WriteLine ( $"Elapsed {Duration.Format ( ( this.Elapsed - scope.StartedAt ).Ticks )} in {scope.Name}." );
            this.WriteLine ( "}" );
        }

        #endregion Scope Management

        #region Operation Management

        /// <summary>
        /// Begins an operation that won't have any logging inside it. For operations that will
        /// oputput logs, use <see cref="BeginScope(String, Boolean)"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="shouldPrintElapsedTime"></param>
        /// <returns></returns>
        public IDisposable BeginOperation ( String name, Boolean shouldPrintElapsedTime = true ) =>
            new Operation ( this, name, this.Elapsed, shouldPrintElapsedTime );

        private void EndOperation ( Operation operation )
        {
            if ( operation.ShouldPrintElapsedTime )
                this.WriteLine ( $"Finished {operation.Name} in {Duration.Format ( ( this.Elapsed - operation.StartedAt ).Ticks )}." );
            else
                this.WriteLine ( $"Finished {operation.Name}." );
        }

        #endregion Operation Management
    }
}