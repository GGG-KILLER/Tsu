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
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Tsu.CLI.Commands.Errors;

// Disable obsolete warnings since we need backwards compatibility
#pragma warning disable CS0618

namespace Tsu.CLI.Commands
{
    internal static class CommandCompiler
    {
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Static readonly field.")]
        private static readonly MethodInfo s_mi_Enum_Parse = typeof(Enum).GetMethod("Parse", new[] {
            typeof(Type),
            typeof(string),
            typeof(bool)
        });

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Static readonly field.")]
        private static readonly MethodInfo s_mi_Convert_ChangeType = typeof(Convert).GetMethod("ChangeType", new[] {
            typeof(object),
            typeof(Type)
        });

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Static readonly field.")]
        private static readonly MethodInfo s_mi_String_Join = typeof(string).GetMethod("Join", new[]
        {
            typeof(string),
            typeof(string[]),
            typeof(int),
            typeof(int)
        });

        private static Expression GetConvertExpression(Type type, Expression arg)
        {
            if (Nullable.GetUnderlyingType(type) is Type underlyingType)
            {
                var expr = GetConvertExpression(underlyingType, arg);
                return Expression.Convert(expr, type);
            }

            // Use .Parse static method if it exists, otherwise use the Convert.ChangeType method
            var parseMethod = type.GetMethod("Parse", new[] { typeof(string) });
            return Expression.Convert(type.IsEnum
                ? Expression.Call(null, s_mi_Enum_Parse, Expression.Constant(type), arg, Expression.Constant(true))
                : parseMethod != null && parseMethod.IsStatic
                    ? Expression.Call(null, parseMethod, arg)
                    : Expression.Call(null, s_mi_Convert_ChangeType, arg, Expression.Constant(type)), type);
        }

        private static Expression GetThrowExpression<T>(Type retType, params object[] unformattedArgs)
            where T : Exception
        {
            var formattedArgs = new Expression[unformattedArgs.Length];
            var types = new Type[unformattedArgs.Length];

            // Convert the args to Expressions and get their types
            for (var i = 0; i < unformattedArgs.Length; i++)
            {
                if (unformattedArgs[i] is Expression expr)
                {
                    formattedArgs[i] = expr;
                    types[i] = expr.Type;
                }
                else
                {
                    formattedArgs[i] = Expression.Constant(unformattedArgs[i]);
                    types[i] = unformattedArgs[i].GetType();
                }
            }

            // Create a new instance of the exception type using the constructor that accepts these types
            var @new = Expression.New(typeof(T).GetConstructor(types), formattedArgs);

            // throw the exception that was created above
            var @throw = Expression.Throw(@new, retType);

            // And then use a hack to make the expression tree happy about the return type of this
            // (even though it'll never return ¯\_(ツ)_/¯)
            return Expression.Convert(@throw, retType);
        }

        private static Expression CompileParamsParameter(ParameterExpression arguments, UnaryExpression argumentsLength, ParameterInfo param, ConstantExpression idxExpression)
        {
            var elementType = param.ParameterType.GetElementType();
            var converted = Expression.Variable(param.ParameterType, "converted");
            var offset = Expression.Variable(typeof(int), "offset");
            var sectionLength = Expression.Subtract(argumentsLength, idxExpression);
            var breakLabel = Expression.Label("break");

            return Expression.Block(
                param.ParameterType,
                /* String[] converted; Int32 offset; */
                new[] { converted, offset },
                /* converted = new String[args.Length - <i>]; */
                Expression.Assign(converted, Expression.NewArrayBounds(elementType, sectionLength)),
                /* offset = 0; */
                Expression.Assign(offset, Expression.Constant(0)),
                /* loop { */
                Expression.Loop(Expression.Block(
                    /* if ( offset >= args.Length - <i> ) break; */
                    Expression.IfThen(Expression.GreaterThanOrEqual(offset, sectionLength), Expression.Break(breakLabel)),
                    /* converted[offset] = <ConvertExpression ( <arrayType>, arguments[<idxExpression> + offset] )>; */
                    Expression.Assign(Expression.ArrayAccess(converted, offset), GetConvertExpression(elementType, Expression.ArrayAccess(arguments, Expression.Add(idxExpression, offset)))),
                    /* offset++; */
                    Expression.Assign(offset, Expression.Increment(offset))
                ), breakLabel),
                /* } */
                /* return converted; */
                converted
            );
        }

