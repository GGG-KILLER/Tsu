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
using System.Globalization;
using System.Linq;
using System.Reflection;
using Tsu.CLI.Commands.Errors;
using Tsu.CLI.Commands.Help;

namespace Tsu.CLI.Commands
{
    /// <summary>
    /// Manages the registering and executing of commands
    /// </summary>
    public class CompiledCommandManager : BaseCommandManager
    {
        /// <summary>
        /// The list containing all commands
        /// </summary>
        protected List<Command> CommandList { get; }

        /// <summary>
        /// The lookup table used on command name resolution
        /// </summary>
        protected Dictionary<String, Command> CommandLookupTable { get; }

        /// <inheritdoc />
        public override IReadOnlyList<Command> Commands => this.CommandList.AsReadOnly ( );

        /// <inheritdoc />
        public override IReadOnlyDictionary<String, Command> CommandDictionary => this.CommandLookupTable;

        /// <summary>
        /// Initializes a <see cref="CompiledCommandManager"/>
        /// </summary>
        public CompiledCommandManager ( )
        {
            this.CommandList = new List<Command> ( );
            this.CommandLookupTable = new Dictionary<String, Command> ( );
        }

        private static String GetFullName ( MethodInfo method, Object inst ) =>
            $"{inst?.GetType ( ).FullName ?? method.DeclaringType.FullName}.{method.Name}";

        #region Commands Loading

        /// <summary>
        /// Loads all methods tagged with <see cref="CommandAttribute"/> from a given type, be they
        /// public or private.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">Instance to use when invoking the methods (null for static classes)</param>
        public void LoadCommands<T> ( T instance ) where T : class =>
            this.LoadCommands ( typeof ( T ), instance );

        /// <summary>
        /// Loads all methods tagged with <see cref="CommandAttribute"/> from a given type, be they
        /// public or private.
        /// </summary>
        /// <param name="type">Type where to load commands from</param>
        /// <param name="instance">Instance to use when invoking the methods (null for static classes)</param>
        public void LoadCommands ( Type type, Object instance )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            // Find static/non-static methods (choose between static and instance by the presence of
            // a non-null instance)
            foreach ( MethodInfo method in type.GetMethods ( BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic )
                .OrderBy ( method => method.Name ) )
            {
                if ( method.IsDefined ( typeof ( CommandAttribute ) ) )
                {
                    // Create a single instance of the command (will validate and compile in the constructor)
                    var command = new CompiledCommand ( method, instance );
                    this.CommandList.Add ( command );

                    // Get all command attributes and then register all of them with the same command
                    foreach ( CommandAttribute attr in method.GetCustomAttributes<CommandAttribute> ( ) )
                    {
                        if ( this.CommandLookupTable.ContainsKey ( attr.Name ) && !attr.Overwrite )
                        {
                            var existingCommand = ( CompiledCommand ) this.CommandLookupTable[attr.Name];
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
        /// <returns>The <see cref="CompiledCommandManager"/> created for the verb</returns>
        public virtual CompiledCommandManager AddVerb ( String verb )
        {
            if ( String.IsNullOrWhiteSpace ( verb ) )
                throw new ArgumentException ( "Verb cannot be null, empty or contain any whitespaces.", nameof ( verb ) );
            if ( verb.Any ( Char.IsWhiteSpace ) )
                throw new ArgumentException ( "Verb cannot have whitespaces.", nameof ( verb ) );
            if ( this.CommandLookupTable.ContainsKey ( verb ) )
                throw new InvalidOperationException ( "A command with this name already exists." );

            // Verb creation
            var verbCommandManager = new CompiledCommandManager ( );
            var verbInst = new Verb ( verbCommandManager );

            // Command registering
            var command = new VerbCompiledCommand (
                verbCommandManager,
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
            if ( cmdClassInstance is null )
                throw new ArgumentNullException ( nameof ( cmdClassInstance ) );
            this.LoadCommands ( cmdClassInstance );
        }

        /// <inheritdoc/>
        public override void Execute ( String line )
        {
            if ( String.IsNullOrEmpty ( line ) )
                return;

            line = line.Trim ( );
#if HAS_STRING_STRINGCOMPARISON_OVERLOADS
            var spaceIdx = line.IndexOf ( ' ', StringComparison.Ordinal );
#else
            var spaceIdx = CultureInfo.InvariantCulture.CompareInfo.IndexOf ( line, ' ', CompareOptions.Ordinal );
#endif
            var cmdName = spaceIdx != -1 ? line.Substring ( 0, spaceIdx ) : line;
            if ( !this.CommandLookupTable.TryGetValue ( cmdName, out Command tmpCmd ) )
                throw new NonExistentCommandException ( cmdName );

            var cmd = ( CompiledCommand ) tmpCmd;
            if ( spaceIdx != -1 )
            {
                IEnumerable<String> parsed;
                if ( cmd.IsRaw )
                    parsed = new[] { line.Substring ( spaceIdx + 1 ) };
                else
                    parsed = InputLineParser.Parse ( line.Substring ( spaceIdx + 1 ) );

                cmd.CompiledCommandDelegate ( cmdName, parsed.ToArray ( ) );
            }
            else
            {
                cmd.CompiledCommandDelegate ( cmdName, Array.Empty<String> ( ) );
            }
        }
    }
}