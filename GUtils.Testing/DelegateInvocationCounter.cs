using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace GUtils.Testing
{
    /// <summary>
    /// A class that tracks the amount of times its <see cref="WrappedDelegate" /> was invoked.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateInvocationCounter<T>
        where T : Delegate
    {
        private Int32 _invocationCount;

        /// <summary>
        /// The number of times <see cref="WrappedDelegate" /> was invoked.
        /// </summary>
        public Int32 InvocationCount => this._invocationCount;

        /// <summary>
        /// The wrapper delegate that increments the invocation count when invoked.
        /// </summary>
        public T WrappedDelegate { get; internal set; } = null!;

        /// <summary>
        /// Atomically increments the number of invocations of this delegate.
        /// </summary>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        internal void Increment ( ) =>
            Interlocked.Increment ( ref this._invocationCount );

        /// <summary>
        /// Resets the number of invocations of this counter.
        /// </summary>
        public void Reset ( ) =>
            this._invocationCount = 0;
    }
}