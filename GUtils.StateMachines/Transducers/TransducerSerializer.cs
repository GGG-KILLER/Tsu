using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using GUtils.Expressions;

namespace GUtils.StateMachines.Transducers
{
    /// <summary>
    /// Serializes a transducer into another format
    /// </summary>
    public static class TransducerSerializer
    {
        /// <summary>
        /// Transforms the transducer into an expression tree
        /// </summary>
        /// <typeparam name="InputT">The type of input accepted by the transducer</typeparam>
        /// <typeparam name="OutputT">The type of output emitted by the transducer</typeparam>
        /// <param name="transducer">The transducer to compile</param>
        /// <returns></returns>
        public static Expression<Func<IEnumerable<InputT>, (Int32, OutputT)>> GetExpressionTree<InputT, OutputT> ( Transducer<InputT, OutputT> transducer )
        {
            ParameterExpression inputArg = Expression.Parameter ( typeof ( IEnumerable<InputT> ), "input" );
            ParameterExpression enumerator = Expression.Variable ( typeof ( IEnumerator<InputT> ), "enumerator" );
            LabelTarget returnLabelTarget = Expression.Label ( typeof ( (Int32, OutputT) ), "return-label" );

            var lambda = Expression.Lambda<Func<IEnumerable<InputT>, (Int32, OutputT)>> (
                Expression.Block (
                    typeof ( (Int32, OutputT) ),
                    new[] { enumerator },
                    Expression.Assign ( enumerator, GExpression.MethodCall<IEnumerable<InputT>> ( inputArg, e => e.GetEnumerator ( ) ) ),
                    Expression.TryFinally (
                        GetStateExpressionTree ( transducer.InitialState, enumerator, 0, returnLabelTarget ),
                        Expression.IfThen (
                            Expression.NotEqual ( enumerator, Expression.Constant ( null ) ),
                            GExpression.MethodCall<IDisposable> ( enumerator, d => d.Dispose ( ) ) )
                    )
                ),
                inputArg
            );
            return lambda;
        }

        private static Expression GetStateExpressionTree<InputT, OutputT> ( TransducerState<InputT, OutputT> state, ParameterExpression enumerator, Int32 depth, LabelTarget returnLabelTarget )
        {
            if ( state.TransitionTable.Count > 0 )
            {
                // Serialize all transitions
                var cases = new List<SwitchCase> ( );
                foreach ( KeyValuePair<InputT, TransducerState<InputT, OutputT>> kv in state.TransitionTable )
                {
                    cases.Add ( Expression.SwitchCase (
                        GetStateExpressionTree ( kv.Value, enumerator, depth + 1, returnLabelTarget ),
                        Expression.Constant ( kv.Key )
                    ) );
                }

                ConstantExpression retVal = state.IsTerminal
                    ? Expression.Constant ( default ( (Int32, OutputT) ) )
                    : Expression.Constant ( ( depth, state.Output ) );

                return Expression.Condition (
                    GExpression.MethodCall<IEnumerator> ( enumerator, e => e.MoveNext ( ) ),
                    Expression.Switch (
                        typeof ( (Int32, OutputT) ),
                        GExpression.PropertyGet<IEnumerator<InputT>, InputT> ( enumerator, e => e.Current ),
                        retVal,
                        null,
                        cases.ToArray ( ) ),

                    // Then serialize the output if this is a terminal state, otherwise return the error
                    // values
                    retVal
                );
            }
            else if ( state.IsTerminal )
            {
                return Expression.Constant ( (depth, state.Output) );
            }

            throw new InvalidOperationException ( "Cannot serialize a non-terminal state without any transitions." );
        }

        /// <summary>
        /// Compiles a transducer into a method for faster execution
        /// </summary>
        /// <typeparam name="InputT"></typeparam>
        /// <typeparam name="OutputT"></typeparam>
        /// <param name="transducer"></param>
        /// <returns></returns>
        public static Func<IEnumerable<InputT>, (Int32, OutputT)> Compile<InputT, OutputT> ( Transducer<InputT, OutputT> transducer )
        {
            Expression<Func<IEnumerable<InputT>, (Int32, OutputT)>> lambda = GetExpressionTree ( transducer );
            Func<IEnumerable<InputT>, (Int32, OutputT)> compiled = lambda.Compile ( );
            return compiled;
        }
    }
}
