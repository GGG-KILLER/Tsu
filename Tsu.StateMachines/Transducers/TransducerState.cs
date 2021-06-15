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
using System.Diagnostics.CodeAnalysis;

namespace Tsu.StateMachines.Transducers
{
    /// <summary>
    /// Represents a state of the <see cref="Transducer{InputT, OutputT}"/>
    /// </summary>
    /// <typeparam name="TInput">
    /// The type of input the <see cref="Transducer{InputT, OutputT}"/> accepts
    /// </typeparam>
    /// <typeparam name="TOutput">
    /// The type of the output the <see cref="Transducer{InputT, OutputT}"/> emits
    /// </typeparam>
    public class TransducerState<TInput, TOutput>
        where TInput : notnull
    {
        /// <summary>
        /// The transitions this input has
        /// </summary>
        protected internal Dictionary<TInput, TransducerState<TInput, TOutput>> InternalTransitionTable { get; } = new Dictionary<TInput, TransducerState<TInput, TOutput>>();

        /// <summary>
        /// Whether this is a terminal state (one that has an output related with it)
        /// </summary>
        [MemberNotNullWhen(true, nameof(Output))]
        public bool IsTerminal { get; set; }

        /// <summary>
        /// The output of the state if it is a terminal state
        /// </summary>
        public TOutput? Output { get; set; }

        /// <summary>
        /// The transitions this input has
        /// </summary>
        public IReadOnlyDictionary<TInput, TransducerState<TInput, TOutput>> TransitionTable => InternalTransitionTable;

        /// <summary>
        /// Creates a new non-terminal state
        /// </summary>
        public TransducerState()
        {
            IsTerminal = false;
        }

        /// <summary>
        /// Creates a new terminal state with the ouput
        /// </summary>
        /// <param name="output"></param>
        public TransducerState(TOutput output)
        {
            IsTerminal = true;
            Output = output;
        }

        /// <summary>
        /// Retrieves a state from the state graph with this node as a starting point
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> GetState(TInput input) =>
            InternalTransitionTable.ContainsKey(input)
                ? InternalTransitionTable[input]
                : InternalTransitionTable[input] = new TransducerState<TInput, TOutput>();

        /// <summary>
        /// Retrieves a state from the state graph with this node as a starting point
        /// </summary>
        /// <param name="sequence">The string of inputs that would lead to the desired state</param>
        /// <param name="startingIndex">The index at which to start reading the inputs from</param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> GetState(TInput[] sequence, int startingIndex = 0)
        {
            if (sequence is null)
                throw new ArgumentNullException(nameof(sequence));

            if (startingIndex < 0 || startingIndex >= sequence.Length)
                throw new ArgumentOutOfRangeException(nameof(startingIndex));

            return startingIndex < sequence.Length - 1
                ? GetState(sequence[startingIndex]).GetState(sequence, startingIndex + 1)
                : GetState(sequence[startingIndex]);
        }

        /// <summary>
        /// Retrieves a state from the state graph starting at this node
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> SetStateOutput(TInput input, TOutput output)
        {
            if (InternalTransitionTable.ContainsKey(input))
            {
                var state = InternalTransitionTable[input];

                // Create a new state with all transitions of the previous state
                var newState = new TransducerState<TInput, TOutput>(output);
                foreach (var kv in state.InternalTransitionTable)
                    newState.InternalTransitionTable[kv.Key] = kv.Value;

                return InternalTransitionTable[input] = newState;
            }
            else
            {
                return InternalTransitionTable[input] = new TransducerState<TInput, TOutput>(output);
            }
        }

        /// <summary>
        /// Retrieves a state from the state graph with this node as a starting point
        /// </summary>
        /// <param name="sequence">The string of inputs that would lead to the desired state</param>
        /// <param name="output">The output the desired state will have</param>
        /// <param name="startingIndex">The index at which to start reading the inputs from</param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> SetStateOutput(TInput[] sequence, TOutput output, int startingIndex = 0)
        {
            if (sequence is null)
                throw new ArgumentNullException(nameof(sequence));

            if (startingIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(sequence));

            return startingIndex < sequence.Length - 1
                ? GetState(sequence[startingIndex]).GetState(sequence, startingIndex + 1)
                : SetStateOutput(sequence[startingIndex], output);
        }

        /// <summary>
        /// Adds a new transition to a new terminal state
        /// </summary>
        /// <param name="input">The input that will trigger the transition</param>
        /// <param name="output">The output that will be returned in this state</param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> OnInput(TInput input, TOutput output)
        {
            SetStateOutput(input, output);
            return this;
        }

        /// <summary>
        /// Adds a new transition to a new non-terminal state
        /// </summary>
        /// <param name="input">The input that will trigger the transition</param>
        /// <param name="action">The action to configure the transitions of the new state</param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> OnInput(TInput input, Action<TransducerState<TInput, TOutput>> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            action(GetState(input));
            return this;
        }

