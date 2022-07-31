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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Tsu
{
    /// <summary>
    /// The class with the factory methods for the result
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// Creates a successful result
        /// </summary>
        /// <typeparam name="TOk"></typeparam>
        /// <typeparam name="TErr"></typeparam>
        /// <param name="ok"></param>
        /// <returns></returns>
        public static Result<TOk, TErr> Ok<TOk, TErr>(TOk ok) =>
            new Result<TOk, TErr>(true, ok, default!);

        /// <summary>
        /// Creates an unsuccessful result
        /// </summary>
        /// <typeparam name="TOk"></typeparam>
        /// <typeparam name="TErr"></typeparam>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<TOk, TErr> Err<TOk, TErr>(TErr error) =>
            new Result<TOk, TErr>(false, default!, error);
    }

    /// <summary>
    /// A result type
    /// </summary>
    /// <typeparam name="TOk"></typeparam>
    /// <typeparam name="TErr"></typeparam>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct Result<TOk, TErr> : IEquatable<Result<TOk, TErr>>
    {
        private readonly TOk _ok;
        private readonly TErr _err;

        /// <summary>
        /// Whether this result is Ok.
        /// </summary>
        public bool IsOk { get; }

        /// <summary>
        /// Whether this result is an Error.
        /// </summary>
        // This is implemented as a calculated propery because, otherwise, by calling the default
        // constructor, or using the default operator, we'd get a Result that both isn't Ok nor Err.
        public bool IsErr => !IsOk;

        /// <summary>
        /// The result of the operation if it was successful.
        /// </summary>
        public Option<TOk> Ok =>
            IsOk ? Option.Some(_ok) : Option.None<TOk>();

        /// <summary>
        /// The error of the operation if it errored.
        /// </summary>
        public Option<TErr> Err =>
            IsErr ? Option.Some(_err) : Option.None<TErr>();

        internal Result(bool isOk, TOk ok, TErr err)
        {
            IsOk = isOk;
            _ok = ok;
            _err = err;
        }

        /// <summary>
        /// Returns <see langword="true" /> if this is an Ok result and the contained value is equal
        /// to the provided <paramref name="value" />.
        /// </summary>
        /// <typeparam name="TOther"></typeparam>
        /// <param name="value">
        /// The value to be compared to the containing value if it's an Ok result.
        /// </param>
        /// <returns></returns>
        public bool Contains<TOther>(TOther value) where TOther : IEquatable<TOk> =>
            IsOk && (value?.Equals(_ok) is true
                     || (default(TOk) is null
                         && default(TOther) is null
                         && value is null
                         && _ok is null));

        /// <summary>
        /// Returns <see langword="true" /> if this is an Err result and the contained Err value is
        /// equal to the provided <paramref name="value" />.
        /// </summary>
        /// <typeparam name="TOther"></typeparam>
        /// <param name="value">
        /// The value to be compared to the containing Err value if it's an Err result.
        /// </param>
        /// <returns></returns>
        public bool ContainsErr<TOther>(TOther value) where TOther : IEquatable<TErr> =>
            IsErr && (value?.Equals(_err) is true
                      || (default(TErr) is null
                          && default(TOther) is null
                          && value is null
                          && _err is null));

        /// <summary>
        /// Maps a <c>Result&lt; <typeparamref name="TOk" />, <typeparamref name="TErr" />&gt;</c>
        /// to <c>Result&lt; <typeparamref name="TMappedOk" />, <typeparamref name="TErr" />&gt;</c>
        /// by invoking the provided <paramref name="op" /> on the wrapped Ok value (if any),
        /// leaving Err untouched.
        /// </summary>
        /// <typeparam name="TMappedOk">
        /// The type that the contained value (if any) will be mapped to.
        /// </typeparam>
        /// <param name="op">The function that maps the result if this Result contains one.</param>
        /// <returns>A Result with the mapped result if it contains one. Returns Err otherwise.</returns>
        public Result<TMappedOk, TErr> Map<TMappedOk>(Func<TOk, TMappedOk> op)
        {
            if (op is null)
                throw new ArgumentNullException(nameof(op));

            return IsOk ? Result.Ok<TMappedOk, TErr>(op(_ok)) : Result.Err<TMappedOk, TErr>(_err);
        }

        /// <summary>
        /// Maps a <c>Result&lt; <typeparamref name="TOk" />, <typeparamref name="TErr" />&gt;</c>
        /// to <c><typeparamref name="TMappedOk" /></c> by invoking the provided <paramref name="op"
        /// /> on the wrapped Ok value or returns the provided <paramref name="fallback" /> value if
        /// the value contained is an Err.
        /// </summary>
        /// <typeparam name="TMappedOk"></typeparam>
        /// <param name="fallback"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public TMappedOk MapOr<TMappedOk>(TMappedOk fallback, Func<TOk, TMappedOk> op)
        {
            if (op is null)
                throw new ArgumentNullException(nameof(op));

            return IsOk ? op(_ok) : fallback;
        }

        /// <summary>
        /// Maps a <c>Result&lt; <typeparamref name="TOk" />, <typeparamref name="TErr" />&gt;</c>
        /// to <c><typeparamref name="TMappedOk" /></c> by invoking the provided <paramref name="op"
        /// /> on the wrapped Ok value or returns the result of invoking the <paramref
        /// name="fallbackFunc" /> function if the value contained is an Err.
        /// </summary>
        /// <typeparam name="TMappedOk"></typeparam>
        /// <param name="fallbackFunc">
        /// The fallback function to be called if this Result doesn't contain a value.
        /// </param>
        /// <param name="op">The function to call with the value contained by this Result (if any).</param>
        /// <returns></returns>
        public TMappedOk MapOrElse<TMappedOk>(Func<TMappedOk> fallbackFunc, Func<TOk, TMappedOk> op)
        {
            if (fallbackFunc is null)
                throw new ArgumentNullException(nameof(fallbackFunc));
            if (op is null)
                throw new ArgumentNullException(nameof(op));

            return IsOk ? op(_ok) : fallbackFunc();
        }

        /// <summary>
        /// Maps a <c>Result&lt; <typeparamref name="TOk" />, <typeparamref name="TErr" />&gt;</c>
        /// into a <c>Result&lt; <typeparamref name="TOk" />, <typeparamref name="TMappedErr"
        /// />&gt;</c> by applying the provided <paramref name="op" /> on the error (if any).
        /// </summary>
        /// <typeparam name="TMappedErr"></typeparam>
        /// <param name="op"></param>
        /// <returns></returns>
        public Result<TOk, TMappedErr> MapErr<TMappedErr>(Func<TErr, TMappedErr> op)
        {
            if (op is null)
                throw new ArgumentNullException(nameof(op));

            return IsErr ? Result.Err<TOk, TMappedErr>(op(_err)) : Result.Ok<TOk, TMappedErr>(_ok);
        }

        /// <summary>
        /// Returns <paramref name="res" /> if this result is Ok ( <see cref="IsOk" />). Otherwise
        /// returns an Err containing the error in this result.
        /// </summary>
        /// <typeparam name="TOtherOk"></typeparam>
        /// <param name="res">The result to be retured if this is an Ok result.</param>
        /// <returns></returns>
        public Result<TOtherOk, TErr> And<TOtherOk>(Result<TOtherOk, TErr> res) =>
            IsOk ? res : Result.Err<TOtherOk, TErr>(_err);

        /// <summary>
        /// Returns the result of invoking <paramref name="op" /> with this ok value if this result
        /// is Ok. Otherwise returns an Err containing th error in this result.
        /// </summary>
        /// <typeparam name="TMappedOk"></typeparam>
        /// <param name="op">
        /// The delegate to be invoked with the Ok value of this result if it's an Ok result.
        /// </param>
        /// <returns></returns>
        public Result<TMappedOk, TErr> AndThen<TMappedOk>(Func<TOk, Result<TMappedOk, TErr>> op)
        {
            if (op is null)
                throw new ArgumentNullException(nameof(op));

            return IsOk ? op(_ok) : Result.Err<TMappedOk, TErr>(_err);
        }

        /// <summary>
        /// Returns this result if it's an Ok result. Otherwies returns <paramref name="res" />.
        /// </summary>
        /// <typeparam name="TOtherErr"></typeparam>
        /// <param name="res">The result to be returned if this is an Err result.</param>
        /// <returns></returns>
        public Result<TOk, TOtherErr> Or<TOtherErr>(Result<TOk, TOtherErr> res) =>
            IsErr ? res : Result.Ok<TOk, TOtherErr>(_ok);

        /// <summary>
        /// Returns this result if it's an Ok result. Otherwise returns the result of invoking
        /// <paramref name="op" /> with the Err value of this result if this is an Err result.
        /// </summary>
        /// <typeparam name="TMappedErr"></typeparam>
        /// <param name="op">
        /// The function to be invoked with the Err value of this result if it's an Err result.
        /// </param>
        /// <returns></returns>
        public Result<TOk, TMappedErr> OrThen<TMappedErr>(Func<TErr, Result<TOk, TMappedErr>> op)
        {
            if (op is null)
                throw new ArgumentNullException(nameof(op));

            return IsErr ? op(_err) : Result.Ok<TOk, TMappedErr>(_ok);
        }

        /// <summary>
        /// Returns the Ok value of this result if it's an Ok result. Otherwise returns the
        /// <paramref name="fallback" /> result.
        /// </summary>
        /// <param name="fallback">The result to be retured if this is an Err result.</param>
        /// <returns></returns>
        public TOk UnwrapOr(TOk fallback) =>
            IsOk ? _ok : fallback;

        /// <summary>
        /// </summary>
        /// <param name="fallbackFunc"></param>
        /// <returns></returns>
        public TOk UnwrapOrElse(Func<TOk> fallbackFunc)
        {
            if (fallbackFunc is null)
                throw new ArgumentNullException(nameof(fallbackFunc));

            return IsOk ? _ok : fallbackFunc();
        }

        #region IEquatable<T>

        /// <summary>
        /// Checks whether this result is equal to another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Result<TOk, TErr> other) =>
            (IsOk && other.IsOk && EqualityComparer<TOk>.Default.Equals(_ok, other._ok))
            || (IsErr && other.IsErr && EqualityComparer<TErr>.Default.Equals(_err, other._err));

        #endregion IEquatable<T>

        #region Object

#pragma warning disable IDE0079 // Remove unnecessary suppression (Valid for some target frameworks)
#pragma warning disable CS8604 // Possible null reference argument. (We ensure null passes only when it's safe)

        /// <inheritdoc />
        public override bool Equals(object? obj) =>
            (obj is Result<TOk, TErr> result && Equals(result))
             // `default(T) is null` is used here to check if T is a reference type.
             // `typeof(T).IsClass` cannot be turned into a constant by the JIT: https://sharplab.io/#v2:EYLgHgbALANALiAhgZwLYB8ACAGABJgRgDoAlAVwDs4BLVAUyIGEB7VAB2oBs6AnAZV4A3agGM6yANwBYAFCzMAZnwAmXI1wBvWbh24A2gFk6cABbMAJgEl2nABRHTF6204B5NjWYVkRAIIBzfx5xZGpBOksKTmoKGP8ASgBdbV1FXGBmZk5cAwIAHgAVAD5beM0U3VSAdlxzOgAzRDJOOFsCsupkXApmzmkZSoBfCtwRw2MzKxt7Cacbd09vP0Dg5FDwyOjYigTkgdSlDKyc5UKSsq19yp1MGrgATzY6Znq2+KJLZEZOFEkR4auo0BaSO2QAGgQCKURpdrtUcvkYnBzv0hiMRiDMuCCMpoYDYXD8DUDKckSj/ujgYcsbgwcoofEYSNKrcEXlCNhyYCAZUMdTjnTcYz8cz4ST2QROaVUboAYMgA=
             || (IsOk
                 && (obj is TOk || (default(TOk) is null && obj is null))
                 && EqualityComparer<TOk>.Default.Equals((TOk?) obj, _ok))
             || (IsErr
                 && (obj is TErr || (default(TErr) is null && obj is null))
                 && EqualityComparer<TErr>.Default.Equals((TErr?) obj, _err));

#pragma warning restore CS8604 // Possible null reference argument. (We ensure null passes only when it's safe)
#pragma warning restore IDE0079 // Remove unnecessary suppression (Valid for some target frameworks)

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = -1322039524;
            hashCode = hashCode * -1521134295 + IsOk.GetHashCode();
            if (IsOk)
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression (required for some target frameworks)
#pragma warning disable CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull] (GetHashCode supports nulls)
                hashCode = hashCode * -1521134295 + EqualityComparer<TOk?>.Default.GetHashCode(_ok);
#pragma warning restore CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull] (GetHashCode supports nulls)
#pragma warning restore IDE0079 // Remove unnecessary suppression (required for some target frameworks)
            }
            else
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression (required for some target frameworks)
#pragma warning disable CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull] (GetHashCode supports nulls)
                hashCode = hashCode * -1521134295 + EqualityComparer<TErr?>.Default.GetHashCode(_err);
