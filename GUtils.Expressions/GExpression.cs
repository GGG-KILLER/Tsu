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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GUtils.Expressions
{
    /// <summary>
    /// Static class full of utilities for creating expression trees
    /// </summary>
    public static partial class GExpression
    {
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        private static Expression GetParameterExpression ( ParameterInfo[] @params, Int32 index, Object[] args )
        {
            if ( args.Length > index )
            {
                return GetExpression ( args[index] );
            }
            else if ( @params[index].HasDefaultValue )
            {
                return Expression.Constant ( @params[index].DefaultValue );
            }
            else
            {
                throw new InvalidOperationException ( $"Argument #{index} does not have a default value." );
            }
        }

        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        private static IEnumerable<Expression> GetParametersExpressions ( ParameterInfo[] @params, Object[] args ) =>
            Enumerable.Range ( 0, @params.Length ).Select ( n => GetParameterExpression ( @params, n, args ) );

        /// <summary>
        /// Creates an <see cref="Expression"/> from the provided <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Expression GetExpression ( Object value ) =>
            value is Expression expression ? expression : Expression.Constant ( value );

        /// <summary>
        /// Returns the operation of calling a method with the given arguments on the given instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="methodCall"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static MethodCallExpression MethodCall<T> ( Expression instance, Expression<Action<T>> methodCall, params Object[] args )
        {
            if ( methodCall is null )
                throw new ArgumentNullException ( nameof ( methodCall ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            var methodCallExpr = methodCall.Body as MethodCallExpression;
            ParameterInfo[] @params = methodCallExpr.Method.GetParameters ( );

            return methodCallExpr.Update ( instance, GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a <see cref="NewExpression"/> of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructor"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static NewExpression New<T> ( Expression<Func<T>> constructor, params Object[] args )
        {
            if ( constructor is null )
                throw new ArgumentNullException ( nameof ( constructor ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            var constructorExpr = constructor.Body as NewExpression;
            ParameterInfo[] @params = constructorExpr.Constructor.GetParameters ( );
            return constructorExpr.Update ( GetParametersExpressions ( @params, args ) );
        }

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructor"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static UnaryExpression Throw<T> ( Expression<Func<T>> constructor, params Object[] args )
        {
            if ( constructor is null )
                throw new ArgumentNullException ( nameof ( constructor ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            return Expression.Throw ( New ( constructor, args ) );
        }

        /// <summary>
        /// Returns the operation of getting a value at an index
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="key"></param>
        public static IndexExpression IndexGet ( Expression instance, Object key )
        {
            if ( instance is null )
                throw new ArgumentNullException ( nameof ( instance ) );

            return Expression.Property ( instance, "Item", GetExpression ( key ) );
        }

        /// <summary>
        /// Returns the operation of assigning a value to an index
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static BinaryExpression IndexSet ( Expression instance, Object key, Object val )
        {
            if ( instance is null )
                throw new ArgumentNullException ( nameof ( instance ) );

            return Expression.Assign ( IndexGet ( instance, key ), GetExpression ( val ) );
        }

        /// <summary>
        /// Returns the operation of retrieving a value from a field
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        public static MemberExpression FieldGet<T> ( Expression instance, String name ) =>
            Expression.Field ( instance, typeof ( T ), name );

        /// <summary>
        /// Returns the operation of assigning a value to a field
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static BinaryExpression FieldSet<T> ( Expression instance, String name, Object val ) =>
            Expression.Assign ( FieldGet<T> ( instance, name ), GetExpression ( val ) );

        /// <summary>
        /// Returns the operation of retrieving a value from a field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TField"></typeparam>
        /// <param name="instance"></param>
        /// <param name="fieldAccessor"></param>
        /// <returns></returns>
        public static MemberExpression FieldGet<T, TField> ( Expression instance, Expression<Func<T, TField>> fieldAccessor )
        {
            if ( instance is null )
                throw new ArgumentNullException ( nameof ( instance ) );

            if ( fieldAccessor is null )
                throw new ArgumentNullException ( nameof ( fieldAccessor ) );

            var fieldExpr = fieldAccessor.Body as MemberExpression;
            if ( !( fieldExpr?.Member is FieldInfo ) )
                throw new ArgumentException ( "The expression provided does not represent a field get operation.", nameof ( fieldAccessor ) );
            return fieldExpr.Update ( instance );
        }

        /// <summary>
        /// Returns the operation of assigning a value to a field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TField"></typeparam>
        /// <param name="instance"></param>
        /// <param name="fieldAccessor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression FieldSet<T, TField> ( Expression instance, Expression<Func<T, TField>> fieldAccessor, Object value ) =>
            Expression.Assign ( FieldGet ( instance, fieldAccessor ), GetExpression ( value ) );

        /// <summary>
        /// Returns the operation of retrieving a value from a property
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        public static MemberExpression PropertyGet<T> ( Expression instance, String name ) =>
            Expression.Property ( instance, typeof ( T ), name );

        /// <summary>
        /// Returns the operation of assigning a value to a property
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static BinaryExpression PropertySet<T> ( Expression instance, String name, Object val ) =>
            Expression.Assign ( PropertyGet<T> ( instance, name ), GetExpression ( val ) );

        /// <summary>
        /// Returns the operation of retrieving a value from a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="instance"></param>
        /// <param name="propertyAccessor"></param>
        /// <returns></returns>
        public static MemberExpression PropertyGet<T, TProperty> ( Expression instance, Expression<Func<T, TProperty>> propertyAccessor )
        {
            if ( instance is null )
                throw new ArgumentNullException ( nameof ( instance ) );

            if ( propertyAccessor is null )
                throw new ArgumentNullException ( nameof ( propertyAccessor ) );

            var memberExpr = propertyAccessor.Body as MemberExpression;
            if ( !( memberExpr?.Member is PropertyInfo ) )
                throw new ArgumentException ( "The expression provided does not represent a property get operation.", nameof ( propertyAccessor ) );
            return memberExpr.Update ( instance );
        }

        /// <summary>
        /// Returns the operation of assigning a value to a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="instance"></param>
        /// <param name="propertyAccessor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression PropertySet<T, TProperty> ( Expression instance, Expression<Func<T, TProperty>> propertyAccessor, Object value ) =>
            Expression.Assign ( PropertyGet ( instance, propertyAccessor ), GetExpression ( value ) );

        /// <summary>
        /// Returns the operation of calling a delegate with the given arguments
        /// </summary>
        /// <param name="delegate"></param>
        /// <param name="args"></param>
        public static MethodCallExpression Call ( Delegate @delegate, params Object[] args )
        {
            if ( @delegate is null )
                throw new ArgumentNullException ( nameof ( @delegate ) );

            if ( args is null )
                throw new ArgumentNullException ( nameof ( args ) );

            ParameterInfo[] @params = @delegate.Method.GetParameters ( );
            return Expression.Call ( @delegate.Target != null ? Expression.Constant ( @delegate.Target ) : null, @delegate.Method, GetParametersExpressions ( @params, args ) );
        }
    }
}