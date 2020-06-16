using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GUtils.CLI.Commands.Errors;
using GUtils.CLI.Commands.Help;

namespace GUtils.CLI.Commands
{
    /// <summary>
    /// A compiled command
    /// </summary>
    public class CompiledCommand : Command
    {
        private static ArgumentModifiers GetArgumentModifiers ( ParameterInfo info )
        {
            ArgumentModifiers mods = ArgumentModifiers.Required;

#pragma warning disable CS0618 // Type or member is obsolete
            if ( info.IsDefined ( typeof ( JoinRestOfArgumentsAttribute ) ) )
                mods |= ArgumentModifiers.JoinRest;
#pragma warning restore CS0618 // Type or member is obsolete
            if ( info.IsDefined ( typeof ( ParamArrayAttribute ) ) )
                mods |= ArgumentModifiers.Params;
            if ( info.HasDefaultValue )
                mods |= ArgumentModifiers.Optional;

            return mods;
        }

        private static IEnumerable<ArgumentHelpData> GetArgumentsHelpData ( MethodInfo method )
        {
            return method.GetParameters ( )
                         .Select ( arg => new ArgumentHelpData ( arg.Name,
                                                                 "",
                                                                 GetArgumentModifiers ( arg ),
                                                                 arg.ParameterType ) );
        }

        private static IEnumerable<String> ValidateMethodAndGetCommandNames ( MethodInfo method )
        {
            ValidateMethod ( method );

            return method.GetCustomAttributes<CommandAttribute> ( )
                         .Select ( cmd => cmd.Name );
        }

        private static IEnumerable<ArgumentHelpData> GetArguments ( MethodInfo method )
        {
            return method.GetParameters ( )
                         .Select ( arg => new ArgumentHelpData (
                                 arg.Name,
                                 arg.GetCustomAttribute<HelpDescriptionAttribute> ( )?.Description ?? "No description was provided for this argument.",
                                 GetArgumentModifiers ( arg ),
                                 arg.ParameterType
                         ) );
        }

        internal readonly MethodInfo Method;
        internal readonly Object Instance;
        internal readonly Action<String, String[]> CompiledCommandDelegate;

        /// <summary>
        /// Initializes a new compiled command instance compiling the conversion method.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="instance"></param>
        /// <param name="names"></param>
        /// <param name="description"></param>
        /// <param name="isRaw"></param>
        /// <param name="examples"></param>
        internal CompiledCommand ( MethodInfo method,
                         Object instance,
                         IEnumerable<String> names,
                         String description = "No description provided for this command.",
                         Boolean isRaw = false,
                         IEnumerable<String> examples = null ) : base ( names, description, isRaw, GetArgumentsHelpData ( method ), examples )
        {
            if ( names == null )
                throw new ArgumentNullException ( nameof ( names ) );

            if ( !names.Any ( ) )
                throw new ArgumentException ( "No names provided", nameof ( names ) );

            this.Method = method ?? throw new ArgumentNullException ( nameof ( method ) );
            this.Instance = instance;
            this.CompiledCommandDelegate = CommandCompiler.Compile ( method, instance );
        }

        /// <summary>
        /// Initializes a <see cref="Command"/>
        /// </summary>
        /// <param name="method"></param>
        /// <param name="instance"></param>
        internal CompiledCommand ( MethodInfo method, Object instance )
            : base ( names: ValidateMethodAndGetCommandNames ( method ),
                     description: method.GetCustomAttribute<HelpDescriptionAttribute> ( )?.Description ?? "No description was provided for this command.",
                     isRaw: method.IsDefined ( typeof ( RawInputAttribute ) ),
                     arguments: GetArguments ( method ),
                     examples: method.GetCustomAttributes<HelpExampleAttribute> ( ).Select ( ex => ex.Example ) )
        {
            this.Method = method;
            this.Instance = instance;
            this.CompiledCommandDelegate = CommandCompiler.Compile ( method, instance );
        }

        #region Validation

        private static void ValidateParameters ( MethodInfo method, ParameterInfo[] @params )
        {
            // Validate that the method does not contain any out
            // parameters and that the rest attribute is not
            // defined for an argument that isn't the last
            for ( var i = 0; i < @params.Length; i++ )
            {
#pragma warning disable CS0618 // Type or member is obsolete
                if ( @params[i].IsDefined ( typeof ( JoinRestOfArgumentsAttribute ) ) && i != @params.Length - 1 )
                    throw new CommandDefinitionException ( method, $"CommandArgumentRest should only be used on the last parameter of a method." );
#pragma warning restore CS0618 // Type or member is obsolete

                if ( @params[i].IsDefined ( typeof ( ParamArrayAttribute ) ) )
                {
                    // params T[], ...)
                    if ( i != @params.Length - 1 )
                        throw new CommandDefinitionException ( method, $"Methods with 'params' not as the final parameter are not supported." );
                    // params T[,]
                    if ( @params[i].ParameterType.GetArrayRank ( ) != 1 )
                        throw new CommandDefinitionException ( method, "Methods with 'params' and multi-dimensional arrays are not supported." );
                    // params T[][]
                    if ( @params[i].ParameterType.GetElementType ( ).IsArray )
                        throw new CommandDefinitionException ( method, "Methods with 'params' and arrays of arrays are not supported." );
                }
                // piggyback on 'params' check
                else if ( @params[i].ParameterType.IsArray )
                {
                    throw new CommandDefinitionException ( method, $"Methods with non-params array parameters are not supported." );
                }

                // in
                if ( @params[i].IsIn )
                    throw new CommandDefinitionException ( method, $"Methods with 'in' parameters are not supported." );

                // out
                if ( @params[i].IsOut )
                    throw new CommandDefinitionException ( method, $"Methods with 'out' parameters are not supported." );

                // ref
                if ( @params[i].ParameterType.IsByRef )
                    throw new CommandDefinitionException ( method, $"Methods with 'ref' parameters are not supported." );
            }
        }

        private static void ValidateMethod ( MethodInfo method )
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
            ValidateParameters ( method, @params );
        }

        #endregion Validation
    }
}
