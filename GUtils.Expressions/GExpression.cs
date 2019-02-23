using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GUtils.Expressions
{
    /// <summary>
    ///	Static class full of utilities for creating expression trees
    /// </summary>
    public static partial class GExpression
    {
        /// <summary>
        /// Returns the operation of calling a method with the given arguments on the given instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inst"></param>
        /// <param name="methodCall"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static MethodCallExpression MethodCall<T> ( Expression inst, Expression<Action<T>> methodCall, params Object[] args ) =>
            ( methodCall.Body as MethodCallExpression )
                .Update ( inst, Array.ConvertAll ( args, arg => arg is Expression expr ? expr : Expression.Constant ( arg ) ) );

        /// <summary>
        /// Returns a <see cref="NewExpression" /> of <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructor"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static NewExpression New<T> ( Expression<Func<T>> constructor, params Object[] args ) =>
            ( constructor.Body as NewExpression )
                .Update ( Array.ConvertAll ( args, arg => arg is Expression expr ? expr : Expression.Constant ( arg ) ) );

        /// <summary>
        /// Returns a throw expression of type <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructor"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static UnaryExpression Throw<T> ( Expression<Func<T>> constructor, params Object[] args ) =>
            Expression.Throw ( New ( constructor, args ) );

        /// <summary>
        /// Returns the operation of getting a value at an index
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="key"></param>
        public static IndexExpression IndexGet ( Expression inst, Expression key ) =>
            Expression.Property ( inst, "Item", key );

        /// <summary>
        /// Returns the operation of assigning a value to an index
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static BinaryExpression IndexSet ( Expression inst, Expression key, Expression val ) =>
            Expression.Assign ( IndexGet ( inst, key ), val );

        /// <summary>
        /// Returns the operation of retrieving a value from a field
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="name"></param>
        public static MemberExpression FieldGet<T> ( Expression inst, String name ) =>
            Expression.Field ( inst, typeof ( T ), name );

        /// <summary>
        /// Returns the operation of retrieving a value from a field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TField"></typeparam>
        /// <param name="inst"></param>
        /// <param name="fieldAccessor"></param>
        /// <returns></returns>
        public static MemberExpression FieldGet<T, TField> ( Expression inst, Expression<Func<T, TField>> fieldAccessor )
        {
            var fieldExpr = fieldAccessor as MemberExpression;
            if ( !( fieldExpr?.Member is FieldInfo ) )
                throw new ArgumentException ( "The expression provided does not represent a field get operation.", nameof ( fieldAccessor ) );
            return fieldExpr.Update ( inst );
        }

        /// <summary>
        /// Returns the operation of assigning a value to a field
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static BinaryExpression FieldSet<T> ( Expression inst, String name, Expression val ) =>
            Expression.Assign ( FieldGet<T> ( inst, name ), val );

        /// <summary>
        /// Returns the operation of assigning a value to a field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TField"></typeparam>
        /// <param name="inst"></param>
        /// <param name="fieldAccessor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression FieldSet<T, TField> ( Expression inst, Expression<Func<T, TField>> fieldAccessor, Expression value ) =>
            Expression.Assign ( FieldGet ( inst, fieldAccessor ), value );

        /// <summary>
        /// Returns the operation of retrieving a value from a property
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="name"></param>
        public static MemberExpression PropertyGet<T> ( Expression inst, String name ) =>
            Expression.Property ( inst, typeof ( T ), name );

        /// <summary>
        /// Returns the operation of retrieving a value from a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="inst"></param>
        /// <param name="propertyAccessor"></param>
        /// <returns></returns>
        public static MemberExpression PropertyGet<T, TProperty> ( Expression inst, Expression<Func<T, TProperty>> propertyAccessor )
        {
            var memberExpr = propertyAccessor.Body as MemberExpression;
            if ( !( memberExpr?.Member is PropertyInfo ) )
                throw new ArgumentException ( "The expression provided does not represent a property get operation.", nameof ( propertyAccessor ) );
            return memberExpr.Update ( inst );
        }

        /// <summary>
        /// Returns the operation of assigning a value to a property
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static BinaryExpression PropertySet<T> ( Expression inst, String name, Expression val ) =>
            Expression.Assign ( PropertyGet<T> ( inst, name ), val );

        /// <summary>
        /// Returns the operation of assigning a value to a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="inst"></param>
        /// <param name="propertyAccessor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression PropertySet<T, TProperty> ( Expression inst, Expression<Func<T, TProperty>> propertyAccessor, Expression value ) =>
            Expression.Assign ( PropertyGet ( inst, propertyAccessor ), value );

        /// <summary>
        /// Returns the operation of calling a delegate with the given arguments
        /// </summary>
        /// <param name="delegate"></param>
        /// <param name="args"></param>
        public static MethodCallExpression Call ( Delegate @delegate, params Expression[] args ) =>
            Expression.Call ( @delegate.Target != null ? Expression.Constant ( @delegate.Target ) : null, @delegate.Method, args );

        /// <summary>
        /// Returns the operation of calling a lambda with the given arguments
        /// </summary>
        /// <param name="lambda"></param>
        /// <param name="args"></param>
        public static MethodCallExpression Call ( LambdaExpression lambda, params Expression[] args ) =>
            Expression.Call ( lambda.Compile ( ).Method, args );
    }
}
