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
using System.Linq.Expressions;
using System.Reflection;

namespace GUtils.Windows.WPF.MVVM
{
    public class DynamicViewModelBuilder<T> where T : class, new()
    {
        private static PropertyInfo GetPropertyInfoFromExpression<TProperty> ( Expression<Func<T, TProperty>> propertySelector )
        {
            if ( !( propertySelector.Body is MemberExpression memberExpression ) || !( memberExpression.Member is PropertyInfo propertyInfo )
                || ( propertySelector.Parameters[0] != memberExpression.Expression ) )
                throw new ArgumentException ( "Provided expression does not represent a non-nested property acess" );

            return propertyInfo;
        }

        private IDictionary<PropertyInfo, DynamicViewModelPropertyInformation> Properties { get; } =
            new Dictionary<PropertyInfo, DynamicViewModelPropertyInformation> ( );

        internal DynamicViewModelBuilder ( )
        {
        }

        public DynamicViewModelBuilder<T> Property<TProperty> ( Expression<Func<T, TProperty>> propertySelector )
        {
            PropertyInfo pinfo = GetPropertyInfoFromExpression ( propertySelector );
            this.Properties[pinfo] = new DynamicViewModelPropertyInformation ( pinfo );

            return this;
        }

        public DynamicViewModelBuilder<T> Collection<TProperty, TItems> ( Expression<Func<T, TProperty>> propertySelector, Boolean shouldProxyItems )
            where TProperty : ICollection<TItems>
        {
            PropertyInfo pinfo = GetPropertyInfoFromExpression ( propertySelector );
            this.Properties[pinfo] = new DynamicViewModelPropertyInformation ( pinfo, shouldProxyItems );

            return this;
        }

        internal Type GetModelType ( ) =>
            typeof ( T );

        internal IEnumerable<DynamicViewModelPropertyInformation> GetPropertiesInformation ( ) =>
            this.Properties.Values;
    }
}
