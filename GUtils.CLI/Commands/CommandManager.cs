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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GUtils.CLI.Commands.Errors;
using GUtils.CLI.Commands.Help;

namespace GUtils.CLI.Commands
{
    public class CommandManager
    {
        internal readonly List<Command> CommandList;
        public IReadOnlyList<Command> Commands => this.CommandList.AsReadOnly ( );
        internal readonly Dictionary<String, Command> CommandLookupTable;

        private readonly Boolean ShouldUseSimpleParsing;

        public CommandManager ( in CommandManagerFlags flags = CommandManagerFlags.Default )
        {
            this.CommandList = new List<Command> ( );
            this.CommandLookupTable = new Dictionary<String, Command> ( );
            this.ShouldUseSimpleParsing = ( flags & CommandManagerFlags.UseSimpleParsing ) != 0;
        }

        private static String GetFullName ( in MethodInfo method, in Object inst ) => $"{inst?.GetType ( ).FullName ?? method.DeclaringType.FullName}.{method.Name}";

        #region Commands Loading

        /// <summary>
        /// Loads all methods tagged with
        /// <see cref="CommandAttribute" /> from a given type, be
        /// they public or private.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">
        /// Instance to use when invoking the methods (null for
        /// static classes)
        /// </param>
        public void LoadCommands<T> ( in T instance ) where T : class => this.LoadCommands ( typeof ( T ), instance );

        /// <summary>
        /// Loads all methods tagged with
        /// <see cref="CommandAttribute" /> from a given type, be
        /// they public or private.
        /// </summary>
        /// <param name="type">Type where to load commands from</param>
        /// <param name="instance">
        /// Instance to use when invoking the methods (null for
        /// static classes)
        /// </param>
        public void LoadCommands ( in Type type, in Object instance )
        {
            const BindingFlags flagsForAllMethods = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            // Find static/non-static methods (choose between
            // static and instance by the presence of a non-null instance)
            foreach ( MethodInfo method in type.GetMethods ( flagsForAllMethods )
                .OrderBy ( method => method.Name ) )
            {
                if ( method.IsDefined ( typeof ( CommandAttribute ) ) )
                {
                    // Create a single instance of the command
                    // (will validate and compile in the constructor)
                    var command = new Command ( method, instance );
                    this.CommandList.Add ( command );

                    // Get all command attributes and then
                    // register all of them with the same command
                    foreach ( CommandAttribute attr in method.GetCustomAttributes<CommandAttribute> ( ) )
                    {
                        if ( this.CommandLookupTable.ContainsKey ( attr.Name ) && !attr.Overwrite )
                        {
                            Command existingCommand = this.CommandLookupTable[attr.Name];
                            throw new CommandDefinitionException ( method, $"Command name {attr.Name} is already defined in: {GetFullName ( existingCommand.Method, existingCommand.Instance )}" );
                        }
                        this.CommandLookupTable[attr.Name] = command;
                    }
                }
            }
        }

        #endregion Commands Loading

        /// <summary>
        /// Adds the <see cref="DefaultHelpCommand"/> to this command manager
        /// </summary>
        public void AddHelpCommand ( ) => this.AddHelpCommand ( new DefaultHelpCommand ( this ) );

        /// <summary>
        /// Adds a help command to this command manager
        /// </summary>
        /// <param name="cmdClassInstance"></param>
        public void AddHelpCommand<T> ( T cmdClassInstance ) where T : DefaultHelpCommand
        {
            if ( cmdClassInstance == null )
                throw new ArgumentNullException ( nameof ( cmdClassInstance ) );
            this.LoadCommands ( cmdClassInstance );
        }

        /// <summary>
        /// Parses a CLI input line and executes the appropriate
        /// command passing the proper arguments
        /// </summary>
        /// <param name="line"></param>
        public void Execute ( String line )
        {
            if ( String.IsNullOrEmpty ( line ) )
                throw new CommandInvocationException ( String.Empty, "No command provided." );

            line = line.Trim ( );
            var spaceIdx = line.IndexOf ( ' ' );
            var cmdName = spaceIdx != -1 ? line.Substring ( 0, spaceIdx ) : line;
            Command cmd;
            if ( !this.CommandLookupTable.TryGetValue ( cmdName, out cmd ) )
                throw new NonExistentCommandException ( cmdName );

            if ( cmd.IsRaw )
                cmd.CompiledCommand ( cmdName, new[] { spaceIdx != -1
                        ? line.Substring ( spaceIdx + 1 )
                        : String.Empty } );
            else if ( spaceIdx != -1 )
                cmd.CompiledCommand ( cmdName, ( this.ShouldUseSimpleParsing
                    ? InputLineParser.SimpleParse ( line.Substring ( spaceIdx + 1 ) )
                    : InputLineParser.Parse ( line.Substring ( spaceIdx + 1 ) ) )
                    .ToArray ( ) );
            else
                cmd.CompiledCommand ( cmdName, Array.Empty<String> ( ) );
        }
    }
}
