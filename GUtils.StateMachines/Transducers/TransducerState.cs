using System;
using System.Collections.Generic;
using GUtils.StateMachines.Transducers;

namespace GUtils.StateMachines.Transducers
{
    /// <summary>
    /// Represents a state of the <see cref="Transducer{InputT, OutputT}" />
    /// </summary>
    /// <typeparam name="TInput">
    /// The type of input the <see cref="Transducer{InputT, OutputT}" /> accepts
    /// </typeparam>
    /// <typeparam name="TOutput">
    /// The type of the output the <see cref="Transducer{InputT, OutputT}" /> emits
    /// </typeparam>
    public class TransducerState<TInput, TOutput>
    {
        /// <summary>
        /// The transitions this input has
        /// </summary>
        protected internal readonly Dictionary<TInput, TransducerState<TInput, TOutput>> transitionTable = new Dictionary<TInput, TransducerState<TInput, TOutput>> ( );

        /// <summary>
        /// Whether this is a terminal state (one that has an output related with it)
        /// </summary>
        public Boolean IsTerminal { get; set; }

        /// <summary>
        /// The output of the state if it is a terminal state
        /// </summary>
        public TOutput Output { get; set; }

        /// <summary>
        /// The transitions this input has
        /// </summary>
        public IReadOnlyDictionary<TInput, TransducerState<TInput, TOutput>> TransitionTable => this.transitionTable;

        /// <summary>
        /// Creates a new non-terminal state
        /// </summary>
        public TransducerState ( )
        {
            this.IsTerminal = false;
        }

        /// <summary>
        /// Creates a new terminal state with the ouput
        /// </summary>
        /// <param name="output"></param>
        public TransducerState ( TOutput output )
        {
            this.IsTerminal = true;
            this.Output = output;
        }

        /// <summary>
        /// Retrieves a state from the state graph with this node as a starting point
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> GetState ( TInput input ) =>
            this.transitionTable.ContainsKey ( input )
                ? this.transitionTable[input]
                : this.transitionTable[input] = new TransducerState<TInput, TOutput> ( );

        /// <summary>
        /// Retrieves a state from the state graph with this node as a starting point
        /// </summary>
        /// <param name="sequence">The string of inputs that would lead to the desired state</param>
        /// <param name="startingIndex">The index at which to start reading the inputs from</param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> GetState ( TInput[] sequence, Int32 startingIndex = 0 )
        {
            if ( sequence is null )
                throw new ArgumentNullException ( nameof ( sequence ) );

            if ( startingIndex < 0 || startingIndex >= sequence.Length )
                throw new ArgumentOutOfRangeException ( nameof ( startingIndex ) );

            return startingIndex < sequence.Length - 1
                ? this.GetState ( sequence[startingIndex] ).GetState ( sequence, startingIndex + 1 )
                : this.GetState ( sequence[startingIndex] );
        }

        /// <summary>
        /// Retrieves a state from the state graph starting at this node
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> SetStateOutput ( TInput input, TOutput output )
        {
            if ( this.transitionTable.ContainsKey ( input ) )
            {
                TransducerState<TInput, TOutput> state = this.transitionTable[input];

                // Create a new state with all transitions of the previous state
                var newState = new TransducerState<TInput, TOutput> ( output );
                foreach ( KeyValuePair<TInput, TransducerState<TInput, TOutput>> kv in state.transitionTable )
                    newState.transitionTable[kv.Key] = kv.Value;

                return this.transitionTable[input] = newState;
            }
            else
            {
                return this.transitionTable[input] = new TransducerState<TInput, TOutput> ( output );
            }
        }

        /// <summary>
        /// Retrieves a state from the state graph with this node as a starting point
        /// </summary>
        /// <param name="sequence">The string of inputs that would lead to the desired state</param>
        /// <param name="output">The output the desired state will have</param>
        /// <param name="startingIndex">The index at which to start reading the inputs from</param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> SetStateOutput ( TInput[] sequence, TOutput output, Int32 startingIndex = 0 )
        {
            if ( sequence is null )
                throw new ArgumentNullException ( nameof ( sequence ) );

            if ( startingIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( sequence ) );

            return startingIndex < sequence.Length - 1
                ? this.GetState ( sequence[startingIndex] ).GetState ( sequence, startingIndex + 1 )
                : this.SetStateOutput ( sequence[startingIndex], output );
        }

        /// <summary>
        /// Adds a new transition to a new terminal state
        /// </summary>
        /// <param name="input">The input that will trigger the transition</param>
        /// <param name="output">The output that will be returned in this state</param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> OnInput ( TInput input, TOutput output )
        {
            this.SetStateOutput ( input, output );
            return this;
        }

