﻿// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
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
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Tsu.Expressions;

namespace Tsu.StateMachines.Transducers
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
        public static Expression<Func<IEnumerable<TInput>, (int, TOutput)>> GetExpressionTree<TInput, TOutput>(Transducer<TInput, TOutput> transducer)
            where TInput : notnull
        {
            if (transducer is null)
                throw new ArgumentNullException(nameof(transducer));

            var inputArg = Expression.Parameter(typeof(IEnumerable<TInput>), "input");
            var enumerator = Expression.Variable(typeof(IEnumerator<TInput>), "enumerator");
            var returnLabelTarget = Expression.Label(typeof((int, TOutput)), "return-label");

            var lambda = Expression.Lambda<Func<IEnumerable<TInput>, (int, TOutput)>>(
                Expression.Block(
                    typeof((int, TOutput)),
                    new[] { enumerator },
                    Expression.Assign(enumerator, GExpression.MethodCall<IEnumerable<TInput>>(inputArg, e => e.GetEnumerator())),
                    Expression.TryFinally(
                        GetStateExpressionTree(transducer.InitialState, enumerator, 0, returnLabelTarget),
                        Expression.IfThen(
                            Expression.NotEqual(enumerator, Expression.Constant(null)),
                            GExpression.MethodCall<IDisposable>(enumerator, d => d.Dispose()))
                    )
                ),
                inputArg
            );
            return lambda;
        }

        private static Expression GetStateExpressionTree<TInput, TOutput>(TransducerState<TInput, TOutput> state, ParameterExpression enumerator, int depth, LabelTarget returnLabelTarget)
            where TInput : notnull
        {
            if (state is null)
                throw new ArgumentNullException(nameof(state));

            if (enumerator is null)
                throw new ArgumentNullException(nameof(enumerator));

            if (returnLabelTarget is null)
                throw new ArgumentNullException(nameof(returnLabelTarget));

            if (state.TransitionTable.Count > 0)
            {
                // Serialize all transitions
                var cases = new List<SwitchCase>();
                foreach (var kv in state.TransitionTable)
                {
                    cases.Add(Expression.SwitchCase(
                        GetStateExpressionTree(kv.Value, enumerator, depth + 1, returnLabelTarget),
                        Expression.Constant(kv.Key)
                    ));
                }

                var retVal = state.IsTerminal
                    ? Expression.Constant(default((int, TOutput)))
                    : Expression.Constant((depth, state.Output));

                return Expression.Condition(
                    GExpression.MethodCall<IEnumerator>(enumerator, e => e.MoveNext()),
                    Expression.Switch(
                        typeof((int, TOutput)),
                        GExpression.PropertyGet<IEnumerator<TInput>, TInput>(enumerator, e => e.Current),
                        retVal,
                        null,
                        cases.ToArray()),

                    // Then serialize the output if this is a terminal state, otherwise return the error
                    // values
                    retVal
                );
            }
            else if (state.IsTerminal)
            {
                return Expression.Constant((depth, state.Output));
            }

            throw new InvalidOperationException("Cannot serialize a non-terminal state without any transitions.");
        }

        /// <summary>
        /// Compiles a transducer into a method for faster execution
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="transducer"></param>
        /// <returns></returns>
        public static Func<IEnumerable<TInput>, (int, TOutput)> Compile<TInput, TOutput>(Transducer<TInput, TOutput> transducer)
            where TInput : notnull
        {
            if (transducer is null)
                throw new ArgumentNullException(nameof(transducer));

            var lambda = GetExpressionTree(transducer);
            var compiled = lambda.Compile();
            return compiled;
        }
    }
}
