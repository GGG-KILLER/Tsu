/*
 * Copyright © 2019 GGG KILLER <gggkiller2@gmail.com>
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
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace GUtils.Expressions
{
    /// <summary>
    ///	Static class full of utilities for creating expression trees
    /// </summary>
    public static partial class GExpression
    {
        #region MethodCall (Generated Code)

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        public static MethodCallExpression MethodCall ( Type type, Expression instance, String name )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The name of the method cannot be null, empty or whitespaces.", nameof ( name ) );

            return Expression.Call ( instance, type.GetMethod ( name ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <typeparam name="TInstance"></typeparam>
        public static MethodCallExpression MethodCall<TInstance> ( Expression instance, String name ) =>
            MethodCall ( typeof ( TInstance ), instance, name );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4, T5> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4, T5> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4, T5> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4, T5, T6> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4, T5, T6> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4, T5, T6> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4, T5, T6, T7> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4, T5, T6, T7> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4, T5, T6, T7> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4, T5, T6, T7, T8> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4, T5, T6, T7, T8> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4, T5, T6, T7, T8> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
                typeof ( T11 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
                typeof ( T11 ),
                typeof ( T12 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
                typeof ( T11 ),
                typeof ( T12 ),
                typeof ( T13 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
                typeof ( T11 ),
                typeof ( T12 ),
                typeof ( T13 ),
                typeof ( T14 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
                typeof ( T11 ),
                typeof ( T12 ),
                typeof ( T13 ),
                typeof ( T14 ),
                typeof ( T15 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ( typeof ( TInstance ), instance, name, args );

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ( Type type, Expression instance, String name, params Object[] args )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            if ( String.IsNullOrWhiteSpace ( name ) )
                throw new ArgumentException ( "The method name cannot be null, empty or whitespaces.", nameof ( name ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            MethodInfo method = type.GetMethod ( name, new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
                typeof ( T11 ),
                typeof ( T12 ),
                typeof ( T13 ),
                typeof ( T14 ),
                typeof ( T15 ),
                typeof ( T16 ),
            } );
            ParameterInfo[] @params = method.GetParameters ( );

            return Expression.Call ( instance, method, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Creates a method call expression
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public static MethodCallExpression MethodCall<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ( Expression instance, String name, params Expression[] args ) =>
            MethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ( typeof ( TInstance ), instance, name, args );

        #endregion MethodCall (Generated Code)

        #region New (Generated Code)

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        public static NewExpression New<TInstance> ( ) =>
            Expression.New ( typeof ( TInstance ) );


        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4, T5> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4, T5, T6> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4, T5, T6, T7> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
                typeof ( T11 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
                typeof ( T11 ),
                typeof ( T12 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
                typeof ( T11 ),
                typeof ( T12 ),
                typeof ( T13 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
                typeof ( T11 ),
                typeof ( T12 ),
                typeof ( T13 ),
                typeof ( T14 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
                typeof ( T11 ),
                typeof ( T12 ),
                typeof ( T13 ),
                typeof ( T14 ),
                typeof ( T15 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static NewExpression New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ( params Object[] args )
        {
            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ConstructorInfo constructor = typeof ( TInstance ).GetConstructor ( new[]
            {
                typeof ( T1 ),
                typeof ( T2 ),
                typeof ( T3 ),
                typeof ( T4 ),
                typeof ( T5 ),
                typeof ( T6 ),
                typeof ( T7 ),
                typeof ( T8 ),
                typeof ( T9 ),
                typeof ( T10 ),
                typeof ( T11 ),
                typeof ( T12 ),
                typeof ( T13 ),
                typeof ( T14 ),
                typeof ( T15 ),
                typeof ( T16 ),
            } );
            var @params = constructor.GetParameters ( );

            return Expression.New ( constructor, GetParametersExpressions ( @params, args ) );
        }

        #endregion New (Generated Code)

        #region Throw (Generated Code)

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        public static UnaryExpression Throw<TInstance> ( ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance> ( ), typeof ( TInstance ) );


        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4, T5> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4, T5> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4, T5, T6> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4, T5, T6> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4, T5, T6, T7> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4, T5, T6, T7> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4, T5, T6, T7, T8> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ( args ), typeof ( TInstance ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="TInstance" />
        /// </summary>
        /// <param name="args"></param>
        public static UnaryExpression Throw<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ( params Object[] args ) where TInstance : Exception =>
            Expression.Throw ( New<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ( args ), typeof ( TInstance ) );

        #endregion Throw (Generated Code)
    }
}
