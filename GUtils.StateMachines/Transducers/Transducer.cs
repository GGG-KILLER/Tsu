using System;
using System.Collections.Generic;

namespace GUtils.StateMachines.Transducers
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
        public Transducer ( )
        {
            this.InitialState = new TransducerState<TInput, TOutput> ( );
        }

        /// <summary>
        /// Initializes a transducer with a terminal <see cref="InitialState" />
        /// </summary>
        /// <param name="output"></param>
        public Transducer ( TOutput output )
        {
            this.InitialState = new TransducerState<TInput, TOutput> ( output );
        }

        /// <summary>
        /// Creates a deep copy of this transducer with a new output for the <see cref="InitialState" />
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public Transducer<TInput, TOutput> WithDefaultOutput ( TOutput output )
        {
            var transducer = new Transducer<TInput,TOutput> ( output );
            foreach ( KeyValuePair<TInput, TransducerState<TInput, TOutput>> kv in this.InitialState.InternalTransitionTable )
                transducer.InitialState.InternalTransitionTable[kv.Key] = kv.Value.DeepCopy ( );
            return transducer;
        }

        /// <summary>
        /// Creates a deep copy of this transducer without an output for the <see cref="InitialState" />
        /// </summary>
        /// <returns></returns>
        public Transducer<TInput, TOutput> WithoutDefaultOutput ( )
        {
            var transducer = new Transducer<TInput,TOutput> ( );
            foreach ( KeyValuePair<TInput, TransducerState<TInput, TOutput>> kv in this.InitialState.InternalTransitionTable )
                transducer.InitialState.InternalTransitionTable[kv.Key] = kv.Value.DeepCopy ( );
            return transducer;
        }

        #region Copiable

        private Transducer ( Boolean isShallowCopy, Transducer<TInput, TOutput> transducer )
        {
            this.InitialState = isShallowCopy ? transducer.InitialState.ShallowCopy ( ) : transducer.InitialState.DeepCopy ( );
        }

        /// <summary>
        /// Creates a shallow copy of this transducer
        /// </summary>
        /// <returns></returns>
        public Transducer<TInput, TOutput> ShallowCopy ( ) => new Transducer<TInput, TOutput> ( true, this );

        /// <summary>
        /// Creates a deep copy of this transducer
        /// </summary>
        /// <returns></returns>
        public Transducer<TInput, TOutput> DeepCopy ( ) => new Transducer<TInput, TOutput> ( false, this );

        #endregion Copiable

        /// <summary>
        /// Executes this state machine on a string of inputs until no transitions happen anymore or the
        /// end of the string is reached
        /// </summary>
        /// <param name="sequence">The string of inputs</param>
        /// <param name="output">The output of the execution</param>
        /// <returns>The amount of inputs read</returns>
        public Int32 Execute ( IEnumerable<TInput> sequence, out TOutput? output )
        {
            if ( sequence == null )
                throw new ArgumentNullException ( nameof ( sequence ) );

            var consumedInputs = 0;
            TransducerState<TInput, TOutput> state = this.InitialState;
            foreach ( TInput value in sequence )
            {
                if ( !state.TransitionTable.TryGetValue ( value, out TransducerState<TInput, TOutput>? tmp ) )
                    break;
                state = tmp;
                consumedInputs++;
            }

            if ( state.IsTerminal )
            {
                output = state.Output;
                return consumedInputs;
            }

            output = default;
            return -1;
        }
    }
}
