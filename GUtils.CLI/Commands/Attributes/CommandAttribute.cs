/*
 * Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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
using System.Linq;

namespace GUtils.CLI.Commands
{
    /// <summary>
    /// Indicates a method is a command or adds a name to the command
    /// </summary>
    [AttributeUsage ( AttributeTargets.Method, AllowMultiple = true, Inherited = true )]
    public sealed class CommandAttribute : Attribute
    {
        /// <summary>
        /// The name of the command
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Whether this is intended to override any other commands with the same name
        /// </summary>
        public Boolean Overwrite { get; set; }

        /// <summary>
        /// Initializes this <see cref="CommandAttribute"/>
        /// </summary>
        /// <param name="Name"></param>
        public CommandAttribute ( String Name )
        {
            if ( String.IsNullOrWhiteSpace ( Name ) )
                throw new ArgumentException ( "Command name must not be null or composed of whitespaces.", nameof ( Name ) );
            if ( Name.Any ( Char.IsWhiteSpace ) )
                throw new ArgumentException ( "Command name cannot have whitespaces in it.", nameof ( Name ) );
            this.Name = Name;
        }
    }
}