        /// <summary>
        /// Adds a new transition to a new terminal state
        /// </summary>
        /// <param name="input">THe input that will trigger the transition</param>
        /// <param name="output">The output that will be returned on this state</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> OnInput(TInput input, TOutput output, Action<TransducerState<TInput, TOutput>> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            action(SetStateOutput(input, output));
            return this;
        }

        /// <summary>
        /// Adds a new transition to a non-terminal state
        /// </summary>
        /// <param name="sequence">The string of inputs that will trigger the transition</param>
        /// <param name="action">
        /// The action that will configure the transitions of the non-terminal state
        /// </param>
        /// <param name="startIndex">The index to start adding transitions from the <paramref name="sequence"/></param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> OnInput(TInput[] sequence, Action<TransducerState<TInput, TOutput>> action, int startIndex = 0)
        {
            if (sequence is null)
                throw new ArgumentNullException(nameof(sequence));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (startIndex < 0 || startIndex >= sequence.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), "Index was outside the bounds of the string.");

            if (startIndex < sequence.Length - 1)
                OnInput(sequence[startIndex], state => state.OnInput(sequence, action, startIndex + 1));
            else
                OnInput(sequence[startIndex], action);
            return this;
        }

        /// <summary>
        /// Adds a new transition to a terminal state
        /// </summary>
        /// <param name="sequence">The string of inputs that will trigger the transition</param>
        /// <param name="output">The output of the terminal state</param>
        /// <param name="startIndex">The index to start adding transitions from the <paramref name="sequence"/></param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> OnInput(TInput[] sequence, TOutput output, int startIndex = 0)
        {
            if (sequence is null)
                throw new ArgumentNullException(nameof(sequence));

            if (startIndex < 0 || startIndex >= sequence.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), "Index was outside the bounds of the string.");

            if (startIndex < sequence.Length - 1)
                OnInput(sequence[startIndex], state => state.OnInput(sequence, output, startIndex + 1));
            else
                OnInput(sequence[startIndex], output);

            return this;
        }

        /// <summary>
        /// Adds a new transition to a terminal state
        /// </summary>
        /// <param name="sequence">The string of inputs that will trigger the transition</param>
        /// <param name="output">The output of the terminal state</param>
        /// <param name="action">
        /// The action that will configure the transitions of the terminal state
        /// </param>
        /// <param name="startIndex">The index to start adding transitions from the <paramref name="sequence"/></param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> OnInput(TInput[] sequence, TOutput output, Action<TransducerState<TInput, TOutput>> action, int startIndex = 0)
        {
            if (sequence is null)
                throw new ArgumentNullException(nameof(sequence));

            if (action is null)
                throw new ArgumentNullException(nameof(action));

            if (startIndex < 0 || startIndex >= sequence.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), "Index was outside the bounds of the string.");

            if (startIndex < sequence.Length - 1)
                OnInput(sequence[startIndex], state => state.OnInput(sequence, output, action, startIndex + 1));
            else
                OnInput(sequence[startIndex], output, action);

            return this;
        }

        /// <summary>
        /// Creates a shallow copy of this state
        /// </summary>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> ShallowCopy()
        {
            var state = IsTerminal ? new TransducerState<TInput, TOutput>(Output) : new TransducerState<TInput, TOutput>();
            foreach (var kv in InternalTransitionTable)
                state.InternalTransitionTable[kv.Key] = kv.Value;
            return state;
        }

        /// <summary>
        /// Creates a deep copy of this state
        /// </summary>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> DeepCopy()
        {
            var state = IsTerminal ? new TransducerState<TInput, TOutput>(Output) : new TransducerState<TInput, TOutput>();
            foreach (var kv in InternalTransitionTable)
                state.InternalTransitionTable[kv.Key] = kv.Value.DeepCopy();
            return state;
        }
    }
}