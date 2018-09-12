﻿using System;
using System.Reflection;
using GUtils.CLI.Commands;
using GUtils.CLI.Commands.Errors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GUtils.CLI.Tests.Commands
{
    [TestClass]
    public class CommandCompilerTests
    {
        public enum RandomEnum
        {
            Enum01,
            Enum02
        }

        private static Object Value;

        public static void DoSomething01 ( UInt32 u32 ) => Value = u32;

        public static void DoSomething02 ( Int32 i32 ) => Value = i32;

        public static void DoSomething03 ( RandomEnum @enum ) => Value = @enum;

        public static void DoSomething04 ( Double f64 ) => Value = f64;

        public static void DoSomething05 ( Single f32 ) => Value = f32;

        public static void DoSomething06 ( String first, [JoinRestOfArguments] String rest ) => Value = first + "." + rest;

        public static void DoSomething07 ( params String[] args ) => Value = String.Join ( ".", args );

        [DataTestMethod]
        [DataRow ( nameof ( DoSomething01 ) )]
        [DataRow ( nameof ( DoSomething02 ) )]
        [DataRow ( nameof ( DoSomething03 ) )]
        [DataRow ( nameof ( DoSomething04 ) )]
        [DataRow ( nameof ( DoSomething05 ) )]
        [DataRow ( nameof ( DoSomething06 ) )]
        public void ShouldCompile ( String methodName )
        {
            MethodInfo method = typeof ( CommandCompilerTests ).GetMethod ( methodName );
            CommandCompiler.Compile ( method, null );
        }

        [DataTestMethod]
        [DataRow ( nameof ( DoSomething01 ), "1252121", 1252121U )]
        [DataRow ( nameof ( DoSomething02 ), "-255", -255 )]
        [DataRow ( nameof ( DoSomething03 ), "enum01", RandomEnum.Enum01 )]
        [DataRow ( nameof ( DoSomething03 ), "EnUm02", RandomEnum.Enum02 )]
        [DataRow ( nameof ( DoSomething04 ), "123.00000001", 123.00000001d )]
        [DataRow ( nameof ( DoSomething05 ), "125.215", 125.215f )]
        [DataRow ( nameof ( DoSomething06 ), "a;b;c;d;e;f;g", "a.bcdefg" )]
        [DataRow ( nameof ( DoSomething07 ), "a;b;c;d;e;f;g", "a.b.c.d.e.f.g" )]
        public void CompiledCommandShouldRun ( String methodName, String inputString, Object expectedVal )
        {
            MethodInfo method = typeof ( CommandCompilerTests ).GetMethod ( methodName );
            Action<String, String[]> comp = CommandCompiler.Compile ( method, null );

            Value = null;
            comp ( String.Empty, inputString.Contains ( ';' )
                ? inputString.Split ( ';' )
                : new[] { inputString } );
            Assert.AreEqual ( expectedVal, Value );
        }

        [DataTestMethod]
        [DataRow ( nameof ( DoSomething01 ), "EFG" )]
        [DataRow ( nameof ( DoSomething02 ), "ABC" )]
        [DataRow ( nameof ( DoSomething03 ), "enum03" )]
        [DataRow ( nameof ( DoSomething04 ), "0x2.p4" )]
        [DataRow ( nameof ( DoSomething05 ), "0x2.p2" )]
        // DoSomething06 and DoSomething07 would only fail for
        // lack of arguments
        public void CompiledCommandShouldThrowOnConversionFail ( String methodName, String inputString )
        {
            MethodInfo method = typeof ( CommandCompilerTests ).GetMethod ( methodName );
            Action<String, String[]> comp = CommandCompiler.Compile ( method, null );
            Assert.ThrowsException<CommandInvocationException> ( ( )
                => comp ( String.Empty, new[] { inputString } ) );
        }

        [DataTestMethod]
        [DataRow ( nameof ( DoSomething01 ) )]
        [DataRow ( nameof ( DoSomething02 ) )]
        [DataRow ( nameof ( DoSomething03 ) )]
        [DataRow ( nameof ( DoSomething04 ) )]
        [DataRow ( nameof ( DoSomething05 ) )]
        [DataRow ( nameof ( DoSomething06 ) )]
        public void CompiledCommandShouldThrowOnLackOfArguments ( String methodName )
        {
            MethodInfo method = typeof ( CommandCompilerTests ).GetMethod ( methodName );
            Action<String, String[]> comp = CommandCompiler.Compile ( method, null );
            Assert.ThrowsException<CommandInvocationException> ( ( )
                => comp ( String.Empty, Array.Empty<String> ( ) ) );
        }
    }
}
