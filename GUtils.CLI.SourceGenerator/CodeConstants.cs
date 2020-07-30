using System;

namespace GUtils.CLI.SourceGenerator
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
            public const String FullyQualifiedName = "GUtils.CLI." + Name;

            /// <summary>
            /// The name of the file containing the attribute source code.
            /// </summary>
            public const String CodeFileName = "GUtils.CLI." + Name + ".cs";

            /// <summary>
            /// The source code for the attribute.
            /// </summary>
            public const String Code = @"
using System;

namespace GUtils.CLI
{
    [AttributeUsage ( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
    public class " + Name + @" : Attribute
    {
        public Type[] CommandClasses { get; }

        public " + Name + @" ( Type commandClass, params Type[] otherCommandClasses )
        {
            var classes = new Type[otherCommandClasses.Length + 1];
            classes[0] = commandClass;
            for ( var i = 0; i < otherCommandClasses.Length; i++ )
            {
                classes[i + 1] = otherCommandClasses[i];
            }
            this.CommandClasses = classes;
        }
    }
}";
        }
    }
}