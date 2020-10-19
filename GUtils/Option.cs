using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable enable

namespace GUtils
{
    /// <summary>
    /// The class that contains the constructors for an Option.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Naming", "CA1716:Identifiers should not match keywords", Justification = "It is the name of the type in rust." )]
    public static class Option
    {
        /// <summary>
        /// Constructs a Some option
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Option<T> Some<T> ( T value ) =>
            new Option<T> ( value );

        /// <summary>
        /// Constructs a None option
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Option<T> None<T> ( ) =>
            new Option<T> ( );
    }

    /// <summary>
    /// The Option type. It represents an optional value. Create one with <see cref="Option.None{T}"
    /// /> or <see cref="Option.Some{T}(T)" />.
    /// </summary>
    /// <typeparam name="T">The type this option wraps.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Naming", "CA1716:Identifiers should not match keywords", Justification = "It is the name of the type in rust." )]
    [DebuggerDisplay ( "{" + nameof ( GetDebuggerDisplay ) + "(),nq}" )]
    public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
    {
        /// <summary>
        /// The value stored by this option
        /// </summary>
        private readonly T _some;

        /// <summary>
        /// Whether the option contains a value.
        /// </summary>
        public Boolean IsSome { get; }

        /// <summary>
        /// Whether the option doesn't contain a value.
        /// </summary>
        public Boolean IsNone => !this.IsSome;

        /// <summary>
        /// The value of the option
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the option doesn't have a value.
        /// </exception>
        public T Value
        {
            get
            {
                if ( this.IsNone )
                {
                    throw new InvalidOperationException ( "Can't get the value of an option that doesn't have one." );
                }

                return this._some;
            }
        }

        /// <summary>
        /// Initializes an Option with a value
        /// </summary>
        /// <param name="value"></param>
        internal Option ( T value )
        {
            this.IsSome = true;
            this._some = value;
        }

        /// <summary>
        /// Returns the wrapped value or the provided <paramref name="fallback" />.
        /// </summary>
        /// <param name="fallback">The value to be returned if this option doesn't contain a value.</param>
        /// <returns>The wrapped value or the <paramref name="fallback" />.</returns>
        public T UnwrapOr ( T fallback ) =>
            this.IsSome ? this._some : fallback;

        /// <summary>
        /// Returns the wrapped value or the result of the provided delegate.
        /// </summary>
        /// <param name="func">The delegate to execute in case the this is a None.</param>
        /// <returns>The wrapped value or the result of invoking <paramref name="func" />.</returns>
        public T UnwrapOrElse ( Func<T> func )
        {
            if ( func is null )
                throw new ArgumentNullException ( nameof ( func ) );

            return this.IsSome ? this._some : func ( );
        }

        /// <summary>
        /// Converts an Option of one type to another by using the provided transformation function
        /// ( <paramref name="func" />). If the current object doesn't have a value, a None for the
        /// result type is returned instead.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public Option<TResult> Map<TResult> ( Func<T, TResult> func )
        {
            if ( func is null )
                throw new ArgumentNullException ( nameof ( func ) );

            return this.IsSome ? Option.Some ( func ( this._some ) ) : Option.None<TResult> ( );
        }

        /// <summary>
        /// Converts an Option of one type to another by using the provided transformation function
        /// ( <paramref name="func" />). If the current object doesn't have a value, a Some with the
        /// <paramref name="fallback" /> value is returned instead.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="fallback"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public TResult MapOr<TResult> ( TResult fallback, Func<T, TResult> func )
        {
            if ( func is null )
                throw new ArgumentNullException ( nameof ( func ) );

            return this.IsSome ? func ( this._some ) : fallback;
        }

        /// <summary>
        /// Converts an Option of one type to another by using the provided transformation function
        /// ( <paramref name="func" />). If the current object doesn't have a value, a Some with the
        /// result of invoking the <paramref name="fallback" /> function is returned instead.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="fallback"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public TResult MapOrElse<TResult> ( Func<TResult> fallback, Func<T, TResult> func )
        {
            if ( fallback is null )
                throw new ArgumentNullException ( nameof ( fallback ) );
            if ( func is null )
                throw new ArgumentNullException ( nameof ( func ) );

            return this.IsSome ? func ( this._some ) : fallback ( );
        }

        /// <summary>
        /// Returns none if this option is none otherwise returns <paramref name="other" />
        /// </summary>
        /// <typeparam name="TOther">The wrapped type of the other option.</typeparam>
        /// <param name="other"></param>
        /// <returns></returns>
        public Option<TOther> And<TOther> ( Option<TOther> other ) =>
            this.IsSome ? other : Option.None<TOther> ( );

        /// <summary>
        /// Returns None if this Option is None, otherwise returns the result of invoking the
        /// provided function with the value.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public Option<TResult> AndThen<TResult> ( Func<T, Option<TResult>> func )
        {
            if ( func is null )
                throw new ArgumentNullException ( nameof ( func ) );

            return this.IsSome ? func ( this._some ) : Option.None<TResult> ( );
        }

        /// <summary>
        /// Returns None if this option is None or the predicate fails. If it passes then it returns
        /// this option.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Option<T> Filter ( Func<T, Boolean> filter )
        {
            if ( filter is null )
                throw new ArgumentNullException ( nameof ( filter ) );

            return this.IsSome && filter ( this._some ) ? this : Option.None<T> ( );
        }

        /// <summary>
        /// Returns this option if it has a value otherwise it returns <paramref name="other" />.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Option<T> Or ( Option<T> other ) =>
            this.IsSome ? this : other;

        /// <summary>
        /// Returns this option if it has a value otherwise returns the result of invoking the
        /// provided <paramref name="func" />.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Option<T> OrElse ( Func<Option<T>> func )
        {
            if ( func is null )
                throw new ArgumentNullException ( nameof ( func ) );

            return this.IsSome ? this : func ( );
        }

        /// <summary>
        /// Return some if either this option or <paramref name="other" /> is Some, otherwise
        /// returns None.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Option<T> Xor ( Option<T> other )
        {
            if ( this.IsSome && !other.IsSome )
                return this;
            else if ( !this.IsSome && other.IsSome )
                return other;
            else
                return Option.None<T> ( );
        }

        #region Object

        /// <inheritdoc />
        public override Boolean Equals ( Object? obj ) =>
            ( obj is Option<T> option && this.Equals ( option ) )
            // `default(T) is null` is used here to check if T is a reference type.
            // `typeof(T).IsClass` cannot be turned into a constant by the JIT: https://sharplab.io/#v2:EYLgHgbALANALiAhgZwLYB8ACAGABJgRgDoAlAVwDs4BLVAUyIGEB7VAB2oBs6AnAZV4A3agGM6yANwBYAFCzMAZnwAmXI1wBvWbh24A2gFk6cABbMAJgEl2nABRHTF6204B5NjWYVkRAIIBzfx5xZGpBOksKTmoKGP8ASgBdbV1FXGBmZk5cAwIAHgAVAD5beM0U3VSAdlxzOgAzRDJOOFsCsupkXApmzmkZSoBfCtwRw2MzKxt7Cacbd09vP0Dg5FDwyOjYigTkgdSlDKyc5UKSsq19yp1MGrgATzY6Znq2+KJLZEZOFEkR4auo0BaSO2QAGgQCKURpdrtUcvkYnBzv0hiMRiDMuCCMpoYDYXD8DUDKckSj/ujgYcsbgwcoofEYSNKrcEXlCNhyYCAZUMdTjnTcYz8cz4ST2QROaVUboAYMgA=
            || ( ( obj is T || ( default ( T ) is null && obj is null ) ) && this.Equals ( ( T ) obj ) );

        /// <inheritdoc />
        public override Int32 GetHashCode ( )
        {
            var hashCode = -254034551;
            hashCode = hashCode * -1521134295 + this.IsSome.GetHashCode ( );
            if ( this.IsSome )
                hashCode = hashCode * -1521134295 + EqualityComparer<T?>.Default.GetHashCode ( this._some );
            return hashCode;
        }

        #endregion Object

        #region IEquatable<T>

        /// <inheritdoc />
        public Boolean Equals ( Option<T> other ) =>
            ( this.IsSome && other.IsSome && EqualityComparer<T>.Default.Equals ( this._some, other._some ) )
            || ( this.IsNone && other.IsNone );

        /// <summary>
        /// Indicates whether this object contains a value and that value is equal to another value
        /// of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true" /> if the current object contains a value and that value is equal
        /// to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public Boolean Equals ( T? other ) =>
            this.IsSome && EqualityComparer<T?>.Default.Equals ( this._some, other );

        #endregion IEquatable<T>

        #region Operators

        /// <summary>
        /// Checks whether the left Option is equal to the right Option.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="true" /> if both options don't have a value or both have a value and the
        /// value is the same for both; otherwise, <see langword="false" />.
        /// </returns>
        public static Boolean operator == ( Option<T> left, Option<T> right ) => left.Equals ( right );

        /// <summary>
        /// Checks whether the left Option is not equal to the right Option.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="false" /> if both options don't have a value or both have a value and the
        /// value is the same for both; otherwise, <see langword="true" />.
        /// </returns>
        public static Boolean operator != ( Option<T> left, Option<T> right ) => !( left == right );

        /// <summary>
        /// Checks whether the left value is equal to the right Option.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="true" /> if the option contains a value and that value is equal to
        /// <paramref name="left" />; otherwise, <see langword="false" />.
        /// </returns>
        public static Boolean operator == ( T left, Option<T> right ) => right.Equals ( left );

        /// <summary>
        /// Checks whether the left value is not equal to the right Option.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="false" /> if the option contains a value and that value is equal to
        /// <paramref name="left" />; otherwise, <see langword="true" />.
        /// </returns>
        public static Boolean operator != ( T left, Option<T> right ) => !( left == right );

        /// <summary>
        /// Checks whether the left Option is equal to the right Option.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="true" /> if the option contains a value and that value is equal to
        /// <paramref name="right" />; otherwise, <see langword="false" />.
        /// </returns>
        public static Boolean operator == ( Option<T> left, T right ) => left.Equals ( right );

        /// <summary>
        /// Checks whether the left Option is not equal to the right Option.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="false" /> if the option contains a value and that value is equal to
        /// <paramref name="right" />; otherwise, <see langword="true" />.
        /// </returns>
        public static Boolean operator != ( Option<T> left, T right ) => !( left == right );

        /// <summary>
        /// Converts a value to a <see cref="Option.Some{T}(T)" /> option.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Usage", "CA2225:Operator overloads have named alternates", Justification = "Static method Option.Some<T>(T) exists." )]
        public static implicit operator Option<T> ( T value ) =>
            new Option<T> ( value );

        #endregion Operators

        private String GetDebuggerDisplay ( ) =>
            this.IsSome ? $"Some({this._some})" : "None";
    }

