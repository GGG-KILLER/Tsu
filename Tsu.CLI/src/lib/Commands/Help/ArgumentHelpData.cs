// Copyright � 2021 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the �Software�), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;

namespace Tsu.CLI.Commands.Help
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
        Params = 0b100
    }

    /// <summary>
    /// Represents the help information available for a given command argument
    /// </summary>
    public readonly struct ArgumentHelpData : IEquatable<ArgumentHelpData>
    {
        /// <summary>
        /// The name of the argument
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The description of the argument
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The modifiers that the argument has
        /// </summary>
        public ArgumentModifiers Modifiers { get; }

        /// <summary>
        /// The type of the argument
        /// </summary>
        public Type ParameterType { get; }

        /// <summary>
        /// Initializes <see cref="ArgumentHelpData" />
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="modifiers"></param>
        /// <param name="parameterType"></param>
        public ArgumentHelpData(string name, string description, ArgumentModifiers modifiers, Type parameterType)
        {
            Name = name;
            Description = description;
            Modifiers = modifiers;
            ParameterType = parameterType;
        }

        #region Generated code

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) =>
            obj is ArgumentHelpData data && Equals(data);

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ArgumentHelpData other) =>
            Name == other.Name
            && Description == other.Description
            && Modifiers == other.Modifiers
            && EqualityComparer<Type>.Default.Equals(ParameterType, other.ParameterType);

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = -1498514776;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + Modifiers.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(ParameterType);
            return hashCode;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        public static bool operator ==(ArgumentHelpData data1, ArgumentHelpData data2) => data1.Equals(data2);

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        public static bool operator !=(ArgumentHelpData data1, ArgumentHelpData data2) => !(data1 == data2);

        #endregion Generated code
    }
}
