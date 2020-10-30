using System;

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
            public const String SimplifiedName = "GeneratedCommandManager";

            /// <summary>
            /// The complete attribute name for command managers.
            /// </summary>
            public const String Name = SimplifiedName + "Attribute";

            /// <summary>
            /// The fully qualified name of the attribute for command managers.
            /// </summary>
            public const String FullyQualifiedName = "Tsu.CLI." + Name;

            /// <summary>
            /// The name of the file containing the attribute source code.
            /// </summary>
            public const String CodeFileName = "Tsu.CLI." + Name + ".cs";

            /// <summary>
            /// The source code for the attribute.
            /// </summary>
            public const String Code = @"
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
            public const String NullableCode = @"
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