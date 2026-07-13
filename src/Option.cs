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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace Tsu
{
    /// <summary>
    /// The class that contains the constructors for an Option.
    /// </summary>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "It is the name of the type in rust.")]
    public static class Option
    {
        /// <summary>
        /// Constructs a Some option
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Option<T> Some<T>(T value) =>
            new Option<T>(value);

        /// <summary>
        /// Constructs a None option
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Option<T> None<T>() =>
            new Option<T>();
    }

    /// <summary>
    /// The Option type. It represents an optional value. Create one with <see cref="Option.None{T}"
    /// /> or <see cref="Option.Some{T}(T)" />.
    /// </summary>
    /// <typeparam name="T">The type this option wraps.</typeparam>
    /// <remarks>
    /// This type implements <see cref="operator true(Option{T})"/> and <see cref="operator false(Option{T})"/> so you
    /// can use it directly in a condition to check if it is Some(T).
    /// </remarks>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "It is the name of the type in rust.")]
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
    {
        /// <summary>
        /// The value stored by this option
        /// </summary>
        private readonly T _some;

        /// <summary>
        /// Whether the option contains a value.
        /// </summary>
        public bool IsSome { get; }

        /// <summary>
        /// Whether the option doesn't contain a value.
        /// </summary>
        public bool IsNone => !IsSome;

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
                if (IsNone)
                {
                    throw new InvalidOperationException("Can't get the value of an option that doesn't have one.");
                }

                return _some;
            }
        }

        /// <summary>
        /// Initializes an Option with a value
        /// </summary>
        /// <param name="value"></param>
        internal Option(T value)
        {
            IsSome = true;
            _some = value;
        }

        /// <summary>
        /// Attempts to get the value from this option.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// <see langword="true"/> if this option is Some(T);
        /// <para><see langword="false"/> if this option is None.</para>
        /// </returns>
        public bool TryGetValue(out T value)
        {
            value = _some;
            return IsSome;
        }

        /// <summary>
        /// Returns the wrapped value or the provided <paramref name="fallback" />.
        /// </summary>
        /// <param name="fallback">The value to be returned if this option doesn't contain a value.</param>
        /// <returns>The wrapped value or the <paramref name="fallback" />.</returns>
        public T UnwrapOr(T fallback) =>
            IsSome ? _some : fallback;

        /// <summary>
        /// Returns the wrapped value or the result of the provided delegate.
        /// </summary>
        /// <param name="func">The delegate to execute in case the this is a None.</param>
        /// <returns>The wrapped value or the result of invoking <paramref name="func" />.</returns>
        public T UnwrapOrElse(Func<T> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return IsSome ? _some : func();
        }

        /// <summary>
        /// Converts an Option of one type to another by using the provided transformation function
        /// ( <paramref name="func" />). If the current object doesn't have a value, a None for the
        /// result type is returned instead.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public Option<TResult> Map<TResult>(Func<T, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return IsSome ? Option.Some(func(_some)) : Option.None<TResult>();
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
        public TResult MapOr<TResult>(TResult fallback, Func<T, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return IsSome ? func(_some) : fallback;
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
        public TResult MapOrElse<TResult>(Func<TResult> fallback, Func<T, TResult> func)
        {
            if (fallback is null)
                throw new ArgumentNullException(nameof(fallback));
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return IsSome ? func(_some) : fallback();
        }

        /// <summary>
        /// Returns none if this option is none otherwise returns <paramref name="other" />
        /// </summary>
        /// <typeparam name="TOther">The wrapped type of the other option.</typeparam>
        /// <param name="other"></param>
        /// <returns></returns>
        public Option<TOther> And<TOther>(Option<TOther> other) =>
            IsSome ? other : Option.None<TOther>();

        /// <summary>
        /// Returns None if this Option is None, otherwise returns the result of invoking the
        /// provided function with the value.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public Option<TResult> AndThen<TResult>(Func<T, Option<TResult>> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return IsSome ? func(_some) : Option.None<TResult>();
        }

        /// <summary>
        /// Returns None if this option is None or the predicate fails. If it passes then it returns
        /// this option.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Option<T> Filter(Func<T, bool> filter)
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));

            return IsSome && filter(_some) ? this : Option.None<T>();
        }

        /// <summary>
        /// Returns this option if it has a value otherwise it returns <paramref name="other" />.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Option<T> Or(Option<T> other) =>
            IsSome ? this : other;

        /// <summary>
        /// Returns this option if it has a value otherwise returns the result of invoking the
        /// provided <paramref name="func" />.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Option<T> OrElse(Func<Option<T>> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return IsSome ? this : func();
        }

        /// <summary>
        /// Return some if either this option or <paramref name="other" /> is Some, otherwise
        /// returns None.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Option<T> Xor(Option<T> other)
        {
            if (IsSome && !other.IsSome)
                return this;
            else if (!IsSome && other.IsSome)
                return other;
            else
                return Option.None<T>();
        }

        #region Object

        /// <inheritdoc />
        public override bool Equals(object? obj) =>
            (obj is Option<T> option && Equals(option))
            // `default(T) is null` is used here to check if T is a reference type.
            // `typeof(T).IsClass` cannot be turned into a constant by the JIT: https://sharplab.io/#v2:EYLgHgbALANALiAhgZwLYB8ACAGABJgRgDoAlAVwDs4BLVAUyIGEB7VAB2oBs6AnAZV4A3agGM6yANwBYAFCzMAZnwAmXI1wBvWbh24A2gFk6cABbMAJgEl2nABRHTF6204B5NjWYVkRAIIBzfx5xZGpBOksKTmoKGP8ASgBdbV1FXGBmZk5cAwIAHgAVAD5beM0U3VSAdlxzOgAzRDJOOFsCsupkXApmzmkZSoBfCtwRw2MzKxt7Cacbd09vP0Dg5FDwyOjYigTkgdSlDKyc5UKSsq19yp1MGrgATzY6Znq2+KJLZEZOFEkR4auo0BaSO2QAGgQCKURpdrtUcvkYnBzv0hiMRiDMuCCMpoYDYXD8DUDKckSj/ujgYcsbgwcoofEYSNKrcEXlCNhyYCAZUMdTjnTcYz8cz4ST2QROaVUboAYMgA=
            || ((obj is T || (default(T) is null && obj is null)) && Equals((T?) obj));

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = -254034551;
            hashCode = hashCode * -1521134295 + IsSome.GetHashCode();
            if (IsSome)
