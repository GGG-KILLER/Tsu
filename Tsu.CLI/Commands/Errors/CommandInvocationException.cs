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
using System.Runtime.Serialization;

namespace Tsu.CLI.Commands.Errors
{
    /// <summary>
    /// Indicates an error happened while executing the command's method
    /// </summary>
    [Serializable]
    public class CommandInvocationException : Exception
    {
        /// <summary>
        /// The name of the command that failed to execute
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Initializes this <see cref="CommandInvocationException"/>
        /// </summary>
        public CommandInvocationException()
        {
        }

        /// <summary>
        /// Initializes this <see cref="CommandInvocationException"/>
        /// </summary>
        /// <param name="message"></param>
        public CommandInvocationException(string message) : base(message)
        {
        }
        /// <summary>
        /// Initializes this <see cref="CommandInvocationException"/>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="message"></param>
        public CommandInvocationException(string command, string message) : base(message)
        {
            Command = command;
        }

        /// <summary>
        /// Initializes this <see cref="CommandInvocationException"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CommandInvocationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes this <see cref="CommandInvocationException"/>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CommandInvocationException(string command, string message, Exception innerException) : base(message, innerException)
        {
            Command = command;
        }

        /// <summary>
        /// Initializes this <see cref="CommandInvocationException"/>
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="streamingContext"></param>
        protected CommandInvocationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            if (serializationInfo is null)
                throw new ArgumentNullException(nameof(serializationInfo));

            Command = serializationInfo.GetString("CommandName");
        }

        /// <inheritdoc/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("CommandName", Command);
        }
    }
}
