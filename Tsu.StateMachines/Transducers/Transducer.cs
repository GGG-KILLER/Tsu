// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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

namespace Tsu.StateMachines.Transducers
{
    /// <summary>
    /// A Transducer Finite State Machine
    /// </summary>
    /// <typeparam name="TInput">The type of input this transducer accepts</typeparam>
    /// <typeparam name="TOutput">The type of output this transducer emits</typeparam>
    public class Transducer<TInput, TOutput>
        where TInput : notnull
    {
        /// <summary>
        /// The initial state of the <see cref="Transducer{InputT, OutputT}" />
        /// </summary>
        public TransducerState<TInput, TOutput> InitialState { get; }

        /// <summary>
        /// Initializes a transducer with a non-terminal <see cref="InitialState" />
        /// </summary>
        public Transducer()
        {
            InitialState = new TransducerState<TInput, TOutput>();
        }

        /// <summary>
        /// Initializes a transducer with a terminal <see cref="InitialState" />
        /// </summary>
        /// <param name="output"></param>
        public Transducer(TOutput output)
        {
            InitialState = new TransducerState<TInput, TOutput>(output);
        }

        /// <summary>
        /// Creates a deep copy of this transducer with a new output for the <see cref="InitialState" />
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public Transducer<TInput, TOutput> WithDefaultOutput(TOutput output)
        {
            var transducer = new Transducer<TInput, TOutput>(output);
            foreach (var kv in InitialState.InternalTransitionTable)
                transducer.InitialState.InternalTransitionTable[kv.Key] = kv.Value.DeepCopy();
            return transducer;
        }

        /// <summary>
        /// Creates a deep copy of this transducer without an output for the <see cref="InitialState" />
        /// </summary>
        /// <returns></returns>
        public Transducer<TInput, TOutput> WithoutDefaultOutput()
        {
            var transducer = new Transducer<TInput, TOutput>();
            foreach (var kv in InitialState.InternalTransitionTable)
                transducer.InitialState.InternalTransitionTable[kv.Key] = kv.Value.DeepCopy();
            return transducer;
        }

        #region Copiable

        private Transducer(bool isShallowCopy, Transducer<TInput, TOutput> transducer)
        {
            InitialState = isShallowCopy ? transducer.InitialState.ShallowCopy() : transducer.InitialState.DeepCopy();
        }

        /// <summary>
        /// Creates a shallow copy of this transducer
        /// </summary>
        /// <returns></returns>
        public Transducer<TInput, TOutput> ShallowCopy() => new Transducer<TInput, TOutput>(true, this);

        /// <summary>
        /// Creates a deep copy of this transducer
        /// </summary>
        /// <returns></returns>
        public Transducer<TInput, TOutput> DeepCopy() => new Transducer<TInput, TOutput>(false, this);

        #endregion Copiable

        /// <summary>
        /// Executes this state machine on a string of inputs until no transitions happen anymore or the
        /// end of the string is reached
        /// </summary>
        /// <param name="sequence">The string of inputs</param>
        /// <param name="output">The output of the execution</param>
        /// <returns>The amount of inputs read</returns>
        public int Execute(IEnumerable<TInput> sequence, out TOutput? output)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            var consumedInputs = 0;
            var state = InitialState;
            foreach (var value in sequence)
            {
                if (!state.TransitionTable.TryGetValue(value, out var tmp))
                    break;
                state = tmp;
                consumedInputs++;
            }

            if (state.IsTerminal)
            {
                output = state.Output;
                return consumedInputs;
            }

            output = default;
            return -1;
        }
    }
}