#pragma warning disable IDE0079 // Remove unnecessary suppression (required for some target frameworks)
#pragma warning disable CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull] (GetHashCode supports nulls)
                hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(_some);
#pragma warning restore CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull] (GetHashCode supports nulls)
#pragma warning restore IDE0079 // Remove unnecessary suppression (required for some target frameworks)
            return hashCode;
        }

        #endregion Object

        #region IEquatable<T>

        /// <inheritdoc />
        public bool Equals(Option<T> other) =>
            (IsSome && other.IsSome && EqualityComparer<T>.Default.Equals(_some, other._some))
            || (IsNone && other.IsNone);

        /// <summary>
        /// Indicates whether this object contains a value and that value is equal to another value
        /// of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true" /> if the current object contains a value and that value is equal
        /// to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(T? other) =>
            IsSome && EqualityComparer<T?>.Default.Equals(_some, other);

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
        public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);

        /// <summary>
        /// Checks whether the left Option is not equal to the right Option.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="false" /> if both options don't have a value or both have a value and the
        /// value is the same for both; otherwise, <see langword="true" />.
        /// </returns>
        public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);

        /// <summary>
        /// Checks whether the left value is equal to the right Option.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="true" /> if the option contains a value and that value is equal to
        /// <paramref name="left" />; otherwise, <see langword="false" />.
        /// </returns>
        public static bool operator ==(T left, Option<T> right) => right.Equals(left);

        /// <summary>
        /// Checks whether the left value is not equal to the right Option.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="false" /> if the option contains a value and that value is equal to
        /// <paramref name="left" />; otherwise, <see langword="true" />.
        /// </returns>
        public static bool operator !=(T left, Option<T> right) => !(left == right);

        /// <summary>
        /// Checks whether the left Option is equal to the right Option.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="true" /> if the option contains a value and that value is equal to
        /// <paramref name="right" />; otherwise, <see langword="false" />.
        /// </returns>
        public static bool operator ==(Option<T> left, T right) => left.Equals(right);

        /// <summary>
        /// Checks whether the left Option is not equal to the right Option.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="false" /> if the option contains a value and that value is equal to
        /// <paramref name="right" />; otherwise, <see langword="true" />.
        /// </returns>
        public static bool operator !=(Option<T> left, T right) => !(left == right);

        /// <summary>
        /// Checks whether this <see cref="Option{T}"/> is Some(T).
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Users can check IsSome.")]
        public static bool operator true(Option<T> option) => option.IsSome;

        /// <summary>
        /// Checks whether this <see cref="Option{T}"/> is None.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool operator false(Option<T> option) => option.IsNone;

        /// <summary>
        /// Converts a value to a <see cref="Option.Some{T}(T)" /> option.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Static method Option.Some<T>(T) exists.")]
        public static implicit operator Option<T>(T value) =>
            new Option<T>(value);

        /// <summary>
        /// Attempts to obtain the <see cref="Value"/> from an <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="option"></param>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "This is just a shortcut to the Value property.")]
        public static explicit operator T(Option<T> option) =>
            option.Value;

        #endregion Operators

        private string GetDebuggerDisplay() =>
            IsSome ? $"Some({_some})" : "None";
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
        public static Option<T> Flatten<T>(this Option<Option<T>> self) =>
            self.Map(opt => opt.Value);

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
        public static Result<Option<TOk>, TErr> Transpose<TOk, TErr>(this Option<Result<TOk, TErr>> self) =>
            self switch
            {
                { IsNone: true } => Result.Ok<Option<TOk>, TErr>(Option.None<TOk>()),
                { IsSome: true, Value: { IsOk: true, Ok: var ok } } => Result.Ok<Option<TOk>, TErr>(ok),
                { IsSome: true, Value: { IsErr: true, Err: var err } } => Result.Err<Option<TOk>, TErr>(err.Value),
                _ => throw new InvalidOperationException("This branch shouldn't be hit!")
            };
    }
}