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
using System.ComponentModel;
using System.Windows.Data;

namespace GUtils.Windows.WPF.MVVM
{
    internal class CollectionProxy<T> : ICollection<T>, IReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private static readonly PropertyChangedEventArgs PropertyChangedEventArgsForCount = new PropertyChangedEventArgs ( nameof ( Count ) );

        private static readonly PropertyChangedEventArgs PropertyChangedEventArgsForIndexer = new PropertyChangedEventArgs ( Binding.IndexerName );

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public ICollection<T> Collection { get; }

        public Int32 Count => this.Collection.Count;

        public Boolean IsReadOnly => this.Collection.IsReadOnly;

        /// <summary>
        /// Initializes a <see cref="CollectionProxy{T}" />
        /// </summary>
        /// <param name="collection"></param>
        public CollectionProxy ( ICollection<T> collection )
        {
            this.Collection = collection;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Add ( T item )
        {
            this.Collection.Add ( item );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForCount );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForIndexer );
            this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Add, item ) );
        }

        public void Clear ( )
        {
            this.Collection.Clear ( );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForCount );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForIndexer );
            this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Reset ) );
        }

        public Boolean Remove ( T item )
        {
            var ret = this.Collection.Remove ( item );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForCount );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForIndexer );
            this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Remove, item ) );
            return ret;
        }

        public Boolean Contains ( T item ) =>
            this.Collection.Contains ( item );

        public void CopyTo ( T[] array, Int32 arrayIndex ) =>
            this.Collection.CopyTo ( array, arrayIndex );

        public IEnumerator<T> GetEnumerator ( ) =>
            this.Collection.GetEnumerator ( );

        IEnumerator IEnumerable.GetEnumerator ( ) =>
            this.Collection.GetEnumerator ( );
    }
}
