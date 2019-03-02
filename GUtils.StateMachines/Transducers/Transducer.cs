using System;
using System.Collections.Generic;

namespace GUtils.StateMachines.Transducers
{
    /// <summary>
    /// A Transducer Finite State Machine
    /// </summary>
    /// <typeparam name="InputT">The type of input this transducer accepts</typeparam>
    /// <typeparam name="OutputT">The type of output this transducer emits</typeparam>
    public class Transducer<InputT, OutputT>
    {
        /// <summary>
        /// The initial state of the <see cref="Transducer{InputT, OutputT}" />
        /// </summary>
        public TransducerState<InputT, OutputT> InitialState { get; }

        /// <summary>
        /// Initializes a transducer with a non-terminal <see cref="InitialState" />
        /// </summary>
        public Transducer ( )
        {
            this.InitialState = new TransducerState<InputT, OutputT> ( );
        }

        /// <summary>
        /// Initializes a transducer with a terminal <see cref="InitialState" />
        /// </summary>
        /// <param name="output"></param>
        public Transducer ( OutputT output )
        {
            this.InitialState = new TransducerState<InputT, OutputT> ( output );
        }

        /// <summary>
        /// Creates a deep copy of this transducer with a new output for the <see cref="InitialState" />
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public Transducer<InputT, OutputT> WithDefaultOutput ( OutputT output )
        {
            var transducer = new Transducer<InputT,OutputT> ( output );
            foreach ( KeyValuePair<InputT, TransducerState<InputT, OutputT>> kv in this.InitialState.transitionTable )
                transducer.InitialState.transitionTable[kv.Key] = kv.Value.DeepCopy ( );
            return transducer;
        }

        /// <summary>
        /// Creates a deep copy of this transducer without an output for the <see cref="InitialState" />
        /// </summary>
        /// <returns></returns>
        public Transducer<InputT, OutputT> WithoutDefaultOutput ( )
        {
            var transducer = new Transducer<InputT,OutputT> ( );
            foreach ( KeyValuePair<InputT, TransducerState<InputT, OutputT>> kv in this.InitialState.transitionTable )
                transducer.InitialState.transitionTable[kv.Key] = kv.Value.DeepCopy ( );
            return transducer;
        }

        #region Copiable

        private Transducer ( Boolean isShallowCopy, Transducer<InputT, OutputT> transducer )
        {
            this.InitialState = isShallowCopy ? transducer.InitialState.ShallowCopy ( ) : transducer.InitialState.DeepCopy ( );
        }

        /// <summary>
        /// Creates a shallow copy of this transducer
        /// </summary>
        /// <returns></returns>
        public Transducer<InputT, OutputT> ShallowCopy ( ) => new Transducer<InputT, OutputT> ( true, this );

        /// <summary>
        /// Creates a deep copy of this transducer
        /// </summary>
        /// <returns></returns>
        public Transducer<InputT, OutputT> DeepCopy ( ) => new Transducer<InputT, OutputT> ( false, this );

        #endregion Copiable

        /// <summary>
        /// Executes this state machine on a string of inputs until no transitions happen anymore or the
        /// end of the string is reached
        /// </summary>
        /// <param name="string">The string of inputs</param>
        /// <param name="output">The output of the execution</param>
        /// <returns>The amount of inputs read</returns>
        public Int32 Execute ( IEnumerable<InputT> @string, out OutputT output )
        {
            if ( @string == null )
                throw new ArgumentNullException ( nameof ( @string ) );

            var consumedInputs = 0;
            TransducerState<InputT, OutputT> state = this.InitialState;
            foreach ( InputT value in @string )
            {
                if ( !state.TransitionTable.TryGetValue ( value, out TransducerState<InputT, OutputT> tmp ) )
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
