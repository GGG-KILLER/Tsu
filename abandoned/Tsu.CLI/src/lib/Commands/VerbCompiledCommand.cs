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
using System.Reflection;

namespace Tsu.CLI.Commands
{
    /// <summary>
    /// A compiled verb command
    /// </summary>
    public class VerbCompiledCommand : CompiledCommand, IVerbCommand
    {
        /// <inheritdoc/>
        public BaseCommandManager CommandManager { get; }

        internal VerbCompiledCommand(
            BaseCommandManager commandManager,
            MethodInfo method,
            object instance,
            IEnumerable<string> names,
            string description = "No description provided for this command.",
            bool isRaw = false,
            IEnumerable<string> examples = null)
            : base(method, instance, names, description, isRaw, examples)
        {
            CommandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
        }

        internal VerbCompiledCommand(
            BaseCommandManager commandManager,
            MethodInfo method,
            object instance)
            : base(method, instance)
        {
            CommandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
        }
    }
}
