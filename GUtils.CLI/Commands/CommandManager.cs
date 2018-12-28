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
    /// <summary>
    /// Manages the registering and executing of commands
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// The list containing all commands
        /// </summary>
        protected readonly List<Command> CommandList;

        /// <summary>
        /// The lookup table used on command name resolution
        /// </summary>
        protected readonly Dictionary<String, Command> CommandLookupTable;

        /// <summary>
        /// The commands registered in this <see cref="CommandManager" />
        /// </summary>
        public IReadOnlyList<Command> Commands => this.CommandList.AsReadOnly ( );

        /// <summary>
        /// Exposes the internal command lookup table as a readonly collection
        /// </summary>
        public IReadOnlyDictionary<String, Command> CommandDictionary => this.CommandLookupTable;

        /// <summary>
        /// Initializes a <see cref="CommandManager" />
        /// </summary>
        public CommandManager ( )
        {
            this.CommandList = new List<Command> ( );
            this.CommandLookupTable = new Dictionary<String, Command> ( );
        }

        private static String GetFullName ( in MethodInfo method, in Object inst ) =>
            $"{inst?.GetType ( ).FullName ?? method.DeclaringType.FullName}.{method.Name}";

        #region Commands Loading

        /// <summary>
        /// Loads all methods tagged with <see cref="CommandAttribute" /> from a given type, be they
        /// public or private.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">
        /// Instance to use when invoking the methods (null for static classes)
        /// </param>
        public void LoadCommands<T> ( T instance ) where T : class =>
            this.LoadCommands ( typeof ( T ), instance );

        /// <summary>
        /// Loads all methods tagged with <see cref="CommandAttribute" /> from a given type, be they
        /// public or private.
        /// </summary>
        /// <param name="type">Type where to load commands from</param>
        /// <param name="instance">
        /// Instance to use when invoking the methods (null for static classes)
        /// </param>
        public void LoadCommands ( Type type, Object instance )
        {
            const BindingFlags flagsForAllMethods = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            // Find static/non-static methods (choose between static and instance by the presence of a
            // non-null instance)
            foreach ( MethodInfo method in type.GetMethods ( flagsForAllMethods )
                .OrderBy ( method => method.Name ) )
            {
                if ( method.IsDefined ( typeof ( CommandAttribute ) ) )
                {
                    // Create a single instance of the command (will validate and compile in the
                    // constructor)
                    var command = new Command ( method, instance );
                    this.CommandList.Add ( command );

                    // Get all command attributes and then register all of them with the same command
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

        #region Nested Commands Hack

        /// <summary>
        /// Adds a nested command (or verb, if you will) to this command manager.
        /// </summary>
        /// <param name="verb"></param>
        /// <returns>The <see cref="CommandManager" /> created for the verb</returns>
        public virtual CommandManager AddVerb ( String verb )
        {
            if ( String.IsNullOrWhiteSpace ( verb ) )
                throw new ArgumentException ( "Verb cannot be null, empty or contain any whitespaces.", nameof ( verb ) );
            if ( verb.Any ( Char.IsWhiteSpace ) )
                throw new ArgumentException ( "Verb cannot have whitespaces.", nameof ( verb ) );
            if ( this.CommandLookupTable.ContainsKey ( verb ) )
                throw new InvalidOperationException ( "A command with this name already exists." );

            // Verb creation
            var verbInst = new Verb ( new CommandManager ( ) );

            // Command registering
            var command = new Command (
                typeof ( Verb ).GetMethod ( nameof ( Verb.RunCommand ), BindingFlags.Instance | BindingFlags.Public ),
                verbInst,
                new[] { verb },
                isRaw: true
            );
            this.CommandList.Add ( command );
            this.CommandLookupTable[verb] = command;

            return verbInst.Manager;
        }

        #endregion Nested Commands Hack

        /// <summary>
        /// Adds a help command to this command manager
        /// </summary>
        /// <param name="cmdClassInstance"></param>
        public void AddHelpCommand<T> ( T cmdClassInstance ) where T : HelpCommand
        {
            if ( cmdClassInstance == null )
                throw new ArgumentNullException ( nameof ( cmdClassInstance ) );
            this.LoadCommands ( cmdClassInstance );
        }

        /// <summary>
        /// Parses a CLI input line and executes the appropriate command passing the proper arguments.
        /// </summary>
        /// <param name="line"></param>
        public void Execute ( String line )
        {
            if ( String.IsNullOrEmpty ( line ) )
                return;

            line = line.Trim ( );
            var spaceIdx = line.IndexOf ( ' ' );
            var cmdName = spaceIdx != -1 ? line.Substring ( 0, spaceIdx ) : line;
            Command cmd;
            if ( !this.CommandLookupTable.TryGetValue ( cmdName, out cmd ) )
                throw new NonExistentCommandException ( cmdName );

            if ( spaceIdx != -1 )
            {
                IEnumerable<String> parsed;
                if ( cmd.IsRaw )
                    parsed = new[] { line.Substring ( spaceIdx + 1 ) };
                else
                    parsed = InputLineParser.Parse ( line.Substring ( spaceIdx + 1 ) );

                cmd.CompiledCommand ( cmdName, parsed.ToArray ( ) );
            }
            else
                cmd.CompiledCommand ( cmdName, Array.Empty<String> ( ) );
        }
    }
}