        /// <summary>
        /// Transforms the conversion process into expression trees
        /// </summary>
        /// <param name="method"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Expression<Action<string, string[]>> CompilePartially(MethodInfo method, object instance)
        {
            var name = Expression.Parameter(typeof(string), "name");
            var arguments = Expression.Parameter(typeof(string[]), "args");
            var argumentsLength = Expression.ArrayLength(arguments);

            var parameters = method.GetParameters();
            var convertedArguments = new Expression[parameters.Length];

            // Create expressions converting all arguments to the expected types given by the
            // function arguments
            for (var idx = 0; idx < parameters.Length; idx++)
            {
                var hasParamsArgument = false;
                var param = parameters[idx];
                var idxExpression = Expression.Constant(idx);
                Expression argument = Expression.ArrayIndex(arguments, idxExpression);
                var parameterType = param.ParameterType;

                // Use the appropriate conversion method depending on the argument type
                /* [JoinRestOfArguments] */
                if (param.IsDefined(typeof(JoinRestOfArgumentsAttribute), true))
                {
                    argument = Expression.Call(null, s_mi_String_Join, Expression.Constant(""), arguments, idxExpression, Expression.Subtract(argumentsLength, idxExpression));
                }
                /* params */
                else if (param.IsDefined(typeof(ParamArrayAttribute), true))
                {
                    hasParamsArgument = true;
                    argument = CompileParamsParameter(arguments, argumentsLength, param, idxExpression);
                }
                /* Non-String arg */
                else if (parameterType != typeof(string))
                {
                    argument = GetConvertExpression(parameterType, argument);
                }

                // Handle exceptions when converting
                var ex = Expression.Variable(typeof(Exception), "ex");
                argument = Expression.TryCatch(
                /* try
                 *     <Convert.ChangeType|Enum.Parse>(arg, <type>) as <type>);
                 */
                    Expression.Convert(argument, parameterType),
                /* catch ( Exception ex )
                 *     throw new CommandInvocationException ( <command>, <message>, ex );
                 */
                    Expression.Catch(ex, GetThrowExpression<CommandInvocationException>(parameterType, name, $"Invalid argument #{idx}.", ex))
                );

                // Add check that there're enough arguments otherwise attempt to use default values,
                // and if this argument doesn't has one, throw an exception
                convertedArguments[idx] = Expression.Condition(
                    Expression.GreaterThan(Expression.ArrayLength(arguments), idxExpression),
                    argument,
                    parameters[idx].HasDefaultValue
                    ? Expression.Convert(Expression.Constant(parameters[idx].DefaultValue), parameterType)
                    : (hasParamsArgument
                        /* Params can have no arguments at all and will call the function with no arguments */
                        ? Expression.NewArrayBounds(parameterType.GetElementType(), Expression.Constant(0))
                        : GetThrowExpression<CommandInvocationException>(parameterType, name, $"Missing argument #{idx}."))
                );
            }

            // Return the lambda expression with the method call using the converted arguments
            return Expression.Lambda<Action<string, string[]>>(Expression.Call(instance != null && !method.IsStatic
                ? Expression.Constant(instance)
                : null,
                method, convertedArguments), name, arguments);
        }

        /// <summary>
        /// Compiles a command argument conversion process so that no extra logic is made when
        /// executing them
        /// </summary>
        /// <param name="method"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Action<string, string[]> Compile(MethodInfo method, object instance) =>
            CompilePartially(method, instance).Compile();
    }
}