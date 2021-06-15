// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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
using System.Runtime.CompilerServices;
using System.Threading;

namespace Tsu.Testing
{
    /// <summary>
    /// A class that tracks the amount of times its <see cref="WrappedDelegate" /> was invoked.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateInvocationCounter<T>
        where T : Delegate
    {
        private int _invocationCount;

        /// <summary>
        /// The number of times <see cref="WrappedDelegate" /> was invoked.
        /// </summary>
        public int InvocationCount => _invocationCount;

        /// <summary>
        /// The wrapper delegate that increments the invocation count when invoked.
        /// </summary>
        public T WrappedDelegate { get; internal set; } = null!;

        /// <summary>
        /// Atomically increments the number of invocations of this delegate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Increment() =>
            Interlocked.Increment(ref _invocationCount);

        /// <summary>
        /// Resets the number of invocations of this counter.
        /// </summary>
        public void Reset() =>
            _invocationCount = 0;
    }
}