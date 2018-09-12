using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using GUtils.CLI.Commands.Errors;
using GUtils.CLI.Commands.Help;

namespace GUtils.CLI.Commands
{
    public class Command
    {
        public readonly ImmutableArray<String> Names;
        public readonly String Description;
        public readonly Boolean IsRaw;
        public readonly ImmutableArray<ArgumentHelpData> Arguments;
        public readonly ImmutableArray<String> Examples;

        internal readonly MethodInfo Method;
        internal readonly Object Instance;
        internal readonly Action<String, String[]> CompiledCommand;

        private static ArgumentModifiers GetArgumentModifiers ( in ParameterInfo info )
        {
            ArgumentModifiers mods = ArgumentModifiers.Required;

            if ( info.IsDefined ( typeof ( JoinRestOfArgumentsAttribute ) ) )
                mods |= ArgumentModifiers.JoinRest;
            if ( info.IsDefined ( typeof ( ParamArrayAttribute ) ) )
                mods |= ArgumentModifiers.Params;
            if ( info.HasDefaultValue )
                mods |= ArgumentModifiers.Optional;

            return mods;
        }

        public Command ( in MethodInfo method, in Object instance )
        {
            ValidateMethod ( method, instance );

            this.IsRaw = method.IsDefined ( typeof ( RawInputAttribute ) );
            this.Method = method;
            this.Instance = instance;

            this.Names = method.GetCustomAttributes<CommandAttribute> ( )
                .Select ( cmd => cmd.Name )
                .ToImmutableArray ( );

            this.Description = method.GetCustomAttribute<HelpDescriptionAttribute> ( )?.Description ?? "No description was provided for this command.";

            this.Arguments = method.GetParameters ( )
                .Select ( arg => new ArgumentHelpData (
                        arg.Name,
                        arg.GetCustomAttribute<HelpDescriptionAttribute> ( )?.Description ?? "No description was provided for this argument.",
                        GetArgumentModifiers ( arg ),
                        arg.ParameterType
                ) )
                .ToImmutableArray ( );

            this.Examples = method.GetCustomAttributes<HelpExampleAttribute> ( )
                .Select ( ex => ex.Example )
                .ToImmutableArray ( );

            this.CompiledCommand = CommandCompiler.Compile ( method, instance );
        }

        #region Validation

        private static void ValidateParameters ( in MethodInfo method, in Object instance, in ParameterInfo[] @params )
        {
            // Validate that the method does not contain any out
            // parameters and that the rest attribute is not
            // defined for an argument that isn't the last
            for ( var i = 0; i < @params.Length; i++ )
            {
                if ( @params[i].IsDefined ( typeof ( JoinRestOfArgumentsAttribute ) ) && i != @params.Length - 1 )
                    throw new CommandDefinitionException ( method, $"CommandArgumentRest should only be used on the last parameter of a method." );

                if ( @params[i].IsDefined ( typeof ( ParamArrayAttribute ) ) )
                {
                    if ( i != @params.Length - 1 )
                        throw new CommandDefinitionException ( method, $"Methods with 'params' not as the final parameter are not supported." );
                    if ( @params[i].ParameterType != typeof ( String[] ) )
                        throw new CommandDefinitionException ( method, $"Methods with 'params' must have the 'params' parameter of the String[] type." );
                }
                // piggyback on 'params' check
                else if ( @params[i].ParameterType.IsArray )
                    throw new CommandDefinitionException ( method, $"Methods with non-params array parameters are not supported." );

                // in
                if ( @params[i].IsIn )
                    throw new CommandDefinitionException ( method, $"Methods with 'in' parameters are not supported." );

                // ref and out
                if ( @params[i].ParameterType.IsByRef )
                {
                    if ( @params[i].IsOut )

                        throw new CommandDefinitionException ( method, $"Methods with 'out' parameters are not supported." );
                    else
                        throw new CommandDefinitionException ( method, $"Methods with 'ref' parameters are not supported." );
                }
            }
        }

        private static void ValidateMethod ( in MethodInfo method, in Object instance )
        {
            // Validate that the method isn't generic
            if ( method.ContainsGenericParameters )
                throw new CommandDefinitionException ( method, $"Generic methods are not supported." );

            ParameterInfo[] @params = method.GetParameters ( );
            if ( method.IsDefined ( typeof ( RawInputAttribute ) ) )
            {
                if ( @params.Length != 1 )
                    throw new CommandDefinitionException ( method, "Raw input command must have a single argument of the String type." );
                if ( @params[0].ParameterType != typeof ( String ) )
                    throw new CommandDefinitionException ( method, "Raw input command must have a singlt argument of the String type." );
            }
            // Validate the parameters of the method
            ValidateParameters ( method, instance, @params );
        }

        #endregion Validation
    }
}
