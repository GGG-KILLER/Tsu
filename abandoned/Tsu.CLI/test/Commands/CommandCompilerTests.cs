// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Diagnostics.CodeAnalysis;
using Tsu.CLI.Commands;
using Tsu.CLI.Commands.Errors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// We need backwards compatibility so disable obsolete stuff warnings
#pragma warning disable CS0618

namespace Tsu.CLI.Tests.Commands
{
    [TestClass]
    public class CommandCompilerTests
    {
        public enum RandomEnum
        {
            Enum01,
            Enum02
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Static field.")]
        private static object s_value;

        public static void DoSomething01(uint u32) => s_value = u32;

        public static void DoSomething02(int i32) => s_value = i32;

        public static void DoSomething03(RandomEnum @enum) => s_value = @enum;

        public static void DoSomething04(double f64) => s_value = f64;

        public static void DoSomething05(float f32) => s_value = f32;

        public static void DoSomething06(string first, [JoinRestOfArguments] string rest) => s_value = first + "." + rest;

        public static void DoSomething07(params string[] args) => s_value = string.Join(".", args);

        public static void DoSomething08(double? opt = null) => s_value = opt;

        public static void DoSomething09(params RandomEnum[] enums) => s_value = string.Join(".", enums);

        [DataTestMethod]
        [DataRow(nameof(DoSomething01))]
        [DataRow(nameof(DoSomething02))]
        [DataRow(nameof(DoSomething03))]
        [DataRow(nameof(DoSomething04))]
        [DataRow(nameof(DoSomething05))]
        [DataRow(nameof(DoSomething06))]
        [DataRow(nameof(DoSomething07))]
        [DataRow(nameof(DoSomething08))]
        [DataRow(nameof(DoSomething09))]
        public void ShouldCompile(string methodName)
        {
            var method = typeof(CommandCompilerTests).GetMethod(methodName)!;
            CommandCompiler.Compile(method, null);
        }

        [DataTestMethod]
        [DataRow(nameof(DoSomething01), "1252121", 1252121U)]
        [DataRow(nameof(DoSomething02), "-255", -255)]
        [DataRow(nameof(DoSomething03), "enum01", RandomEnum.Enum01)]
        [DataRow(nameof(DoSomething03), "EnUm02", RandomEnum.Enum02)]
        [DataRow(nameof(DoSomething04), "123.00000001", 123.00000001d)]
        [DataRow(nameof(DoSomething05), "125.215", 125.215f)]
        [DataRow(nameof(DoSomething06), "a;b;c;d;e;f;g", "a.bcdefg")]
        [DataRow(nameof(DoSomething07), "a;b;c;d;e;f;g", "a.b.c.d.e.f.g")]
        [DataRow(nameof(DoSomething08), "-2", -2D)]
        [DataRow(nameof(DoSomething08), "", null)]
        [DataRow(nameof(DoSomething09), "enum01", "Enum01")]
        [DataRow(nameof(DoSomething09), "enum01;enum01", "Enum01.Enum01")]
        [DataRow(nameof(DoSomething09), "enum01;enum02;enum01", "Enum01.Enum02.Enum01")]
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Inputs are known")]
        [SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "Can't specify it without breaking it on some TFMs.")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Applicable to some TFMs.")]
        public void CompiledCommandShouldRun(string methodName, string inputString, object expectedVal)
        {
            var method = typeof(CommandCompilerTests).GetMethod(methodName);
            var comp = CommandCompiler.Compile(method, null);

            s_value = null;
            comp(string.Empty, inputString.Contains(";")
                ? inputString.Split(';')
                : (inputString.Length < 1 ? Array.Empty<string>() : new[] { inputString }));
            Assert.AreEqual(expectedVal, s_value);
        }

        [DataTestMethod]
        [DataRow(nameof(DoSomething01), "EFG")]
        [DataRow(nameof(DoSomething02), "ABC")]
        [DataRow(nameof(DoSomething03), "enum03")]
        [DataRow(nameof(DoSomething04), "0x2.p4")]
        [DataRow(nameof(DoSomething05), "0x2.p2")]

        // DoSomething06 and DoSomething07 would only fail for lack of arguments
        public void CompiledCommandShouldThrowOnConversionFail(string methodName, string inputString)
        {
            var method = typeof(CommandCompilerTests).GetMethod(methodName);
            var comp = CommandCompiler.Compile(method, null);
            Assert.ThrowsException<CommandInvocationException>(()
              => comp(string.Empty, new[] { inputString }));
        }

        [DataTestMethod]
        [DataRow(nameof(DoSomething01))]
        [DataRow(nameof(DoSomething02))]
        [DataRow(nameof(DoSomething03))]
        [DataRow(nameof(DoSomething04))]
        [DataRow(nameof(DoSomething05))]
        [DataRow(nameof(DoSomething06))]
        public void CompiledCommandShouldThrowOnLackOfArguments(string methodName)
        {
            var method = typeof(CommandCompilerTests).GetMethod(methodName);
            var comp = CommandCompiler.Compile(method, null);
            Assert.ThrowsException<CommandInvocationException>(()
              => comp(string.Empty, Array.Empty<string>()));
        }
    }
}
