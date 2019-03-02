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
using System.Collections.Concurrent;

namespace GUtils.Pooling
{
    /// <summary>
    /// Represents a pool of objects of type
    /// <typeparamref name="T" />. Used to reduce the amount of
    /// allocations throughout the program.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// Restricted to classes because value types are passed by
    /// copy, thus there's no point to having them in a pool.
    /// </remarks>
    public abstract partial class Pool<T> where T : class
    {
        /// <summary>
        /// The bucket containing the pooled resources
        /// </summary>
        private readonly ConcurrentBag<T> Bucket;

        /// <summary>
        /// Whether returned items should be cleared by default
        /// </summary>
        public Boolean AreItemsClearedByDefault { get; }

        /// <summary>
        /// The maximum capacity of this <see cref="Pool{T}" />
        /// </summary>
        public Int32 Capacity { get; }

        /// <summary>
        /// Initializes the pool with the given capacity and sets
        /// the default returned item clearing behavior.
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="shouldClearByDefault"></param>
        protected Pool ( Int32 capacity, Boolean shouldClearByDefault )
        {
            this.Bucket = new ConcurrentBag<T> ( );
            this.AreItemsClearedByDefault = shouldClearByDefault;
            this.Capacity = capacity;
        }

        /// <summary>
        /// Initializes a new item
        /// </summary>
        /// <returns></returns>
        protected abstract T InitializeItem ( );

        /// <summary>
        /// Resets an item's state
        /// </summary>
        /// <param name="item"></param>
        protected abstract void ResetItem ( T item );

        /// <summary>
        /// Rents an object.
        /// </summary>
        /// <returns></returns>
        public T Rent ( ) =>
            // Return an object if we have one stored otherwise
            // create and return a new one
            this.Bucket.TryTake ( out T item )
                ? item
                : this.InitializeItem ( );

        /// <summary>
        /// Returns to the pool an item obtained with
        /// <see cref="Rent" />.
        /// </summary>
        /// <param name="item"></param>
        public void Return ( T item ) => this.Return ( item, this.AreItemsClearedByDefault );

        /// <summary>
        /// Returns to the pool an item obtained with
        /// <see cref="Rent" />
        /// </summary>
        /// <param name="item"></param>
        /// <param name="resetItem"></param>
        public void Return ( T item, Boolean resetItem )
        {
            // If we're already over the capacity, just throw the
            // object away.
            if ( this.Bucket.Count >= this.Capacity )
                return;

            // Otherwise check and apply the default clearing
            // behavior
            if ( resetItem )
                this.ResetItem ( item );

            // And add the object back into the bucket
            this.Bucket.Add ( item );
        }

        /// <summary>
        /// Executes an action with a rented item
        /// </summary>
        /// <param name="action"></param>
        public void WithRentedItem ( Action<T> action )
        {
            if ( action == null )
                throw new ArgumentNullException ( nameof ( action ) );

            T item = this.Rent ( );
            try
            {
                action ( item );
            }
            finally
            {
                this.Return ( item );
            }
        }

        /// <summary>
        /// Executes a function with a rented item
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public R WithRentedItem<R> ( Func<T, R> func )
        {
            if ( func == null )
                throw new ArgumentNullException ( nameof ( func ) );

            T item = this.Rent ( );
            try
            {
                return func ( item );
            }
            finally
            {
                this.Return ( item );
            }
        }
    }
}