    /// <summary>
    /// Extension methods that can't be implemented directly in <see cref="Option{T}" />.
    /// </summary>
    public static class OptionExtensions
    {
        /// <summary>
        /// Flattens a nested <see cref="Option{T}" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Option<T> Flatten<T> ( this Option<Option<T>> self ) =>
            self.Map ( opt => opt.Value );

        /// <summary>
        /// Transposes an Option of a Result into a Result of an Option.
        /// </summary>
        /// <remarks>
        /// The mapping rules are the same as rust:
        /// <list type="bullet">
        /// <item>None → Ok(None)</item>
        /// <item>Some(Ok(x)) → Ok(Some(x))</item>
        /// <item>Some(Err(x)) → Err(Some(x))</item>
        /// </list>
        /// </remarks>
        /// <typeparam name="TOk"></typeparam>
        /// <typeparam name="TErr"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Result<Option<TOk>, TErr> Transpose<TOk, TErr> ( this Option<Result<TOk, TErr>> self ) =>
            self switch
            {
                { IsNone: true } => Result.Ok<Option<TOk>, TErr> ( Option.None<TOk> ( ) ),
                { IsSome: true, Value: { IsOk: true, Ok: var ok } } => Result.Ok<Option<TOk>, TErr> ( ok ),
                { IsSome: true, Value: { IsErr: true, Err: var err } } => Result.Err<Option<TOk>, TErr> ( err.Value ),
                _ => throw new InvalidOperationException ( "This branch shouldn't be hit!" )
            };
    }
}