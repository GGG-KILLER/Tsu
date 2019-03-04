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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace GUtils.Windows.WPF.MVVM
{
    public class CollectionProxy<T> : ICollectionProxy<T>
    {
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public ICollection<T> Collection { get; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public Int32 Count => this.Collection.Count;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public Boolean IsReadOnly => this.Collection.IsReadOnly;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public event EventHandler<T> ItemAdded;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public event EventHandler<T> ItemRemoved;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public event EventHandler<ItemInsertedEventArgs<T>> ItemInserted;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public event EventHandler Cleared;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Initializes a <see cref="CollectionProxy{T}" />
        /// </summary>
        /// <param name="collection"></param>
        public CollectionProxy ( ICollection<T> collection )
        {
            this.Collection = collection;
        }

        protected virtual void OnItemAdded ( T item )
        {
            this.ItemAdded?.Invoke ( this, item );
            this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Add, item ) );
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="item"></param>
        public void Add ( T item )
        {
            this.Collection.Add ( item );
            this.OnItemAdded ( item );
        }

        protected virtual void OnClear ( )
        {
            this.Cleared?.Invoke ( this, EventArgs.Empty );
            this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Reset ) );
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public void Clear ( )
        {
            this.Collection.Clear ( );
            this.OnClear ( );
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Boolean Contains ( T item ) =>
            this.Collection.Contains ( item );

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo ( T[] array, Int32 arrayIndex ) =>
            this.Collection.CopyTo ( array, arrayIndex );

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator ( ) =>
            this.Collection.GetEnumerator ( );

        protected virtual void OnItemRemoved ( T item )
        {
            this.ItemRemoved?.Invoke ( this, item );
            this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Remove, item ) );
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Boolean Remove ( T item )
        {
            try
            {
                return this.Collection.Remove ( item );
            }
            finally
            {
                this.ItemRemoved?.Invoke ( this, item );
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator ( ) =>
            ( ( IEnumerable ) this.Collection ).GetEnumerator ( );
    }
}
