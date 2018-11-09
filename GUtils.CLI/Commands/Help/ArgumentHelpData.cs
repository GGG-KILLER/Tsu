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

namespace GUtils.CLI.Commands.Help
{
    /// <summary>
    /// The possible modifiers an argument can have
    /// </summary>
    [Flags]
    public enum ArgumentModifiers
    {
        /// <summary>
        /// The required argument modifier
        /// </summary>
        Required = 0b000,

        /// <summary>
        /// The optional argument modifier
        /// </summary>
        Optional = 0b001,

        /// <summary>
        /// The argument requests the rest of the arguments to be joined in a string
        /// </summary>
        JoinRest = 0b010,

        /// <summary>
        /// The arguments has the params modifier
        /// </summary>
        Params   = 0b100
    }

    /// <summary>
    /// Represents the help information available for a given command argument
    /// </summary>
    public readonly struct ArgumentHelpData
    {
        /// <summary>
        /// The name of the argument
        /// </summary>
        public readonly String Name;

        /// <summary>
        /// The description of the argument
        /// </summary>
        public readonly String Description;

        /// <summary>
        /// The modifiers that the argument has
        /// </summary>
        public readonly ArgumentModifiers Modifiers;

        /// <summary>
        /// The type of the argument
        /// </summary>
        public readonly Type ParameterType;

        /// <summary>
        /// Initializes <see cref="ArgumentHelpData"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="modifiers"></param>
        /// <param name="parameterType"></param>
        public ArgumentHelpData ( String name, String description, ArgumentModifiers modifiers, Type parameterType )
        {
            this.Name          = name;
            this.Description   = description;
            this.Modifiers     = modifiers;
            this.ParameterType = parameterType;
        }
    }
}
