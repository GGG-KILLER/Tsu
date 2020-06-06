/*
 * Copyright � 2019 GGG KILLER <gggkiller2@gmail.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the �Software�), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
 * the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using GUtils.CLI.Commands.Help;

// Disable obsolete warnings since we need backwards compatibility
#pragma warning disable CS0618

namespace GUtils.CLI.Commands
{
    /// <summary>
    /// Represents a command that was registered in a command manager
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

        /// <summary>
        /// Initializes a new command instance.
        /// </summary>
        /// <param name="names"></param>
        /// <param name="description"></param>
        /// <param name="isRaw"></param>
        /// <param name="arguments"></param>
        /// <param name="examples"></param>
        public Command ( IEnumerable<String> names,
                         String description = "No description provided for this command.",
                         Boolean isRaw = false,
                         IEnumerable<ArgumentHelpData> arguments = null,
                         IEnumerable<String> examples = null )
        {
            if ( names == null )
                throw new ArgumentNullException ( nameof ( names ) );

            if ( !names.Any ( ) )
                throw new ArgumentException ( "No names provided", nameof ( names ) );

            this.Names = names.ToImmutableArray ( );
            this.Description = description ?? throw new ArgumentNullException ( nameof ( description ) );
            this.IsRaw = isRaw;
            this.Arguments = arguments?.ToImmutableArray ( ) ?? ImmutableArray<ArgumentHelpData>.Empty;
            this.Examples = examples?.ToImmutableArray ( ) ?? ImmutableArray<String>.Empty;
        }
    }
}
