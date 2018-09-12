using System;

namespace GUtils.CLI.Commands.Help
{
    [Flags]
    public enum ArgumentModifiers
    {
        Required = 0b000,
        Optional = 0b001,
        JoinRest = 0b010,
        Params   = 0b100
    }

    public readonly struct ArgumentHelpData
    {
        public readonly String Name;
        public readonly String Description;
        public readonly ArgumentModifiers Modifiers;
        public readonly Type ParameterType;

        public ArgumentHelpData ( String name, String description, ArgumentModifiers modifiers, Type parameterType )
        {
            this.Name          = name;
            this.Description   = description;
            this.Modifiers     = modifiers;
            this.ParameterType = parameterType;
        }
    }
}
