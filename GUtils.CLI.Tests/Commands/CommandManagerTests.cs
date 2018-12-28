/*
 * Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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
using GUtils.CLI.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

// We need backwards compatibility so disable obsolete stuff warnings
#pragma warning disable CS0618

namespace GUtils.CLI.Tests.Commands
{
    [TestClass]
    public class CommandManagerTests
    {
        #region Raw Command Test

        private class RawCommand
        {
            public static String RawValue { get; private set; }

            [RawInput]
            [Command ( "raw-command" )]
            [HelpDescription ( "Raw Command" )]
            public static void SetRawValue ( [HelpDescription ( "The raw value" )] String value ) => RawValue = value;
        }

        [DataTestMethod]
        [DataRow ( "'command in'put" )]
        [DataRow ( "'c''o''m''m''a''n''d'' ''i''n''p''u''t'" )]
        [DataRow ( "r:command input" )]
        [DataRow ( @"""\b1100011\b1101111\x6d\o155\o141\o156\100\b100000\105\x6e\o160\o165\x74""" )]
        public void ShouldExecuteRawCommandsProperly ( String input )
        {
            var man = new CommandManager ( );
            man.LoadCommands<RawCommand> ( null );
            Logger.LogMessage ( $"Commands: {String.Join ( ", ", man.Commands )}" );

            man.Execute ( $"raw-command {input}" );
            Assert.AreEqual ( input, RawCommand.RawValue );
        }

        #endregion Raw Command Test

        #region Normal Command Test

        private class NormalCommand
        {
            public static (String, Int32, UInt32) Args { get; private set; }

            [Command ( "normal-command" )]
            [HelpDescription ( "Normal command." )]
            public static void SetValues ( String a, Int32 b, UInt32 c ) => Args = (a, b, c);
        }

        [DataTestMethod]
        [DataRow ( "\"hello world\" -123 123", "hello world", -123, 123U )]
        [DataRow ( "hello 321 4294967295", "hello", 321, 4294967295 )]
        public void ShouldExecuteNormalCommandsProperly ( String input, String a, Int32 b, UInt32 c )
        {
            var man = new CommandManager ( );
            man.LoadCommands<NormalCommand> ( null );
            Logger.LogMessage ( $"Commands: {String.Join ( ", ", man.Commands )}" );

            man.Execute ( $"normal-command {input}" );
            Assert.AreEqual ( a, NormalCommand.Args.Item1 );
            Assert.AreEqual ( b, NormalCommand.Args.Item2 );
            Assert.AreEqual ( c, NormalCommand.Args.Item3 );
        }

        #endregion Normal Command Test

        #region Help Command Test

        private class HelpTextClass
        {
            [Command ( "command-with-help" )]
            [HelpDescription ( "This command has a help text" )]
            [HelpExample ( "command-with-help 1 -2 3 4 5" )]
            public static Int32 DoAbsolutelyNothing01 (
                [HelpDescription ( "First value" )] Int32 a,
                Int32 b,
                [HelpDescription ( "All other values" )] params String[] c ) => a + b + c.Length;

            [Command ( "second-command-with-help" )]
            public static Boolean DoAbsolutelyNothing02 (
                [HelpDescription ( "First attribute" )] Int32 a,
                [JoinRestOfArguments] String b
            ) => a.ToString ( ) == b;

            [Command ( "third-command-with-help" )]
            public static Boolean DoAbsolutelyNothing03 (
                [HelpDescription ( "First attribute" )] Int32 a,
                [JoinRestOfArguments] String b = null
            ) => a.ToString ( ) == b;
        }

        [TestMethod]
        public void ShouldPrintProperHelpText ( )
        {
            var man = new CommandManager ( );
            var help = new TestHelpCommand ( man );
            man.LoadCommands<HelpTextClass> ( null );
            man.AddHelpCommand ( help );

            help.AddLine ( "Showing help for all commands:" );
            help.AddLine ( "    command-with-help - This command has a help text" );
            help.AddLine ( "        Usage:" );
            help.AddLine ( "            command-with-help a b [c...]" );
            help.AddLine ( "        Arguments:" );
            help.AddLine ( "            a:Int32    - First value" );
            help.AddLine ( "            b:Int32    - No description was provided for this argument." );
            help.AddLine ( "            c:String[] - All other values" );
            help.AddLine ( "        Examples:" );
            help.AddLine ( "            command-with-help 1 -2 3 4 5" );
            help.AddLine ( "    second-command-with-help - No description was provided for this command." );
            help.AddLine ( "        Usage:" );
            help.AddLine ( "            second-command-with-help a b..." );
            help.AddLine ( "        Arguments:" );
            help.AddLine ( "            a:Int32  - First attribute" );
            help.AddLine ( "            b:String - No description was provided for this argument." );
            help.AddLine ( "    third-command-with-help - No description was provided for this command." );
            help.AddLine ( "        Usage:" );
            help.AddLine ( "            third-command-with-help a [b...]" );
            help.AddLine ( "        Arguments:" );
            help.AddLine ( "            a:Int32  - First attribute" );
            help.AddLine ( "            b:String - No description was provided for this argument." );
            help.AddLine ( "    help - Shows help text" );
            help.AddLine ( "        Usage:" );
            help.AddLine ( "            help [commandName]" );
            help.AddLine ( "        Arguments:" );
            help.AddLine ( "            commandName:String - name of the command to get the help text" );
            help.AddLine ( "        Examples:" );
            help.AddLine ( "            help      (will list all commands)" );
            help.AddLine ( "            help help (will show the help text for this command)" );
            man.Execute ( "help" );

            Assert.AreEqual ( 0, help.ExpectedWritesQueue.Count );
        }

        #endregion Help Command Test

        #region Verb Command Test

        private class VerbCommands
        {
            public static Object Something { get; set; }

            [Command ( "a" )]
            public static void Command01 ( String a, Int32 b ) =>
                Something = (a, b);

            [Command ( "b" )]
            public static void Command02 ( Int32 a, Int32 b ) =>
                Something = (a, b);
        }

        [DataTestMethod]
        [DataRow("verb0 a 'ab' 2", "ab", 2)]
        [DataRow("verb0 b 1 2", 1, 2)]
        [DataRow("verb0 verb1 a 'cd' 3", "cd", 3)]
        [DataRow("verb0 verb1 b 2 3", 2, 3)]
        [DataRow("verb0 verb1 verb2 a 'ef' 4", "ef", 4)]
        [DataRow("verb0 verb1 verb2 b 3 4", 3, 4)]
        public void VerbsShouldWork ( String line, Object a, Object b )
        {
            var root = new CommandManager ( );
            CommandManager verb0 = root.AddVerb ( "verb0" );
            CommandManager verb1 = verb0.AddVerb ( "verb1" );
            CommandManager verb2 = verb1.AddVerb ( "verb2" );
            verb0.LoadCommands<VerbCommands> ( null );
            verb1.LoadCommands<VerbCommands> ( null );
            verb2.LoadCommands<VerbCommands> ( null );

            VerbCommands.Something = "fail";
            root.Execute ( line );
            Assert.AreEqual ( (a, b), VerbCommands.Something );
        }

        #endregion
    }
}
