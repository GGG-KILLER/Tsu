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
using System.Collections.Specialized;

namespace GUtils.Windows.WPF.MVVM
{
    /// <summary>
    /// Represents an item being inserted
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ItemInsertedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// The item that was inserted
        /// </summary>
        public T Item { get; }

        /// <summary>
        /// The index at which the item was inserted
        /// </summary>
        public Int32 Index { get; }

        /// <summary>
        /// Initializes this <see cref="ItemInsertedEventArgs{T}" />
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        public ItemInsertedEventArgs ( T item, Int32 index )
        {
            this.Item = item;
            this.Index = index;
        }
    }

    /// <summary>
    /// Represents a proxied collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICollectionProxy<T> : ICollection<T>, INotifyCollectionChanged
    {
        /// <summary>
        /// The collection being proxied
        /// </summary>
        ICollection<T> Collection { get; }

        /// <summary>
        /// Called when the collection is cleared
        /// </summary>
        event EventHandler Cleared;

        /// <summary>
        /// Called when an item is added
        /// </summary>
        event EventHandler<T> ItemAdded;

        /// <summary>
        /// Called when an item is removed
        /// </summary>
        event EventHandler<T> ItemRemoved;

        /// <summary>
        /// Called when an item is inserted
        /// </summary>
        event EventHandler<ItemInsertedEventArgs<T>> ItemInserted;
    }
}
