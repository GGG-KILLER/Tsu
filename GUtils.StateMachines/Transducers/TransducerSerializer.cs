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
        /// <typeparam name="TInput">The type of input accepted by the transducer</typeparam>
        /// <typeparam name="TOutput">The type of output emitted by the transducer</typeparam>
        /// <param name="transducer">The transducer to compile</param>
        /// <returns></returns>
        public static Expression<Func<IEnumerable<TInput>, (Int32, TOutput)>> GetExpressionTree<TInput, TOutput> ( Transducer<TInput, TOutput> transducer )
            where TInput : notnull
        {
            if ( transducer is null )
                throw new ArgumentNullException ( nameof ( transducer ) );

            ParameterExpression inputArg = Expression.Parameter ( typeof ( IEnumerable<TInput> ), "input" );
            ParameterExpression enumerator = Expression.Variable ( typeof ( IEnumerator<TInput> ), "enumerator" );
            LabelTarget returnLabelTarget = Expression.Label ( typeof ( (Int32, TOutput) ), "return-label" );

            var lambda = Expression.Lambda<Func<IEnumerable<TInput>, (Int32, TOutput)>> (
                Expression.Block (
                    typeof ( (Int32, TOutput) ),
                    new[] { enumerator },
                    Expression.Assign ( enumerator, GExpression.MethodCall<IEnumerable<TInput>> ( inputArg, e => e.GetEnumerator ( ) ) ),
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

        private static Expression GetStateExpressionTree<TInput, TOutput> ( TransducerState<TInput, TOutput> state, ParameterExpression enumerator, Int32 depth, LabelTarget returnLabelTarget )
            where TInput : notnull
        {
            if ( state is null )
                throw new ArgumentNullException ( nameof ( state ) );
            
            if ( enumerator is null )
                throw new ArgumentNullException ( nameof ( enumerator ) );
            
            if ( returnLabelTarget is null )
                throw new ArgumentNullException ( nameof ( returnLabelTarget ) );

            if ( state.TransitionTable.Count > 0 )
            {
                // Serialize all transitions
                var cases = new List<SwitchCase> ( );
                foreach ( KeyValuePair<TInput, TransducerState<TInput, TOutput>> kv in state.TransitionTable )
                {
                    cases.Add ( Expression.SwitchCase (
                        GetStateExpressionTree ( kv.Value, enumerator, depth + 1, returnLabelTarget ),
                        Expression.Constant ( kv.Key )
                    ) );
                }

                ConstantExpression retVal = state.IsTerminal
                    ? Expression.Constant ( default ( (Int32, TOutput) ) )
                    : Expression.Constant ( ( depth, state.Output ) );

                return Expression.Condition (
                    GExpression.MethodCall<IEnumerator> ( enumerator, e => e.MoveNext ( ) ),
                    Expression.Switch (
                        typeof ( (Int32, TOutput) ),
                        GExpression.PropertyGet<IEnumerator<TInput>, TInput> ( enumerator, e => e.Current ),
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
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="transducer"></param>
        /// <returns></returns>
        public static Func<IEnumerable<TInput>, (Int32, TOutput)> Compile<TInput, TOutput> ( Transducer<TInput, TOutput> transducer )
            where TInput : notnull
        {
            if ( transducer is null )
                throw new ArgumentNullException ( nameof ( transducer ) );

            Expression<Func<IEnumerable<TInput>, (Int32, TOutput)>> lambda = GetExpressionTree ( transducer );
            Func<IEnumerable<TInput>, (Int32, TOutput)> compiled = lambda.Compile ( );
            return compiled;
        }
    }
}
