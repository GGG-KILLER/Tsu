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
using System.Diagnostics;
using System.Text;
using Tsu.Numerics;

namespace Tsu.Timing
{
    /// <summary>
    /// A micro profiler. Basically a tree of Stopwatches with associated names.
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public sealed class MicroProfiler : IDisposable
    {
        /// <summary>
        /// Instantiates and starts a new <see cref="MicroProfiler" />
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MicroProfiler StartNew(string name)
        {
            var prof = new MicroProfiler(name);
            prof._stopwatch.Start();
            return prof;
        }

        /// <summary>
        /// The list of child profilers.
        /// </summary>
        private readonly List<MicroProfiler> _childProfilers;

        /// <summary>
        /// The stopwatch used for timing of this micro profiler.
        /// </summary>
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// The name associated with this microprofiler
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The list of child microprofilers
        /// </summary>
        public IReadOnlyList<MicroProfiler> ChildProfilers => _childProfilers;

        /// <summary>
        /// The total milliseconds elapsed on this operation
        /// </summary>
        public double ElapsedMilliseconds => _stopwatch.ElapsedTicks / Duration.TicksPerMillisecond;

        /// <summary>
        /// Initializes a new MicroProfiler with the given name.
        ///
        /// Does NOT start the internal stopwatch.
        /// </summary>
        /// <param name="name"></param>
        public MicroProfiler(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _childProfilers = new List<MicroProfiler>();
            _stopwatch = new Stopwatch();
        }

        /// <summary>
        /// Instantiates, starts and adds a new <see cref="MicroProfiler" /> with the provided
        /// <paramref name="name" /> to the <see cref="ChildProfilers" />.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MicroProfiler StartChild(string name)
        {
            var res = StartNew(name);
            _childProfilers.Add(res);
            return res;
        }

        /// <summary>
        /// Starts the internal stopwatch
        /// </summary>
        public void Start() => _stopwatch.Start();

        /// <summary>
        /// Restarts the internal stopwatch
        /// </summary>
        public void Restart() => _stopwatch.Restart();

        /// <summary>
        /// Stops the internal stopwatch
        /// </summary>
        public void Stop() => _stopwatch.Stop();

        /// <summary>
        /// Resets the internal stopwatch
        /// </summary>
        public void Reset() => _stopwatch.Reset();

        /// <summary>
        /// Writes the tree of timings to the provided <paramref name="builder" />.
        /// </summary>
        /// <param name="builder"></param>
        public void WriteTreeString(StringBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            WriteTreeString(builder, "", true, true);
        }

        /// <summary>
        /// Outputs the tree of <see cref="MicroProfiler" /> s as an ASCII-like tree.
        /// </summary>
        /// <remarks>Uses the followign unicode characters: │, ├, ─ and └</remarks>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            WriteTreeString(sb);
            return sb.ToString();
        }

        /// <inheritdoc/>
        public void Dispose() => Stop();

        private string GetDebuggerDisplay() => ToString();

        /// <summary>
        /// Recursively writes the tree containing all timings to the provided
        /// <paramref name="builder" />.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="StringBuilder" /> where all output will be written to.
        /// </param>
        /// <param name="indent">The indentation up to this level.</param>
        /// <param name="isLast">
        /// Whether this is the last compiler in it's parent node.
        /// </param>
        /// <param name="isRoot">Whether this is the root microprofiler.</param>
        private void WriteTreeString(StringBuilder builder, string indent = "", bool isLast = true, bool isRoot = false)
        {
            builder.Append(indent);
            if (!isRoot)
                builder.Append(isLast ? "└─ " : "├─ ");
            builder.AppendLine($"{Name}: {Duration.Format(_stopwatch.ElapsedTicks)}");

            if (!isRoot)
                indent += isLast ? "   " : "|  ";
            var childResults = _childProfilers;
            for (var i = 0; i < childResults.Count; i++)
            {
                childResults[i].WriteTreeString(builder, indent, i == childResults.Count - 1);
            }
        }
    }
}
