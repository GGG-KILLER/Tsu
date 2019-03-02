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
using System.Reflection;
using System.Text;

namespace GUtils.CLI.Commands.Errors
{
    /// <summary>
    /// Thrown when a command's definition is invalid
    /// </summary>
    public class CommandDefinitionException : Exception
    {
        /// <summary>
        /// The method that is the body of the command
        /// </summary>
        public readonly MethodInfo Method;

        /// <summary>
        /// Initializes this <see cref="CommandDefinitionException"/>
        /// </summary>
        /// <param name="method"></param>
        /// <param name="message"></param>
        public CommandDefinitionException ( MethodInfo method, String message ) : base ( message )
        {
            this.Method = method;
        }

        /// <summary>
        /// Initializes this <see cref="CommandDefinitionException"/>
        /// </summary>
        /// <param name="method"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CommandDefinitionException ( MethodInfo method, String message, Exception innerException ) : base ( message, innerException )
        {
            this.Method = method;
        }
    }
}
