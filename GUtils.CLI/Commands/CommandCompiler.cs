/*
 * Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the “Software”), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
 * the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Linq.Expressions;
using System.Reflection;
using GUtils.CLI.Commands.Errors;

namespace GUtils.CLI.Commands
{
    internal static class CommandCompiler
    {
        private static MethodCallExpression GetEnumConvertExpression ( Type type, Expression arg )
        {
            // Only method available to enums is the Enum.Parse
            // method, so use that
            return Expression.Call ( null, typeof ( Enum ).GetMethod ( "Parse", new[] {
                typeof ( Type ),
                typeof ( String ),
                typeof ( Boolean )
            } ), Expression.Constant ( type ), arg, Expression.Constant ( true ) );
        }

        private static MethodCallExpression GetConvertExpression ( Type type, Expression arg )
        {
            MethodInfo parseM = type.GetMethod ( "Parse", new[] { typeof ( String ) } );
            MethodInfo changeTypeM = typeof ( Convert ).GetMethod ( "ChangeType", new[] {
                typeof ( Object ),
                typeof ( Type )
            } );
            // Use .Parse static method if it exists, otherwise
            // use the Convert.ChangeType method
            return parseM != null
                ? Expression.Call ( null, parseM, arg )
                : Expression.Call ( null, changeTypeM, arg, Expression.Constant ( type ) );
        }

        private static Expression GetThrowExpression<T> ( Type retType, params Object[] unformattedArgs )
            where T : Exception
        {
            var formattedArgs = new Expression[unformattedArgs.Length];
            var types = new Type[unformattedArgs.Length];

            // Convert the args to Expressions and get their types
            for ( var i = 0; i < unformattedArgs.Length; i++ )
            {
                if ( unformattedArgs[i] is Expression expr )
                {
                    formattedArgs[i] = expr;
                    types[i] = expr.Type;
                }
                else
                {
                    formattedArgs[i] = Expression.Constant ( unformattedArgs[i] );
                    types[i] = unformattedArgs[i].GetType ( );
                }
            }

            // Create a new instance of the exception type using
            // the constructor that accepts these types
            NewExpression @new = Expression.New ( typeof ( T ).GetConstructor ( types ), formattedArgs );

            // throw the exception that was created above
            UnaryExpression @throw = Expression.Throw ( @new, retType );

            // And then use a hack to make the expression tree
            // happy about the return type of this (even though
            // it'll never return ¯\_(ツ)_/¯)
            return Expression.Convert ( @throw, retType );
        }

        /// <summary>
        /// Transforms the conversion process into expression trees
        /// </summary>
        /// <param name="method"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Expression<Action<String, String[]>> CompilePartially ( MethodInfo method, Object instance )
        {
            ParameterExpression name = Expression.Parameter ( typeof ( String ) );
            ParameterExpression arguments = Expression.Parameter ( typeof ( String[] ) );
            UnaryExpression argumentsLength = Expression.ArrayLength ( arguments );

            ParameterInfo[] parameters = method.GetParameters ( );
            var hasParamsArgument = false;
            var convertedArguments = new Expression[parameters.Length];

            // Create expressions converting all arguments to the
            // expected types given by the function arguments
            for ( var idx = 0; idx < parameters.Length; idx++ )
            {
                ParameterInfo param = parameters[idx];
                ConstantExpression idxExpression = Expression.Constant ( idx );
                Expression argument = Expression.ArrayIndex ( arguments, idxExpression );
                Type parameterType = param.ParameterType;

                // Use the appropriate conversion method depending
                // on the argument type
                #region [JoinRestOfArguments]

                if ( param.IsDefined ( typeof ( JoinRestOfArgumentsAttribute ), true ) )
                {
                    argument = Expression.Call ( null, typeof ( String ).GetMethod ( "Join", new[]
                      {
                        typeof ( String ),
                        typeof ( String[] ),
                        typeof ( Int32 ),
                        typeof ( Int32 )
                    } ), Expression.Constant ( "" ), arguments, idxExpression, Expression.Subtract ( argumentsLength, idxExpression ) );
                }
                #endregion [JoinRestOfArguments]
                #region params

                else if ( param.IsDefined ( typeof ( ParamArrayAttribute ), true ) )
                {
                    ParameterExpression section = Expression.Variable ( typeof ( String[] ), "section" );
                    BinaryExpression sectionLength = Expression.Subtract ( argumentsLength, idxExpression );

                    hasParamsArgument = true;
                    argument = Expression.Block (
                        typeof ( String[] ),
                        /* String[] arr; */
                        new[] { section },
                        /* arr = new String[args.Length - <i>]; */
                        Expression.Assign (
                            section,
                            Expression.NewArrayBounds ( typeof ( String ), sectionLength ) ),
                        /* Array.CopyTo ( args, arr, args.Length - <i> ); */
                        Expression.Call ( null, typeof ( Array ).GetMethod ( "Copy", new[]
                        {
                            typeof ( Array ),
                            typeof ( Array ),
                            typeof ( Int32 )
                        } ), arguments, section, sectionLength ),
                        /* return arr; (?) */
                        section
                    );
                }

                #endregion params
                #region Enum arg

                else if ( parameterType.IsEnum )
                    argument = GetEnumConvertExpression ( parameterType, argument );

                #endregion Enum arg
                #region Non-String arg

                else if ( parameterType != typeof ( String ) )
                    argument = GetConvertExpression ( parameterType, argument );

                #endregion Non-String arg

                // Handle exceptions when converting
                ParameterExpression ex = Expression.Variable ( typeof ( Exception ), "ex" );
                argument = Expression.TryCatch (
                /* try
                 *     <Convert.ChangeType|Enum.Parse>(arg, <type>) as <type>);
                 */
                    Expression.Convert ( argument, parameterType ),
                /* catch ( Exception ex )
                 *     throw new CommandInvocationException ( <command>, <message>, ex );
                 */
                    Expression.Catch ( ex,
                        GetThrowExpression<CommandInvocationException> ( parameterType, name,
                                $"Invalid argument #{idx}.", ex ) )
                );

                // Add check that there're enough arguments
                // otherwise attempt to use default values, and if
                // this argument doesn't has one, throw an exception
                convertedArguments[idx] = Expression.Condition (
                    Expression.GreaterThan ( Expression.ArrayLength ( arguments ), idxExpression ),
                    argument,
                    parameters[idx].HasDefaultValue
                    ? Expression.Convert ( Expression.Constant ( parameters[idx].DefaultValue ), parameterType )
                    : ( hasParamsArgument
                        // Params can have no arguments at all and
                        // will call the function with no arguments
                        ? Expression.Constant ( Array.Empty<String> ( ) )
                        : GetThrowExpression<CommandInvocationException> ( parameterType, name,
                            $"Missing argument #{idx}." ) )
                );
            }

            // Return the lambda expression with the method call
            // using the converted arguments
            return Expression.Lambda<Action<String, String[]>> ( Expression.Call ( instance != null && !method.IsStatic
                ? Expression.Constant ( instance )
                : null,
                method, convertedArguments ), name, arguments );
        }

        /// <summary>
        /// Compiles a command argument conversion process so that
        /// no extra logic is made when executing them
        /// </summary>
        /// <param name="method"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Action<String, String[]> Compile ( MethodInfo method, Object instance )
        {
            return CompilePartially ( method, instance ).Compile ( );
        }
    }
}