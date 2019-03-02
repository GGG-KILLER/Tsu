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
using System.Text;

namespace GUtils.CLI.Commands.Errors
{
    /// <summary>
    /// Indicates an error happened while executing the command's method
    /// </summary>
    public class CommandInvocationException : Exception
    {
        /// <summary>
        /// The name of the command that failed to execute
        /// </summary>
        public readonly String Command;

        /// <summary>
        /// Initializes this <see cref="CommandInvocationException"/>
        /// </summary>
        public CommandInvocationException ( )
        {
        }

        /// <summary>
        /// Initializes this <see cref="CommandInvocationException"/>
        /// </summary>
        /// <param name="message"></param>
        public CommandInvocationException ( String message ) : base ( message )
        {
        }
        /// <summary>
        /// Initializes this <see cref="CommandInvocationException"/>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="message"></param>
        public CommandInvocationException ( String command, String message ) : base ( message )
        {
            this.Command = command;
        }

        /// <summary>
        /// Initializes this <see cref="CommandInvocationException"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CommandInvocationException ( String message, Exception innerException ) : base ( message, innerException )
        {
        }

        /// <summary>
        /// Initializes this <see cref="CommandInvocationException"/>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CommandInvocationException ( String command, String message, Exception innerException ) : base ( message, innerException )
        {
            this.Command = command;
        }
    }
}
