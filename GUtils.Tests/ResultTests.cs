using System;
using System.Diagnostics.CodeAnalysis;
using GUtils.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GUtils.Tests
{
    [TestClass]
    public class ResultTests
    {
        [ExcludeFromCodeCoverage]
        private readonly struct Box<T> : IEquatable<T> where T : IEquatable<T>
        {
            public T Boxed { get; }

            public Box ( T boxed )
            {
                this.Boxed = boxed;
            }

            public Boolean Equals ( [AllowNull] T other ) => this.Boxed.Equals ( other );
        }

        private const String OkValue = "ok";

        private const Int32 ErrValue = -1;

        private static Result<String, Int32> Ok => Result.Ok<String, Int32> ( OkValue );

        private static Result<String, Int32> Err => Result.Err<String, Int32> ( ErrValue );

        [TestMethod]
        public void IsOkReturnsTrueForOkResult ( )
        {
            Result<String, Int32> ok = Ok;
            Assert.IsTrue ( ok.IsOk );
        }

        [TestMethod]
        public void IsOkReturnsFalseForErrResult ( )
        {
            Result<String, Int32> err = Err;
            Assert.IsFalse ( err.IsOk );
        }

        [TestMethod]
        public void IsErrReturnsFalseForOkResult ( )
        {
            Result<String, Int32> ok = Ok;
            Assert.IsFalse ( ok.IsErr );
        }

        [TestMethod]
        public void IsErrReturnsTrueForErrResult ( )
        {
            Result<String, Int32> err = Err;
            Assert.IsTrue ( err.IsErr );
        }

        [TestMethod]
        public void OkReturnsSomeForOk ( )
        {
            Result<String, Int32> ok = Ok;
            Assert.IsTrue ( ok.Ok.IsSome );
            Assert.AreEqual ( OkValue, ok.Ok.Value );
        }

        [TestMethod]
        public void OkReturnsNoneForErr ( )
        {
            Result<String, Int32> ok = Ok;
            Assert.IsTrue ( ok.Err.IsNone );
        }

        [TestMethod]
        public void ErrReturnsSomeForErr ( )
        {
            Result<String, Int32> err = Err;
            Assert.IsTrue ( err.Err.IsSome );
            Assert.AreEqual ( ErrValue, err.Err.Value );
        }

        [TestMethod]
        public void ErrReturnsNoneForOk ( )
        {
            Result<String, Int32> err = Err;
            Assert.IsTrue ( err.Ok.IsNone );
        }

        [TestMethod]
        public void ContainsReturnsTrueForEqualValueOnOk ( )
        {
            Result<String, Int32> ok = Ok;
            var raw = OkValue;
            var boxed = new Box<String> ( OkValue );

            Assert.IsTrue ( ok.Contains ( raw ) );
            Assert.IsTrue ( ok.Contains ( boxed ) );
        }

        [TestMethod]
        public void ContainsReturnFalseForDifferentValueOnOk ( )
        {
            Result<String, Int32> ok = Ok;
            var wrongRaw = "not " + OkValue;
            var wrongBoxed = new Box<String> ( wrongRaw );

            Assert.IsFalse ( ok.Contains ( wrongRaw ) );
            Assert.IsFalse ( ok.Contains ( wrongBoxed ) );
        }

        [TestMethod]
        public void ContainsReturnsFalseWithAnyValueOnErr ( )
        {
            Result<String, Int32> err = Err;
            var raw = OkValue;
            var boxed = new Box<String> ( OkValue );
            var wrongRaw = "not " + OkValue;
            var wrongBoxed = new Box<String> ( wrongRaw );

            Assert.IsFalse ( err.Contains ( raw ) );
            Assert.IsFalse ( err.Contains ( boxed ) );
            Assert.IsFalse ( err.Contains ( wrongRaw ) );
            Assert.IsFalse ( err.Contains ( wrongBoxed ) );
        }

        [TestMethod]
        public void ContainsErrReturnsTrueForEqualValueOnErr ( )
        {
            Result<String, Int32> err = Err;
            var raw = ErrValue;
            var boxed = new Box<Int32> ( raw );

            Assert.IsTrue ( err.ContainsErr ( raw ) );
            Assert.IsTrue ( err.ContainsErr ( boxed ) );
        }

        [TestMethod]
        public void ContainsErrReturnsFalseForDifferentValueOnErr ( )
        {
            Result<String, Int32> err = Err;
            var wrongRaw = ErrValue * 2;
            var wrongBoxed = new Box<Int32> ( wrongRaw );

            Assert.IsFalse ( err.ContainsErr ( wrongRaw ) );
            Assert.IsFalse ( err.ContainsErr ( wrongBoxed ) );
        }

        [TestMethod]
        public void ContainsErrReturnsFalseForAnyValueOnOk ( )
        {
            Result<String, Int32> ok = Ok;
            var raw = ErrValue;
            var boxed = new Box<Int32> ( raw );
            var wrongRaw = ErrValue * 2;
            var wrongBoxed = new Box<Int32> ( wrongRaw );

            Assert.IsFalse ( ok.ContainsErr ( raw ) );
            Assert.IsFalse ( ok.ContainsErr ( boxed ) );
            Assert.IsFalse ( ok.ContainsErr ( wrongRaw ) );
            Assert.IsFalse ( ok.ContainsErr ( wrongBoxed ) );
        }

        [TestMethod]
        public void MapReturnsInvocationResultForOk ( )
        {
            Result<String, Int32> ok = Ok;
            DelegateInvocationCounter<Func<String, Int32>> mapFuncCounter =
                DelegateHelpers.TrackInvocationCount<String, Int32> ( str => str.Length );

            Result<Int32, Int32> mappedOk = ok.Map ( mapFuncCounter.WrappedDelegate );

            Assert.AreEqual ( 1, mapFuncCounter.InvocationCount );
            Assert.IsTrue ( mappedOk.IsOk );
            Assert.AreEqual ( OkValue.Length, mappedOk.Ok.Value );
        }

        [TestMethod]
        public void MapReturnsErrResultForErr ( )
        {
            Result<String, Int32> err = Err;
            DelegateInvocationCounter<Func<String, Int32>> mapFuncCounter =
                DelegateHelpers.TrackInvocationCount<String, Int32> ( str => str.Length );

            Result<Int32, Int32> mappedErr = err.Map ( mapFuncCounter.WrappedDelegate );

            Assert.AreEqual ( 0, mapFuncCounter.InvocationCount );
            Assert.IsTrue ( mappedErr.IsErr );
            Assert.AreEqual ( ErrValue, mappedErr.Err.Value );
        }

        [TestMethod]
        public void MapOrReturnsInvocationResultForOk ( )
        {
            Result<String, Int32> ok = Ok;
            DelegateInvocationCounter<Func<String, Int32>> mapFuncCounter =
                DelegateHelpers.TrackInvocationCount<String, Int32> ( str => str.Length );

            var mapped = ok.MapOr ( -1, mapFuncCounter.WrappedDelegate );

            Assert.AreEqual ( 1, mapFuncCounter.InvocationCount );
            Assert.AreEqual ( OkValue.Length, mapped );
        }

        [TestMethod]
        public void MapOrReturnsFallbackForErr ( )
        {
            Result<String, Int32> err = Err;
            DelegateInvocationCounter<Func<String, Int32>> mapFuncCounter =
                DelegateHelpers.TrackInvocationCount<String, Int32> ( str => str.Length );

            var mapped = err.MapOr ( -1, mapFuncCounter.WrappedDelegate );

            Assert.AreEqual ( 0, mapFuncCounter.InvocationCount );
            Assert.AreEqual ( -1, mapped );
        }

        [TestMethod]
        public void MapOrElseReturnsInvocationResultForOk ( )
        {
            Result<String, Int32> ok = Ok;
            DelegateInvocationCounter<Func<Int32>> fallbackFuncCounter =
                DelegateHelpers.TrackInvocationCount ( ( ) => -1 );
            DelegateInvocationCounter<Func<String, Int32>> mapFuncCounter =
                DelegateHelpers.TrackInvocationCount<String, Int32> ( str => str.Length );

            var mapped = ok.MapOrElse ( fallbackFuncCounter.WrappedDelegate, mapFuncCounter.WrappedDelegate );

            Assert.AreEqual ( 0, fallbackFuncCounter.InvocationCount );
            Assert.AreEqual ( 1, mapFuncCounter.InvocationCount );
            Assert.AreEqual ( OkValue.Length, mapped );
        }

        [TestMethod]
        public void MapOrElseReturnsFallbackInvocationResultForErr ( )
        {
            Result<String, Int32> err = Err;
            DelegateInvocationCounter<Func<Int32>> fallbackFuncCounter =
                DelegateHelpers.TrackInvocationCount ( ( ) => -1 );
            DelegateInvocationCounter<Func<String, Int32>> mapFuncCounter =
                DelegateHelpers.TrackInvocationCount<String, Int32> ( str => str.Length );

            var mapped = err.MapOrElse ( fallbackFuncCounter.WrappedDelegate, mapFuncCounter.WrappedDelegate );

            Assert.AreEqual ( 1, fallbackFuncCounter.InvocationCount );
            Assert.AreEqual ( 0, mapFuncCounter.InvocationCount );
            Assert.AreEqual ( -1, mapped );
        }

        [TestMethod]
        public void MapErrReturnsOkResultForOk ( )
        {
            Result<String, Int32> ok = Ok;
            DelegateInvocationCounter<Func<Int32, Int32>> funcCounter =
                DelegateHelpers.TrackInvocationCount<Int32, Int32> ( n => n * n );

            Result<String, Int32> mappedOk = ok.MapErr ( funcCounter.WrappedDelegate );

            Assert.AreEqual ( 0, funcCounter.InvocationCount );
            Assert.IsTrue ( mappedOk.IsOk );
            Assert.AreEqual ( OkValue, mappedOk.Ok.Value );
        }

        [TestMethod]
        public void MapErrReturnsInvocationResultForErr ( )
        {
            Result<String, Int32> err = Err;
            DelegateInvocationCounter<Func<Int32, Int32>> funcCounter =
                DelegateHelpers.TrackInvocationCount<Int32, Int32> ( n => n * n );

            Result<String, Int32> mappedErr = err.MapErr ( funcCounter.WrappedDelegate );

            Assert.AreEqual ( 1, funcCounter.InvocationCount );
            Assert.IsTrue ( mappedErr.IsErr );
            Assert.AreEqual ( ErrValue * ErrValue, mappedErr.Err.Value );
        }

        [TestMethod]
        public void AndReturnsResForOk ( )
        {
            var ok1 = Result.Ok<String, Int32> ( OkValue + '1' );
            var ok2 = Result.Ok<String, Int32> ( OkValue + '2' );
            Result<String, Int32> err = Err;

            Assert.AreEqual ( ok2, ok1.And ( ok2 ) );
            Assert.AreEqual ( err, ok1.And ( err ) );
        }

        [TestMethod]
        public void AndReturnsObjectForErr ( )
        {
            var err1 = Result.Err<String, Int32> ( ErrValue );
            var err2 = Result.Err<String, Int32> ( ErrValue - 1 );
            Result<String, Int32> ok = Ok;

            Assert.AreEqual ( err1, err1.And ( ok ) );
            Assert.AreEqual ( err1, err1.And ( err2 ) );
        }

        [TestMethod]
        public void AndThenReturnsInvocationResultForOk ( )
        {
            Result<String, Int32> ok = Ok;
            DelegateInvocationCounter<Func<String, Result<Int32, Int32>>> op =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => Result.Ok<Int32, Int32> ( s.Length ) );

            Assert.AreEqual ( Result.Ok<Int32, Int32> ( OkValue.Length ), ok.AndThen ( op.WrappedDelegate ) );
            Assert.AreEqual ( 1, op.InvocationCount );
        }

        [TestMethod]
        public void AndThenReturnsErrorResultWithErrorValueForErr ( )
        {
            Result<String, Int32> err = Err;
            DelegateInvocationCounter<Func<String, Result<Int32, Int32>>> op =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => Result.Ok<Int32, Int32> ( s.Length ) );

            Assert.AreEqual ( Result.Err<Int32, Int32> ( ErrValue ), err.AndThen ( op.WrappedDelegate ) );
            Assert.AreEqual ( 0, op.InvocationCount );
        }

        [TestMethod]
        public void OrReturnsFirstResultForOk ( )
        {
            var ok1 = Result.Ok<String, Int32> ( OkValue + "1" );
            var ok2 = Result.Ok<String, Int32> ( OkValue + "2" );

            Assert.AreEqual ( ok1, ok1.Or ( ok2 ) );
            Assert.AreEqual ( ok2, ok2.Or ( ok1 ) );
        }

        [TestMethod]
        public void OrReturnsSecondResultForErr ( )
        {
            Result<String, Int32> ok = Ok;
            Result<String, Int32> err = Err;

            Assert.AreEqual ( ok, err.Or ( ok ) );
        }

        [TestMethod]
        public void OrThenReturnsFirstResultForOk ( )
        {
            var ok1 = Result.Ok<String, Int32> ( OkValue + '1' );
            DelegateInvocationCounter<Func<Int32, Result<String, Int32>>> ok2Func =
                DelegateHelpers.TrackInvocationCount ( ( Int32 err ) => Result.Err<String, Int32> ( err * err ) );

            Assert.AreEqual ( ok1, ok1.OrThen ( ok2Func.WrappedDelegate ) );
            Assert.AreEqual ( 0, ok2Func.InvocationCount );
        }

        [TestMethod]
        public void UnwrapOrReturnsOkValueForOk ( )
        {
            Result<String, Int32> ok = Ok;
            Assert.AreEqual ( OkValue, ok.UnwrapOr ( "fallback" ) );
        }

        [TestMethod]
        public void UnwrapOrReturnsFallbackValueForErr ( )
        {
            Result<String, Int32> err = Err;
            Assert.AreEqual ( "fallback", err.UnwrapOr ( "fallback" ) );
        }

        [TestMethod]
        public void UnwrapOrElseReturnsOkValueForOk ( )
        {
            Result<String, Int32> ok = Ok;
            DelegateInvocationCounter<Func<String>> fallbackFunc =
                DelegateHelpers.TrackInvocationCount ( ( ) => "fallback" );

            Assert.AreEqual ( OkValue, ok.UnwrapOrElse ( fallbackFunc.WrappedDelegate ) );
            Assert.AreEqual ( 0, fallbackFunc.InvocationCount );
        }

        [TestMethod]
        public void UnwrapOrElseReturnsFallbackFunctionInvocationResultForErr ( )
        {
            Result<String, Int32> err = Err;
            DelegateInvocationCounter<Func<String>> fallbackFunc =
                DelegateHelpers.TrackInvocationCount ( ( ) => "fallback" );

            Assert.AreEqual ( "fallback", err.UnwrapOrElse ( fallbackFunc.WrappedDelegate ) );
            Assert.AreEqual ( 1, fallbackFunc.InvocationCount );
        }
    }
}