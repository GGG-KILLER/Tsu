using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tsu.MVVM
{
    /// <summary>
    /// Implements a few utility functions for a ViewModel base
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Does the logic for calling <see cref="PropertyChanged"/> if the value has changed (and
        /// also sets the value of the field)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        [MethodImpl ( MethodImplOptions.NoInlining )]
        protected virtual void SetField<T> ( ref T field, T newValue, [CallerMemberName] String propertyName = null )
        {
            if ( EqualityComparer<T>.Default.Equals ( field, newValue ) )
                return;
            field = newValue;
            this.OnPropertyChanged ( propertyName );
        }

        /// <summary>
        /// Invokes <see cref="PropertyChanged"/>
        /// </summary>
        /// <param name="propertyName"></param>
        /// <exception cref="ArgumentNullException">
        /// Invoked if a <paramref name="propertyName"/> is not provided (and the value isn't
        /// auto-filled by the compiler)
        /// </exception>
        [MethodImpl ( MethodImplOptions.NoInlining )]
        protected virtual void OnPropertyChanged ( [CallerMemberName] String propertyName = null ) =>
            this.PropertyChanged?.Invoke (
                this,
                new PropertyChangedEventArgs ( propertyName ?? throw new ArgumentNullException ( nameof ( propertyName ) ) ) );
    }
}