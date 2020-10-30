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
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;

namespace Tsu.CLI.Commands.Errors
{
    /// <summary>
    /// Thrown when a command's definition is invalid
    /// </summary>
    [Serializable]
    [SuppressMessage ( "Design", "CA1032:Implement standard exception constructors", Justification = "This exception shouldn't be constructed without a command method." )]
    public class CommandDefinitionException : Exception
    {
        /// <summary>
        /// The method that is the body of the command
        /// </summary>
        public MethodInfo Method { get; }

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

        /// <summary>
        /// Initializes this <see cref="CommandDefinitionException"/>
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="streamingContext"></param>
        protected CommandDefinitionException ( SerializationInfo serializationInfo, StreamingContext streamingContext )
            : base ( serializationInfo, streamingContext )
        {
            if ( serializationInfo is null )
                throw new ArgumentNullException ( nameof ( serializationInfo ) );

            this.Method = ( MethodInfo ) serializationInfo.GetValue ( "CommandMethod", typeof ( MethodInfo ) );
        }

        /// <inheritdoc/>
        public override void GetObjectData ( SerializationInfo info, StreamingContext context )
        {
            base.GetObjectData ( info, context );
            info.AddValue ( "CommandMethod", this.Method );
        }
    }
}
