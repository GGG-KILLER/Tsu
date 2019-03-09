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
using System.Reflection;
using System.Reflection.Emit;

namespace GUtils.Windows.WPF.MVVM
{
    internal class DynamicBindableModelCompiler
    {
        private static readonly MethodInfo MI_Delegate_Combine = typeof ( Delegate )
            .GetMethod ( "Combine", new[] { typeof ( Delegate ), typeof ( Delegate ) } );

        private static readonly MethodInfo MI_Delegate_Remove = typeof ( Delegate )
            .GetMethod ( "Remove", new[] { typeof ( Delegate ), typeof ( Delegate ) } );

        private static (PropertyInfo, MethodInfo) GetEqualityComparer ( Type type )
        {
            Type equalityComparer = typeof ( EqualityComparer<> ).MakeGenericType ( type );
            return (equalityComparer.GetProperty ( "Default" ), equalityComparer.GetMethod ( "Equals", new[] { type, type } ));
        }

        private static String GetBackingFieldName ( String propertyName ) =>
            String.Concat ( "<", propertyName, ">k__BackingField" );

        private readonly DynamicBindableModelFactory modelFactory;
        private readonly Type modelType;
        private readonly TypeBuilder typeBuilder;
        private readonly FieldBuilder model;
        private readonly FieldBuilder propertyChanged;
        private readonly MethodBuilder onPropertyChanged;
        private readonly Boolean finished;

        public Type Type { get; }

        public DynamicBindableModelCompiler ( DynamicBindableModelFactory modelFactory, ModuleBuilder moduleBuilder, Type modelType )
        {
            ILGenerator ilgen;

            this.modelFactory = modelFactory;
            this.modelType = modelType;
            this.typeBuilder = moduleBuilder.DefineType (
                $"DynamicBindableModel_{modelType.GetHashCode ( ):X}",
                TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit,
                modelType,
                new[] { typeof ( INotifyPropertyChanged ) } );

            this.Type = this.typeBuilder;

            this.model = this.typeBuilder.DefineField (
                "model",
                modelType,
                FieldAttributes.Private | FieldAttributes.InitOnly | FieldAttributes.SpecialName );

            #region PropertyChanged

            this.propertyChanged = this.typeBuilder.DefineField (
                    "PropertyChanged",
                    typeof ( PropertyChangedEventHandler ),
                    FieldAttributes.Private );

            EventBuilder propertyChangedEvent = this.typeBuilder.DefineEvent (
                "PropertyChanged",
                EventAttributes.None,
                typeof ( PropertyChangedEventHandler ) );

            MethodBuilder propertyChangedAdd = this.typeBuilder.DefineMethod (
                "PropertyChanged_add",
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
                typeof ( void ),
                new[] { typeof ( PropertyChangedEventHandler ) } );
            ilgen = propertyChangedAdd.GetILGenerator ( );
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Ldfld, this.propertyChanged );
            ilgen.Emit ( OpCodes.Ldarg_1 );
            ilgen.Emit ( OpCodes.Call, MI_Delegate_Combine );
            ilgen.Emit ( OpCodes.Castclass, typeof ( PropertyChangedEventHandler ) );
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Stfld, this.propertyChanged );
            ilgen.Emit ( OpCodes.Ret );
            propertyChangedEvent.SetAddOnMethod ( propertyChangedAdd );

