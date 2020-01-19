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
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using GUtils.CLI.Commands.Errors;
using GUtils.CLI.Commands.Help;

// Disable obsolete warnings since we need backwards compatibility
#pragma warning disable CS0618

namespace GUtils.CLI.Commands
{
    /// <summary>
    /// Represents a command that was registered in the <see cref="CommandManager"/>
    /// </summary>
    public class Command
    {
        /// <summary>
        /// The names that this command can be referred by
        /// </summary>
        public ImmutableArray<String> Names { get; }

        /// <summary>
        /// The description shown when the help command is invoked
        /// </summary>
        public String Description { get; }

        /// <summary>
        /// Whether this command accepts raw input
        /// </summary>
        public Boolean IsRaw { get; }

        /// <summary>
        /// The arguments this command accepts
        /// </summary>
        public ImmutableArray<ArgumentHelpData> Arguments { get; }

        /// <summary>
        /// The command invokation examples provided (or not)
        /// </summary>
        public ImmutableArray<String> Examples { get; }

        internal readonly MethodInfo Method;
        internal readonly Object Instance;
        internal readonly Action<String, String[]> CompiledCommand;

        private static ArgumentModifiers GetArgumentModifiers ( ParameterInfo info )
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

        internal Command ( MethodInfo method, Object instance, IEnumerable<String> names, String description = "No description provided for this command.", Boolean isRaw = false, IEnumerable<String> examples = null )
        {
            if ( names == null )
                throw new ArgumentNullException ( nameof ( names ) );

            if ( !names.Any ( ) )
                throw new ArgumentException ( "No names provided", nameof ( names ) );

            this.Names = names.ToImmutableArray ( );
            this.Description = description ?? throw new ArgumentNullException ( nameof ( description ) );
            this.IsRaw = isRaw;
            this.Method = method ?? throw new ArgumentNullException ( nameof ( method ) );
            this.Instance = instance;
            this.Arguments = method.GetParameters ( )
                                   .Select ( arg => new ArgumentHelpData ( arg.Name,
                                                                           "",
                                                                           GetArgumentModifiers ( arg ),
                                                                           arg.ParameterType ) )
                                   .ToImmutableArray ( );
            this.Examples = examples?.ToImmutableArray ( ) ?? ImmutableArray<String>.Empty;
            this.CompiledCommand = CommandCompiler.Compile ( method, instance );
        }

        /// <summary>
        /// Initializes a <see cref="Command"/>
        /// </summary>
        /// <param name="method"></param>
        /// <param name="instance"></param>
        internal Command ( MethodInfo method, Object instance )
        {
            ValidateMethod ( method );

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

        private static void ValidateParameters ( MethodInfo method, ParameterInfo[] @params )
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
