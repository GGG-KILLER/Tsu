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
using System.Runtime.Serialization;

namespace Tsu.CLI.Commands.Errors
{
    /// <summary>
    /// Thrown when a command is not found by the command manager
    /// </summary>
    [Serializable]
    public class NonExistentCommandException : CommandInvocationException
    {
        /// <summary>
        /// Initializes this <see cref="NonExistentCommandException" />
        /// </summary>
        public NonExistentCommandException()
        {
        }

        /// <summary>
        /// Initializes this <see cref="NonExistentCommandException" />
        /// </summary>
        /// <param name="command"></param>
        public NonExistentCommandException(string command) : base(command, "Command does not exist.")
        {
        }

        /// <summary>
        /// Initializes this <see cref="NonExistentCommandException" />
        /// </summary>
        /// <param name="command"></param>
        /// <param name="innerException"></param>
        public NonExistentCommandException(string command, Exception innerException) : base(command, "Command does not exist.", innerException)
        {
        }

        /// <summary>
        /// Initializes this <see cref="NonExistentCommandException"/>
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="streamingContext"></param>
        protected NonExistentCommandException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