            MethodBuilder propertyChangedRemove = this.typeBuilder.DefineMethod (
                "PropertyChanged_remove",
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
                typeof ( void ),
                new[] { typeof ( PropertyChangedEventHandler ) } );
            ilgen = propertyChangedRemove.GetILGenerator ( );
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Ldfld, this.propertyChanged );
            ilgen.Emit ( OpCodes.Ldarg_1 );
            ilgen.Emit ( OpCodes.Call, MI_Delegate_Remove );
            ilgen.Emit ( OpCodes.Castclass, typeof ( PropertyChangedEventHandler ) );
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Stfld, this.propertyChanged );
            ilgen.Emit ( OpCodes.Ret );
            propertyChangedEvent.SetRemoveOnMethod ( propertyChangedRemove );

            this.onPropertyChanged = this.typeBuilder.DefineMethod (
                "OnPropertyChanged",
                MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.HideBySig,
                typeof ( void ),
                new[] { typeof ( String ) } );
            ilgen = this.onPropertyChanged.GetILGenerator ( );
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Ldfld, this.propertyChanged );
            ilgen.Emit ( OpCodes.Dup );
            Label lblContinue = ilgen.DefineLabel ( );
            ilgen.Emit ( OpCodes.Brtrue_S, lblContinue );

            ilgen.Emit ( OpCodes.Pop );
            ilgen.Emit ( OpCodes.Ret );

            ilgen.MarkLabel ( lblContinue );
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Ldarg_1 );
            ilgen.Emit ( OpCodes.Newobj, typeof ( PropertyChangedEventArgs ).GetConstructor ( new[] { typeof ( String ) } ) );
            ilgen.Emit ( OpCodes.Callvirt, typeof ( PropertyChangedEventHandler ).GetMethod ( "Invoke" ) );
            ilgen.Emit ( OpCodes.Ret );
            propertyChangedEvent.SetRaiseMethod ( this.onPropertyChanged );

            #endregion PropertyChanged

            #region Constructor

            ConstructorInfo parentCtor = modelType.GetConstructor ( Type.EmptyTypes );
            ConstructorBuilder ctor = this.typeBuilder.DefineConstructor (
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new[] { modelType } );
            ilgen = ctor.GetILGenerator ( );
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Call, parentCtor );
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Ldarg_1 );
            ilgen.Emit ( OpCodes.Stfld, this.model );
            ilgen.Emit ( OpCodes.Ret );

            #endregion Constructor
        }

        public void RegisterNormalProperty ( PropertyInfo oldProperty )
        {
            PropertyBuilder property = this.typeBuilder.DefineProperty (
                    oldProperty.Name,
                    PropertyAttributes.None,
                    oldProperty.PropertyType,
                    Type.EmptyTypes );

            MethodInfo oldGetter = oldProperty.GetGetMethod ( true );
            MethodBuilder getter = this.typeBuilder.DefineMethod (
                    oldGetter.Name,
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                    oldProperty.PropertyType,
                    Type.EmptyTypes );
            this.typeBuilder.DefineMethodOverride ( getter, oldGetter );
            ILGenerator ilgen = getter.GetILGenerator ( );
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Ldfld, this.model );
            ilgen.Emit ( OpCodes.Call, oldGetter );
            ilgen.Emit ( OpCodes.Ret );

            MethodInfo oldSetter = oldProperty.GetSetMethod ( true );
            MethodBuilder setter = this.typeBuilder.DefineMethod (
                    oldSetter.Name,
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                    typeof ( void ),
                    new[] { oldProperty.PropertyType } );
            this.typeBuilder.DefineMethodOverride ( setter, oldSetter );
            (PropertyInfo defaultProperty, MethodInfo equalsMethod) = GetEqualityComparer ( oldProperty.PropertyType );
            ilgen = setter.GetILGenerator ( );
            ilgen.Emit ( OpCodes.Call, defaultProperty.GetGetMethod ( ) );
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Call, oldGetter );
            ilgen.Emit ( OpCodes.Ldarg_1 );
            ilgen.Emit ( OpCodes.Callvirt, equalsMethod );
            Label propertyUnchangedLabel = ilgen.DefineLabel ( );
            ilgen.Emit ( OpCodes.Brtrue_S, propertyUnchangedLabel );

            // if propval != value then {
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Ldarg_1 );
            ilgen.Emit ( OpCodes.Call, oldSetter );
            ilgen.Emit ( OpCodes.Ldarg_0 );
            ilgen.Emit ( OpCodes.Ldstr, oldProperty.Name );

            // }
            ilgen.MarkLabel ( propertyUnchangedLabel );
            ilgen.Emit ( OpCodes.Ret );
        }

        public void RegisterListProperty ( PropertyInfo propertyInfo )
        {
        }

        public void RegisterCollectionProperty ( PropertyInfo propertyInfo )
        {
        }

        public void RegisterNormalPropertyWithProxy ( PropertyInfo propertyInfo )
        {
        }

        public void RegisterListPropertyWithProxy ( PropertyInfo propertyInfo )
        {
        }

        public void RegisterCollectionPropertyWithProxy ( PropertyInfo propertyInfo )
        {
        }
    }
}
