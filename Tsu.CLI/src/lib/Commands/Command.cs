// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tsu.CLI.Commands.Help;

namespace Tsu.CLI.Commands
{
    /// <summary>
    /// Represents a command that was registered in a command manager
    /// </summary>
    public class Command
    {
        /// <summary>
        /// The names that this command can be referred by
        /// </summary>
        public ImmutableArray<string> Names { get; }

        /// <summary>
        /// The description shown when the help command is invoked
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Whether this command accepts raw input
        /// </summary>
        public bool IsRaw { get; }

        /// <summary>
        /// The arguments this command accepts
        /// </summary>
        public ImmutableArray<ArgumentHelpData> Arguments { get; }

        /// <summary>
        /// The command invokation examples provided (or not)
        /// </summary>
        public ImmutableArray<string> Examples { get; }

        /// <summary>
        /// Initializes a new command instance.
        /// </summary>
        /// <param name="names"></param>
        /// <param name="description"></param>
        /// <param name="isRaw"></param>
        /// <param name="arguments"></param>
        /// <param name="examples"></param>
        public Command(IEnumerable<string> names,
                         string description = "No description provided for this command.",
                         bool isRaw = false,
                         IEnumerable<ArgumentHelpData> arguments = null,
                         IEnumerable<string> examples = null)
        {
            if (names == null)
                throw new ArgumentNullException(nameof(names));

            if (!names.Any())
                throw new ArgumentException("No names provided", nameof(names));

            Names = names.ToImmutableArray();
            Description = description ?? throw new ArgumentNullException(nameof(description));
            IsRaw = isRaw;
            Arguments = arguments?.ToImmutableArray() ?? ImmutableArray<ArgumentHelpData>.Empty;
            Examples = examples?.ToImmutableArray() ?? ImmutableArray<string>.Empty;
        }
    }
}
