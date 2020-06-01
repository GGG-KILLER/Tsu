/*
 * Copyright © 2020 GGG KILLER <gggkiller2@gmail.com>
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

namespace GUtils.Testing
{
    /// <summary>
    /// A class that contains helpers for dealing with delegates.
    /// </summary>
    public static partial class DelegateHelpers
    {
        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action> TrackInvocationCount ( Action @delegate )
        {
            var counter = new DelegateInvocationCounter<Action> ( );
            counter.WrappedDelegate = ( ) =>
            {
                counter.Increment ( );
                @delegate ( );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1>> TrackInvocationCount<T1> ( Action<T1> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1>> ( );
            counter.WrappedDelegate = ( arg1 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2>> TrackInvocationCount<T1, T2> ( Action<T1, T2> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2>> ( );
            counter.WrappedDelegate = ( arg1, arg2 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3>> TrackInvocationCount<T1, T2, T3> ( Action<T1, T2, T3> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4>> TrackInvocationCount<T1, T2, T3, T4> ( Action<T1, T2, T3, T4> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4, T5>> TrackInvocationCount<T1, T2, T3, T4, T5> ( Action<T1, T2, T3, T4, T5> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4, T5>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4, arg5 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6>> TrackInvocationCount<T1, T2, T3, T4, T5, T6> ( Action<T1, T2, T3, T4, T5, T6> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4, arg5, arg6 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7> ( Action<T1, T2, T3, T4, T5, T6, T7> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8> ( Action<T1, T2, T3, T4, T5, T6, T7, T8> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9> ( Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ( Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ( Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ( Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ( Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ( Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ( Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ( Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> @delegate )
        {
            var counter = new DelegateInvocationCounter<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 ) =>
            {
                counter.Increment ( );
                @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<TReturn>> TrackInvocationCount<TReturn> ( Func<TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<TReturn>> ( );
            counter.WrappedDelegate = ( ) =>
            {
                counter.Increment ( );
                return @delegate ( );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, TReturn>> TrackInvocationCount<T1, TReturn> ( Func<T1, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, TReturn>> ( );
            counter.WrappedDelegate = ( arg1 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, TReturn>> TrackInvocationCount<T1, T2, TReturn> ( Func<T1, T2, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, TReturn>> TrackInvocationCount<T1, T2, T3, TReturn> ( Func<T1, T2, T3, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, TReturn>> TrackInvocationCount<T1, T2, T3, T4, TReturn> ( Func<T1, T2, T3, T4, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, TReturn>> TrackInvocationCount<T1, T2, T3, T4, T5, TReturn> ( Func<T1, T2, T3, T4, T5, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4, arg5 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, TReturn>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, TReturn> ( Func<T1, T2, T3, T4, T5, T6, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4, arg5, arg6 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, TReturn>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, TReturn> ( Func<T1, T2, T3, T4, T5, T6, T7, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> ( Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn> ( Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn> ( Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn> ( Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn> ( Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn> ( Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn> ( Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn> ( Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 );
            };
            return counter;
        }

        /// <summary>
        /// Wraps the provided delegate into a <see cref="DelegateInvocationCounter{T}"/>.
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn>> TrackInvocationCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn> ( Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn> @delegate )
        {
            var counter = new DelegateInvocationCounter<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn>> ( );
            counter.WrappedDelegate = ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 ) =>
            {
                counter.Increment ( );
                return @delegate ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 );
            };
            return counter;
        }
    }
}