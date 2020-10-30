/*
 * Copyright © 2020 GGG KILLER <gggkiller2@gmail.com>
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tsu.Testing.Tests
{
    public partial class DelegateHelpersTests
    {
        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action> counter = DelegateHelpers.TrackInvocationCount ( ( ) =>
            {
                callCount++;
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }

        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction01 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String>> counter = DelegateHelpers.TrackInvocationCount<String> ( ( arg1 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction02 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String> ( ( arg1, arg2 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction03 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String> ( ( arg1, arg2, arg3 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction04 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String> ( ( arg1, arg2, arg3, arg4 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction05 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String> ( ( arg1, arg2, arg3, arg4, arg5 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction06 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String> ( ( arg1, arg2, arg3, arg4, arg5, arg6 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction07 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction08 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String, String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction09 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String, String, String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction10 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String, String, String, String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction11 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String, String, String, String, String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, String> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                Assert.AreEqual ( "arg11", arg11 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10", "arg11" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction12 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String, String, String, String, String, String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, String, String> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                Assert.AreEqual ( "arg11", arg11 );
                Assert.AreEqual ( "arg12", arg12 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10", "arg11", "arg12" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction13 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String, String, String, String, String, String, String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, String, String, String> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                Assert.AreEqual ( "arg11", arg11 );
                Assert.AreEqual ( "arg12", arg12 );
                Assert.AreEqual ( "arg13", arg13 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10", "arg11", "arg12", "arg13" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction14 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String, String, String, String, String, String, String, String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, String, String, String, String> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                Assert.AreEqual ( "arg11", arg11 );
                Assert.AreEqual ( "arg12", arg12 );
                Assert.AreEqual ( "arg13", arg13 );
                Assert.AreEqual ( "arg14", arg14 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10", "arg11", "arg12", "arg13", "arg14" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction15 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String, String, String, String, String, String, String, String, String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, String, String, String, String, String> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                Assert.AreEqual ( "arg11", arg11 );
                Assert.AreEqual ( "arg12", arg12 );
                Assert.AreEqual ( "arg13", arg13 );
                Assert.AreEqual ( "arg14", arg14 );
                Assert.AreEqual ( "arg15", arg15 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10", "arg11", "arg12", "arg13", "arg14", "arg15" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForAction16 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Action<String, String, String, String, String, String, String, String, String, String, String, String, String, String, String, String>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, String, String, String, String, String, String> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                Assert.AreEqual ( "arg11", arg11 );
                Assert.AreEqual ( "arg12", arg12 );
                Assert.AreEqual ( "arg13", arg13 );
                Assert.AreEqual ( "arg14", arg14 );
                Assert.AreEqual ( "arg15", arg15 );
                Assert.AreEqual ( "arg16", arg16 );
            });

            for ( var i = 1; i < 6; i++ )
            {
                counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10", "arg11", "arg12", "arg13", "arg14", "arg15", "arg16" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
            }

            counter.Reset ( );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<Int32>> counter = DelegateHelpers.TrackInvocationCount<Int32> ( ( ) =>
            {
                callCount++;
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }

        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc01 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, Int32> ( ( arg1 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc02 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, Int32> ( ( arg1, arg2 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc03 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, Int32> ( ( arg1, arg2, arg3 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc04 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc05 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4, arg5 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc06 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4, arg5, arg6 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc07 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc08 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc09 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, String, String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc10 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, String, String, String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc11 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, String, String, String, String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                Assert.AreEqual ( "arg11", arg11 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10", "arg11" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc12 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, String, String, String, String, String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                Assert.AreEqual ( "arg11", arg11 );
                Assert.AreEqual ( "arg12", arg12 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10", "arg11", "arg12" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc13 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, String, String, String, String, String, String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                Assert.AreEqual ( "arg11", arg11 );
                Assert.AreEqual ( "arg12", arg12 );
                Assert.AreEqual ( "arg13", arg13 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10", "arg11", "arg12", "arg13" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc14 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, String, String, String, String, String, String, String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                Assert.AreEqual ( "arg11", arg11 );
                Assert.AreEqual ( "arg12", arg12 );
                Assert.AreEqual ( "arg13", arg13 );
                Assert.AreEqual ( "arg14", arg14 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10", "arg11", "arg12", "arg13", "arg14" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc15 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, String, String, String, String, String, String, String, String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, String, String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                Assert.AreEqual ( "arg11", arg11 );
                Assert.AreEqual ( "arg12", arg12 );
                Assert.AreEqual ( "arg13", arg13 );
                Assert.AreEqual ( "arg14", arg14 );
                Assert.AreEqual ( "arg15", arg15 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10", "arg11", "arg12", "arg13", "arg14", "arg15" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }


        [TestMethod]
        public void TrackInvocationCountImplementedCorrectlyForFunc16 ( )
        {
            var callCount = 0;
            DelegateInvocationCounter<Func<String, String, String, String, String, String, String, String, String, String, String, String, String, String, String, String, Int32>> counter = DelegateHelpers.TrackInvocationCount<String, String, String, String, String, String, String, String, String, String, String, String, String, String, String, String, Int32> ( ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 ) =>
            {
                callCount++;
                Assert.AreEqual ( "arg1", arg1 );
                Assert.AreEqual ( "arg2", arg2 );
                Assert.AreEqual ( "arg3", arg3 );
                Assert.AreEqual ( "arg4", arg4 );
                Assert.AreEqual ( "arg5", arg5 );
                Assert.AreEqual ( "arg6", arg6 );
                Assert.AreEqual ( "arg7", arg7 );
                Assert.AreEqual ( "arg8", arg8 );
                Assert.AreEqual ( "arg9", arg9 );
                Assert.AreEqual ( "arg10", arg10 );
                Assert.AreEqual ( "arg11", arg11 );
                Assert.AreEqual ( "arg12", arg12 );
                Assert.AreEqual ( "arg13", arg13 );
                Assert.AreEqual ( "arg14", arg14 );
                Assert.AreEqual ( "arg15", arg15 );
                Assert.AreEqual ( "arg16", arg16 );
                return 12 + callCount;
            });

            for ( var i = 1; i < 6; i++ )
            {
                var ret = counter.WrappedDelegate ( "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7", "arg8", "arg9", "arg10", "arg11", "arg12", "arg13", "arg14", "arg15", "arg16" );

                Assert.AreEqual ( i, counter.InvocationCount );
                Assert.AreEqual ( i, callCount );
                Assert.AreEqual ( 12 + i, ret );
            }
        }

    }
}