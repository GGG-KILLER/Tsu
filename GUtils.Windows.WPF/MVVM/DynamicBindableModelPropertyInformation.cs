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
using System.Reflection;

namespace GUtils.Windows.WPF.MVVM
{
    internal readonly struct DynamicBindableModelPropertyInformation : IEquatable<DynamicBindableModelPropertyInformation>
    {
        public PropertyInfo PropertyInfo { get; }

        public Boolean IsCollection { get; }

        public Boolean ShouldBeProxied { get; }

        public DynamicBindableModelPropertyInformation ( PropertyInfo propertyInfo, Boolean isCollection, Boolean shouldBeProxied )
        {
            this.PropertyInfo    = propertyInfo;
            this.IsCollection    = isCollection;
            this.ShouldBeProxied = shouldBeProxied;
        }

        #region Generated Code

        public override Boolean Equals ( Object obj ) =>
            obj is DynamicBindableModelPropertyInformation && this.Equals ( ( DynamicBindableModelPropertyInformation ) obj );

        public Boolean Equals ( DynamicBindableModelPropertyInformation other ) =>
            EqualityComparer<PropertyInfo>.Default.Equals ( this.PropertyInfo, other.PropertyInfo )
            && this.IsCollection == other.IsCollection
            && this.ShouldBeProxied == other.ShouldBeProxied;

        public override Int32 GetHashCode ( )
        {
            var hashCode = 1850586761;
            hashCode = hashCode * -1521134295 + EqualityComparer<PropertyInfo>.Default.GetHashCode ( this.PropertyInfo );
            hashCode = hashCode * -1521134295 + this.IsCollection.GetHashCode ( );
            hashCode = hashCode * -1521134295 + this.ShouldBeProxied.GetHashCode ( );
            return hashCode;
        }

        public static Boolean operator == ( DynamicBindableModelPropertyInformation information1, DynamicBindableModelPropertyInformation information2 ) => information1.Equals ( information2 );

        public static Boolean operator != ( DynamicBindableModelPropertyInformation information1, DynamicBindableModelPropertyInformation information2 ) => !( information1 == information2 );

        #endregion Generated Code
    }
}
