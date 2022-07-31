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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Tsu.Numerics;

namespace Tsu.Timing
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
            public string Name { get; }

            /// <summary>
            /// The <see cref="Elapsed"/> at which this scope was entered
            /// </summary>
            public TimeSpan StartedAt { get; }

            /// <summary>
            /// Whether the amount of time elapsed on this scope should be printed at the end.
            /// </summary>
            public bool ShouldPrintElapsedTime { get; }

            /// <summary>
            /// Initializes a scope
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="name"></param>
            /// <param name="startedAt"></param>
            /// <param name="shouldPrintElapsedTime"></param>
            public Scope(TimingLogger owner, string name, TimeSpan startedAt, bool shouldPrintElapsedTime)
            {
                _owner = owner;
                Name = name;
                StartedAt = startedAt;
                ShouldPrintElapsedTime = shouldPrintElapsedTime;
            }

            private bool _disposed;

            void IDisposable.Dispose()
            {
                if (!_disposed)
                {
                    _owner.EndScope(this);
                    _disposed = true;
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
            public string Name { get; }

            /// <summary>
            /// The <see cref="Elapsed"/> at which this scope was entered
            /// </summary>
            public TimeSpan StartedAt { get; }

            /// <summary>
            /// Whether the amount of time elapsed on this scope should be printed at the end.
            /// </summary>
            public bool ShouldPrintElapsedTime { get; }

            /// <summary>
            /// Initializes a scope
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="name"></param>
            /// <param name="startedAt"></param>
            /// <param name="shouldPrintElapsedTime"></param>
            public Operation(TimingLogger owner, string name, TimeSpan startedAt, bool shouldPrintElapsedTime)
            {
                _owner = owner;
                Name = name;
                StartedAt = startedAt;
                ShouldPrintElapsedTime = shouldPrintElapsedTime;
            }

            private bool _disposed;

            void IDisposable.Dispose()
            {
                if (!_disposed)
                {
                    _owner.EndOperation(this);
                    _disposed = true;
                }
            }
        }

        private readonly Stopwatch _stopwatch;
        private readonly Stack<Scope> _scopes;

        /// <summary>
        /// Whether the current line has already been prefixed
        /// </summary>
        protected bool HasLineBeenPrefixed { get; set; }

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
        public bool PrintLevelPrefixes { get; set; }

        /// <summary>
        /// The amount of time elapsed since this logger started
        /// </summary>
        public TimeSpan Elapsed => _stopwatch.Elapsed;

        /// <summary>
        /// Initializes a TimingLogger
        /// </summary>
        protected TimingLogger()
        {
            _stopwatch = Stopwatch.StartNew();
            _scopes = new Stack<Scope>();
            MinimumLogLevel = LogLevel.Debug;
            PrintLevelPrefixes = true;
        }

        #region I/O

        /// <summary>
        /// Writes a value to the output
        /// </summary>
        /// <param name="str"></param>
        protected abstract void WriteInternal(string str);

        /// <summary>
        /// Writes a value to the output with a given color
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        protected abstract void WriteInternal(string str, ConsoleColor color);

        /// <summary>
        /// Writes a value to the output followed by a line break
        /// </summary>
        /// <param name="line"></param>
        protected abstract void WriteLineInternal(string line);

        /// <summary>
        /// Writes a value to the output followed by a line break with a given color
        /// </summary>
        /// <param name="line"></param>
        /// <param name="color"></param>
        protected abstract void WriteLineInternal(string line, ConsoleColor color);

        #endregion I/O

        #region Input message processing

        /// <summary>
        /// Writes the elapsed time and the <see cref="LogLevel"/> prefix, if enabled through <see cref="PrintLevelPrefixes"/>
        /// </summary>
        /// <param name="level"></param>
        protected virtual void WriteLinePrefix(LogLevel level)
        {
            if (HasLineBeenPrefixed)
                return;
            HasLineBeenPrefixed = true;

            WriteInternal($"[{Elapsed:hh\\:mm\\:ss\\.ffffff}]{new string(' ', _scopes.Count * 4)}");
            if (!PrintLevelPrefixes || level < LogLevel.Debug || level > LogLevel.Error)
                return;

            WriteInternal("[");
            switch (level)
            {
                case LogLevel.Debug:
                    WriteInternal("DBUG", DebugColor);
                    break;

                case LogLevel.Information:
                    WriteInternal("INFO", InformationColor);
                    break;

                case LogLevel.Warning:
                    WriteInternal("WARN", WarningColor);
                    break;

                case LogLevel.Error:
                    WriteInternal("FAIL", ErrorColor);
                    break;

                default:
                    break;
            }
            WriteInternal("]");
        }

        /// <summary>
        /// Processes a call to write a value to the output
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        [SuppressMessage("Style", "IDE0056:Use index operator", Justification = "Not available in all target frameworks.")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Applicable to some target frameworks.")]
        private void ProcessWrite(LogLevel level, string message)
        {
            WriteLinePrefix(level);

            if (message is "\r\n" or "\n")
            {
                WriteLineInternal("");
                HasLineBeenPrefixed = false;
            }
            else if (
#if HAS_STRING__CONTAINS_CHAR
                message.Contains('\n', StringComparison.Ordinal)
#else
                CultureInfo.InvariantCulture.CompareInfo.IndexOf(message, '\n', CompareOptions.Ordinal) >= 0
#endif
            )
            {
                var lines = message.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                WriteLineInternal(lines[0]);
                HasLineBeenPrefixed = false;

                for (var i = 1; i < lines.Length - 1; i++)
                {
                    WriteLinePrefix(level);
                    WriteLineInternal(lines[i]);
                    HasLineBeenPrefixed = false;
                }

                if (lines.Length > 1)
                {
                    var lastLine = lines[lines.Length - 1];
                    WriteLinePrefix(level);
                    WriteInternal(lastLine);
                }
            }
            else
            {
                WriteInternal(message);
            }
        }

        /// <summary>
        /// Processes a WriteLine request with a specified log level and message
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        protected virtual void ProcessWriteLine(LogLevel level, string message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            ProcessWrite(level, message);
            WriteLineInternal("");
            HasLineBeenPrefixed = false;
        }

        #endregion Input message processing

        #region Public Simple I/O

        /// <summary>
        /// Writes a value to the output
        /// </summary>
        /// <param name="value"></param>
        public void Write(object value) =>
            ProcessWrite(LogLevel.None, value?.ToString() ?? "");

        /// <summary>
        /// Writes a value to the output
        /// </summary>
        /// <param name="value"></param>
        public void Write(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            ProcessWrite(LogLevel.None, value);
        }

        /// <summary>
        /// Writes a value to the output
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        [SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "As designed.")]
        public void Write(string format, params object[] args) =>
            ProcessWrite(LogLevel.None, string.Format(format, args));

        /// <summary>
        /// Writes a value to the output followed by a new line
        /// </summary>
        /// <param name="value"></param>
        public void WriteLine(object value) =>
            ProcessWriteLine(LogLevel.None, value?.ToString() ?? "");

        /// <summary>
        /// Writes a value to the output followed by a new line
        /// </summary>
        /// <param name="value"></param>
        public void WriteLine(string value) =>
            ProcessWriteLine(LogLevel.None, value);

        /// <summary>
        /// Writes a value to the output followed by a new line
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        [SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "As designed.")]
        public void WriteLine(string format, params object[] args) =>
            ProcessWriteLine(LogLevel.None, string.Format(format, args));

        #endregion Public Simple I/O

        #region Message Logging

        /// <summary>
        /// Logs a message to the output
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public virtual void LogMessage(LogLevel level, string message)
        {
            if (level < MinimumLogLevel)
                return;

            ProcessWriteLine(level, message);
        }

        /// <summary>
        /// Logs a message to the output
        /// </summary>
        /// <param name="level"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        [SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "As designed.")]
        public virtual void LogMessage(LogLevel level, string format, params object[] args)
        {
            if (level < MinimumLogLevel)
                return;

            ProcessWriteLine(level, string.Format(format, args));
        }

        /// <summary>
        /// Logs a debug message to the output
        /// </summary>
        /// <param name="message"></param>
        public void LogDebug(string message) =>
            LogMessage(LogLevel.Debug, message);

        /// <summary>
        /// Logs a debug message to the output
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogDebug(string format, params object[] args) =>
            LogMessage(LogLevel.Debug, format, args);

        /// <summary>
        /// Logs an information message to the output
        /// </summary>
        /// <param name="message"></param>
        public void LogInformation(string message) =>
            LogMessage(LogLevel.Information, message);

        /// <summary>
        /// Logs an information message to the output
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogInformation(string format, params object[] args) =>
            LogMessage(LogLevel.Information, format, args);

        /// <summary>
        /// Logs a warning message to the outout
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(string message) =>
            LogMessage(LogLevel.Warning, message);

        /// <summary>
        /// Logs a warning message to the outout
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogWarning(string format, params object[] args) =>
            LogMessage(LogLevel.Warning, format, args);

        /// <summary>
        /// Logs an error message to the output
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message) =>
            LogMessage(LogLevel.Error, message);

        /// <summary>
        /// Logs an error message to the output
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogError(string format, params object[] args) =>
            LogMessage(LogLevel.Error, format, args);

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
        public IDisposable BeginScope(string name, bool shouldPrintElapsedTime = true)
        {
            WriteLine($"{name}...");
            WriteLine("{");
            var scope = new Scope(this, name, Elapsed, shouldPrintElapsedTime);
            _scopes.Push(scope);
            return scope;
        }

        [SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "By design.")]
        [SuppressMessage("Style", "IDE0057:Substring can be simplified", Justification = "Not available in all target frameworks.")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Applicable to some target frameworks.")]
        private static string UnCapitalize(string text) =>
            text.Length > 1 ? char.ToLower(text[0]) + text.Substring(1) : text.ToLower();

        private void EndScope(Scope scope)
        {
            if (_scopes.Count < 1)
                throw new InvalidOperationException("No scopes to end.");

            if (!_scopes.Peek().Equals(scope))
                throw new InvalidOperationException("Attempt to end outer scope without ending inner scope(s).");

            if (scope.ShouldPrintElapsedTime)
                WriteLine($"Elapsed {Duration.Format((Elapsed - scope.StartedAt).Ticks)} in {UnCapitalize(scope.Name)}.");

            _scopes.Pop();
            WriteLine("}");
        }

        #endregion Scope Management

        #region Operation Management

        /// <summary>
        /// Begins an operation that won't have any logging inside it. For operations that will
        /// oputput logs, use <see cref="BeginScope(string, bool)"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="shouldPrintElapsedTime"></param>
        /// <returns></returns>
        public IDisposable BeginOperation(string name, bool shouldPrintElapsedTime = true)
        {
            Write($"{name}...");
            return new Operation(this, name, Elapsed, shouldPrintElapsedTime);
        }

        private void EndOperation(Operation operation)
        {
            if (operation.ShouldPrintElapsedTime)
                WriteLine($" done. ({Duration.Format((Elapsed - operation.StartedAt).Ticks)})");
            else
                WriteLine($" done.");
        }

        #endregion Operation Management
    }
}