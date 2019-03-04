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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace GUtils.Windows.WPF.MVVM
{
    public static class DynamicViewModelCreator
    {
        /// <summary>
        /// The <see cref="AssemblyBuilder"/> used by this
        /// </summary>
        private static readonly AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly (
            new AssemblyName ( "GUtils.Windows.WPF.MVVM.DynamicViewModels" ),
            AssemblyBuilderAccess.RunAndCollect );

        /// <summary>
        /// The <see cref="ModuleBuilder"/> used by this
        /// </summary>
        private static readonly ModuleBuilder moduleBuilder = assemblyBuilder.GetDynamicModule ( "GUtils.Windows.WPF.MVVM.DynamicViewModels" );

        /// <summary>
        /// The ViewModels cache
        /// </summary>
        private static readonly IDictionary<Type, Type> dynamicProxyTypes = new Dictionary<Type, Type> ( );

        /// <summary>
        /// Gets a type builder for a given type
        /// </summary>
        /// <param name="base"></param>
        /// <returns></returns>
        private static TypeBuilder GetTypeBuilder ( Type @base ) =>
            moduleBuilder.DefineType (
                $"DynamicViewModel_{@base.GetHashCode ( ):X}",
                TypeAttributes.Public | TypeAttributes.Class,
                @base,
                new[] { typeof ( INotifyPropertyChanged ) } );

        private static void BuildPropertyProxy ( TypeBuilder typeBuilder, PropertyInfo propertyInfo )
        {
            Type propertyType = propertyInfo.PropertyType;
            Type[] genericArgs = propertyType.GetGenericArguments ( );
            // If we have an ICollection<T>, then proxy it through an ObservableCollection<T>
            if ( genericArgs.Length == 1 )
            {
                if ( typeof ( IEnumerable<> ).MakeGenericType ( genericArgs[0] ).IsAssignableFrom ( propertyType ) )
                    throw new NotSupportedException ( "IEnumerable<T> properties are not supported because ObservableCollection<T>" );
            }
        }

        private static Type BuildDynamicViewModel ( Type @base, IEnumerable<PropertyInfo> propertyInfos )
        {
            TypeBuilder typeBuilder = GetTypeBuilder ( @base );
            FieldBuilder model = typeBuilder.DefineField ( "model", @base, FieldAttributes.Private | FieldAttributes.InitOnly );
            typeBuilder.DefineEvent ( "PropertyChanged", EventAttributes.None, typeof ( PropertyChangedEventHandler ) );

            #region OnPropertyChanged

            MethodBuilder onPropertyChanged = typeBuilder.DefineMethod (
                "OnPropertyChanged",
                MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.HideBySig,
                typeof ( void ),
                new[] { typeof ( String ) } );
            FieldInfo propertyChanged = typeBuilder.GetField ( "PropertyChanged" );
            ILGenerator ilgen = onPropertyChanged.GetILGenerator ( );
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Ldfld, propertyChanged );
            ilgen.Emit ( OpCodes.Dup );
            Label lblHasEvent = ilgen.DefineLabel ( );
            ilgen.Emit ( OpCodes.Brtrue_S, lblHasEvent );

            ilgen.Emit ( OpCodes.Pop );
            ilgen.Emit ( OpCodes.Ret );
            ilgen.MarkLabel ( lblHasEvent );

            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Ldarg_1 );
            ilgen.Emit ( OpCodes.Newobj, typeof ( PropertyChangedEventArgs ).GetConstructor ( new[] { typeof ( String ) } ) );
            ilgen.Emit ( OpCodes.Callvirt, typeof ( PropertyChangedEventHandler ).GetMethod ( "Invoke" ) );
            ilgen.Emit ( OpCodes.Ret );

            #endregion OnPropertyChanged

            foreach ( PropertyInfo propertyInfo in propertyInfos )
            {
                PropertyBuilder propBuilder = typeBuilder.DefineProperty (
                    propertyInfo.Name,
                    PropertyAttributes.None,
                    propertyInfo.PropertyType,
                    Type.EmptyTypes );

                MethodBuilder getter = typeBuilder.DefineMethod (
                    propertyInfo.Name + "_get",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                    propertyInfo.PropertyType,
                    Type.EmptyTypes );
                ilgen = getter.GetILGenerator ( );
                ilgen.Emit ( OpCodes.Ldfld, model );
                ilgen.Emit ( OpCodes.Call, propertyInfo.GetGetMethod ( ) );
                ilgen.Emit ( OpCodes.Ret );

                MethodBuilder setter = typeBuilder.DefineMethod (
                    propertyInfo.Name + "_set",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                    typeof ( void ),
                    new[] { propertyInfo.PropertyType } );
            }

            return typeBuilder.CreateType ( );
        }

        public static INotifyPropertyChanged GenerateProxy ( Type type, Func<PropertyInfo, Boolean> propertyFilter = null )
        {
            if ( !type.IsClass )
                throw new ArgumentException ( "The type provided is not a class type.", nameof ( type ) );

            if ( !dynamicProxyTypes.TryGetValue ( type, out Type viewModelType ) )
            {
                PropertyInfo[] properties = type.GetProperties ( BindingFlags.Public | BindingFlags.Instance )
                    .Where ( p => p.GetCustomAttribute<NotProxiedAttribute> ( ) != null )
                    .Where ( propertyFilter ?? (p => true) )
                    .ToArray ( );

                dynamicProxyTypes[type] =
                    viewModelType = BuildDynamicViewModel ( type, properties );
            }
        }

        public static T GenerateProxy<T> ( ) where T : class =>
            GenerateProxy ( typeof ( T ) ) as T;
    }
}
