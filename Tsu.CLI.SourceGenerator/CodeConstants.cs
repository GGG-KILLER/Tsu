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

namespace Tsu.CLI.SourceGenerator
{
    /// <summary>
    /// Constants used for code throughout the program.
    /// </summary>
    internal static class CodeConstants
    {
        /// <summary>
        /// The constants for the command manager attribute.
        /// </summary>
        public static class CommandManagerAttribute
        {
            /// <summary>
            /// The simplified attribute name for command managers.
            /// </summary>
            public const string SimplifiedName = "GeneratedCommandManager";

            /// <summary>
            /// The complete attribute name for command managers.
            /// </summary>
            public const string Name = SimplifiedName + "Attribute";

            /// <summary>
            /// The fully qualified name of the attribute for command managers.
            /// </summary>
            public const string FullyQualifiedName = "Tsu.CLI." + Name;

            /// <summary>
            /// The name of the file containing the attribute source code.
            /// </summary>
            public const string CodeFileName = "Tsu.CLI." + Name + ".cs";

            /// <summary>
            /// The source code for the attribute.
            /// </summary>
            public const string Code = @"
using System;

namespace Tsu.CLI
{
    [AttributeUsage ( AttributeTargets.Class, AllowMultiple = true, Inherited = false )]
    public class " + Name + @" : Attribute
    {
        public Type CommandClass { get; }
        public String Verb { get; set; }

        public " + Name + @" ( Type commandClass )
        {
            this.CommandClass = commandClass;
        }
    }
}";

            /// <summary>
            /// The source code for the attribute when nullable reference types are enabled.
            /// </summary>
            public const string NullableCode = @"
using System;

namespace Tsu.CLI
{
    [AttributeUsage ( AttributeTargets.Class, AllowMultiple = true, Inherited = false )]
    public class " + Name + @" : Attribute
    {
        public Type CommandClass { get; }
        public String? Verb { get; set; }

        public " + Name + @" ( Type commandClass )
        {
            this.CommandClass = commandClass;
        }
    }
}";
        }
    }
}