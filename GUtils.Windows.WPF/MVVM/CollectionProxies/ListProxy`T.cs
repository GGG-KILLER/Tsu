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

namespace GUtils.Windows.WPF.MVVM.CollectionProxies
{
    internal class ListProxy<T> : IList<T>, IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private static readonly PropertyChangedEventArgs PropertyChangedEventArgsForCount = new PropertyChangedEventArgs ( nameof ( Count ) );

        private static readonly PropertyChangedEventArgs PropertyChangedEventArgsForIndexer = new PropertyChangedEventArgs ( Binding.IndexerName );

        public IList<T> List { get; }

        public Int32 Count => this.List.Count;

        public Boolean IsReadOnly => this.List.IsReadOnly;

        public T this[Int32 index]
        {
            get => this.List[index];

            set
            {
                if ( this.List.Count > index )
                {
                    T original = this.List[index];
                    this.List[index] = value;

                    this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForIndexer );
                    this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Replace, original, value, index ) );
                }
                else
                {
                    this.List[index] = value;

                    this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForIndexer );
                    this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Add, value ) );
                }
            }
        }

        public ListProxy ( IList<T> list )
        {
            this.List = list;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Add ( T item )
        {
            this.List.Add ( item );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForCount );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForIndexer );
            this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Add, item ) );
        }

        public void Insert ( Int32 index, T item )
        {
            this.List.Insert ( index, item );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForCount );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForIndexer );
            this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Add, item, index ) );
        }

        public void Clear ( )
        {
            this.List.Clear ( );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForCount );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForIndexer );
            this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Reset, this ) );
        }

        public void RemoveAt ( Int32 index )
        {
            T removed = this.List[index];
            this.List.RemoveAt ( index );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForCount );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForIndexer );
            this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Remove, removed, index ) );
        }

        public Boolean Remove ( T item )
        {
            var result = this.List.Remove ( item );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForCount );
            this.PropertyChanged?.Invoke ( this, PropertyChangedEventArgsForIndexer );
            this.CollectionChanged?.Invoke ( this, new NotifyCollectionChangedEventArgs ( NotifyCollectionChangedAction.Remove, item ) );
            return result;
        }

        public Int32 IndexOf ( T item ) =>
            this.List.IndexOf ( item );

        public Boolean Contains ( T item ) =>
            this.List.Contains ( item );

        public void CopyTo ( T[] array, Int32 arrayIndex ) =>
            this.List.CopyTo ( array, arrayIndex );

        public IEnumerator<T> GetEnumerator ( ) =>
            this.List.GetEnumerator ( );

        IEnumerator IEnumerable.GetEnumerator ( ) =>
            this.List.GetEnumerator ( );
    }
}