#pragma warning restore CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull] (GetHashCode supports nulls)
#pragma warning restore IDE0079 // Remove unnecessary suppression (required for some target frameworks)
            }
            return hashCode;
        }

        #endregion Object

        #region Operators

        /// <summary>
        /// Checks structural equality between two <see cref="Result{TOk, TErr}" /> s.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Result<TOk, TErr> left, Result<TOk, TErr> right) => left.Equals(right);

        /// <summary>
        /// Checks structural difference between two <see cref="Result{TOk, TErr}" /> s.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Result<TOk, TErr> left, Result<TOk, TErr> right) => !(left == right);

        /// <summary>
        /// Converts an <paramref name="ok" /> value to a result.
        /// </summary>
        /// <param name="ok"></param>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Result.Ok<TOk, TErr>(TOk) exists.")]
        public static explicit operator Result<TOk, TErr>(TOk ok) =>
            new Result<TOk, TErr>(true, ok, default!);

        /// <summary>
        /// Converts an <paramref name="err" /> value to a result.
        /// </summary>
        /// <param name="err"></param>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Result.Err<TOk, TErr>(TErr) exists.")]
        public static explicit operator Result<TOk, TErr>(TErr err) =>
            new Result<TOk, TErr>(false, default!, err);

        #endregion Operators

        private string GetDebuggerDisplay() =>
            IsOk ? $"Ok({_ok})" : $"Err({_err})";
    }

    /// <summary>
    /// Extension methods for <see cref="Result{TOk, TErr}" />.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Transposes a Result of an Option into an Option of a Result.
        /// </summary>
        /// <remarks>
        /// The mapping rules are the same as rust:
        /// <list type="bullet">
        /// <item>Ok(None) → None</item>
        /// <item>Ok(Some(x)) → Some(Ok(x))</item>
        /// <item>Err(x) → Some(Err(x))</item>
        /// </list>
        /// </remarks>
        /// <typeparam name="TOk"></typeparam>
        /// <typeparam name="TErr"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Option<Result<TOk, TErr>> Transpose<TOk, TErr>(this Result<Option<TOk>, TErr> self)
        {
            return self switch
            {
                { IsOk: true, Ok: { Value: var nestedOption } } => nestedOption switch
                {
                    // Ok(None) -> None
                    { IsNone: true } => Option.None<Result<TOk, TErr>>(),
                    // Ok(Some(_)) -> Some(Ok(_))
                    { IsSome: true, Value: var value } => Option.Some(Result.Ok<TOk, TErr>(value)),
                    // Branch shouldn't be reached
                    _ => throw new Exception("This branch shouldn't be reached."),
                },
                // Err(_) -> Some(Err(_))
                { IsErr: true, Err: { Value: var value } } => Option.Some(Result.Err<TOk, TErr>(value)),
                // Branch shouldn't be reached
                _ => throw new Exception("This branch shouldn't be reached."),
            };
        }
    }
}