        /// <summary>
        /// Adds a new transition to a new non-terminal state
        /// </summary>
        /// <param name="input">The input that will trigger the transition</param>
        /// <param name="action">The action to configure the transitions of the new state</param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> OnInput ( TInput input, Action<TransducerState<TInput, TOutput>> action )
        {
            if ( action == null )
                throw new ArgumentNullException ( nameof ( action ) );

            action ( this.GetState ( input ) );
            return this;
        }

        /// <summary>
        /// Adds a new transition to a new terminal state
        /// </summary>
        /// <param name="input">THe input that will trigger the transition</param>
        /// <param name="output">The output that will be returned on this state</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> OnInput ( TInput input, TOutput output, Action<TransducerState<TInput, TOutput>> action )
        {
            if ( action == null )
                throw new ArgumentNullException ( nameof ( action ) );

            action ( this.SetStateOutput ( input, output ) );
            return this;
        }

        /// <summary>
        /// Adds a new transition to a non-terminal state
        /// </summary>
        /// <param name="sequence">The string of inputs that will trigger the transition</param>
        /// <param name="action">
        /// The action that will configure the transitions of the non-terminal state
        /// </param>
        /// <param name="startIndex">
        /// The index to start adding transitions from the <paramref name="sequence" />
        /// </param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> OnInput ( TInput[] sequence, Action<TransducerState<TInput, TOutput>> action, Int32 startIndex = 0 )
        {
            if ( sequence is null )
                throw new ArgumentNullException ( nameof ( sequence ) );

            if ( action == null )
                throw new ArgumentNullException ( nameof ( action ) );

            if ( startIndex < 0 || startIndex >= sequence.Length )
                throw new ArgumentOutOfRangeException ( nameof ( startIndex ), "Index was outside the bounds of the string." );

            if ( startIndex < sequence.Length - 1 )
                this.OnInput ( sequence[startIndex], state => state.OnInput ( sequence, action, startIndex + 1 ) );
            else
                this.OnInput ( sequence[startIndex], action );
            return this;
        }

        /// <summary>
        /// Adds a new transition to a terminal state
        /// </summary>
        /// <param name="sequence">The string of inputs that will trigger the transition</param>
        /// <param name="output">The output of the terminal state</param>
        /// <param name="startIndex">
        /// The index to start adding transitions from the <paramref name="sequence" />
        /// </param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> OnInput ( TInput[] sequence, TOutput output, Int32 startIndex = 0 )
        {
            if ( sequence is null )
                throw new ArgumentNullException ( nameof ( sequence ) );

            if ( startIndex < 0 || startIndex >= sequence.Length )
                throw new ArgumentOutOfRangeException ( nameof ( startIndex ), "Index was outside the bounds of the string." );

            if ( startIndex < sequence.Length - 1 )
                this.OnInput ( sequence[startIndex], state => state.OnInput ( sequence, output, startIndex + 1 ) );
            else
                this.OnInput ( sequence[startIndex], output );

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
        /// <param name="startIndex">
        /// The index to start adding transitions from the <paramref name="sequence" />
        /// </param>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> OnInput ( TInput[] sequence, TOutput output, Action<TransducerState<TInput, TOutput>> action, Int32 startIndex = 0 )
        {
            if ( sequence is null )
                throw new ArgumentNullException ( nameof ( sequence ) );

            if ( action is null )
                throw new ArgumentNullException ( nameof ( action ) );

            if ( startIndex < 0 || startIndex >= sequence.Length )
                throw new ArgumentOutOfRangeException ( nameof ( startIndex ), "Index was outside the bounds of the string." );

            if ( startIndex < sequence.Length - 1 )
                this.OnInput ( sequence[startIndex], state => state.OnInput ( sequence, output, action, startIndex + 1 ) );
            else
                this.OnInput ( sequence[startIndex], output, action );

            return this;
        }

        /// <summary>
        /// Creates a shallow copy of this state
        /// </summary>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> ShallowCopy ( )
        {
            TransducerState<TInput, TOutput> state = this.IsTerminal ? new TransducerState<TInput, TOutput> ( this.Output ) : new TransducerState<TInput, TOutput> ( );
            foreach ( KeyValuePair<TInput, TransducerState<TInput, TOutput>> kv in this.transitionTable )
                state.transitionTable[kv.Key] = kv.Value;
            return state;
        }

        /// <summary>
        /// Creates a deep copy of this state
        /// </summary>
        /// <returns></returns>
        public TransducerState<TInput, TOutput> DeepCopy ( )
        {
            TransducerState<TInput, TOutput> state = this.IsTerminal ? new TransducerState<TInput, TOutput> ( this.Output ) : new TransducerState<TInput, TOutput> ( );
            foreach ( KeyValuePair<TInput, TransducerState<TInput, TOutput>> kv in this.transitionTable )
                state.transitionTable[kv.Key] = kv.Value.DeepCopy ( );
            return state;
        }
    }
}
