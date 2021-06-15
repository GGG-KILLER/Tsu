// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tsu.Testing;

namespace Tsu.Tests
{
    [TestClass]
    public class ResultTests
    {
        [ExcludeFromCodeCoverage]
        private readonly struct Box<T> : IEquatable<T> where T : IEquatable<T>
        {
            public T Boxed { get; }

            public Box(T boxed)
            {
                Boxed = boxed;
            }

            public bool Equals(T? other) => EqualityComparer<T?>.Default.Equals(Boxed, other);
        }

        private const string OkValue = "ok";

        private const int ErrValue = -1;

        private static Result<string, int> Ok => Result.Ok<string, int>(OkValue);

        private static Result<string, int> Err => Result.Err<string, int>(ErrValue);

        [TestMethod]
        public void IsOkReturnsTrueForOkResult()
        {
            var ok = Ok;
            Assert.IsTrue(ok.IsOk);
        }

        [TestMethod]
        public void IsOkReturnsFalseForErrResult()
        {
            var err = Err;
            Assert.IsFalse(err.IsOk);
        }

        [TestMethod]
        public void IsErrReturnsFalseForOkResult()
        {
            var ok = Ok;
            Assert.IsFalse(ok.IsErr);
        }

        [TestMethod]
        public void IsErrReturnsTrueForErrResult()
        {
            var err = Err;
            Assert.IsTrue(err.IsErr);
        }

        [TestMethod]
        public void OkReturnsSomeForOk()
        {
            var ok = Ok;
            Assert.IsTrue(ok.Ok.IsSome);
            Assert.AreEqual(OkValue, ok.Ok.Value);
        }

        [TestMethod]
        public void OkReturnsNoneForErr()
        {
            var ok = Ok;
            Assert.IsTrue(ok.Err.IsNone);
        }

        [TestMethod]
        public void ErrReturnsSomeForErr()
        {
            var err = Err;
            Assert.IsTrue(err.Err.IsSome);
            Assert.AreEqual(ErrValue, err.Err.Value);
        }

        [TestMethod]
        public void ErrReturnsNoneForOk()
        {
            var err = Err;
            Assert.IsTrue(err.Ok.IsNone);
        }

        [TestMethod]
        public void ContainsReturnsTrueForEqualValueOnOk()
        {
            var ok = Ok;
            var raw = OkValue;
            var boxed = new Box<string>(OkValue);

            Assert.IsTrue(ok.Contains(raw));
            Assert.IsTrue(ok.Contains(boxed));
        }

        [TestMethod]
        public void ContainsReturnFalseForDifferentValueOnOk()
        {
            var ok = Ok;
            var wrongRaw = "not " + OkValue;
            var wrongBoxed = new Box<string>(wrongRaw);

            Assert.IsFalse(ok.Contains(wrongRaw));
            Assert.IsFalse(ok.Contains(wrongBoxed));
        }

        [TestMethod]
        public void ContainsReturnsFalseWithAnyValueOnErr()
        {
            var err = Err;
            var raw = OkValue;
            var boxed = new Box<string>(OkValue);
            var wrongRaw = "not " + OkValue;
            var wrongBoxed = new Box<string>(wrongRaw);

            Assert.IsFalse(err.Contains(raw));
            Assert.IsFalse(err.Contains(boxed));
            Assert.IsFalse(err.Contains(wrongRaw));
            Assert.IsFalse(err.Contains(wrongBoxed));
        }

        [TestMethod]
        public void ContainsErrReturnsTrueForEqualValueOnErr()
        {
            var err = Err;
            var raw = ErrValue;
            var boxed = new Box<int>(raw);

            Assert.IsTrue(err.ContainsErr(raw));
            Assert.IsTrue(err.ContainsErr(boxed));
        }

        [TestMethod]
        public void ContainsErrReturnsFalseForDifferentValueOnErr()
        {
            var err = Err;
            var wrongRaw = ErrValue * 2;
            var wrongBoxed = new Box<int>(wrongRaw);

            Assert.IsFalse(err.ContainsErr(wrongRaw));
            Assert.IsFalse(err.ContainsErr(wrongBoxed));
        }

        [TestMethod]
        public void ContainsErrReturnsFalseForAnyValueOnOk()
        {
            var ok = Ok;
            var raw = ErrValue;
            var boxed = new Box<int>(raw);
            var wrongRaw = ErrValue * 2;
            var wrongBoxed = new Box<int>(wrongRaw);

            Assert.IsFalse(ok.ContainsErr(raw));
            Assert.IsFalse(ok.ContainsErr(boxed));
            Assert.IsFalse(ok.ContainsErr(wrongRaw));
            Assert.IsFalse(ok.ContainsErr(wrongBoxed));
        }

        [TestMethod]
        public void MapReturnsInvocationResultForOk()
        {
            var ok = Ok;
            var mapFuncCounter =
                DelegateHelpers.TrackInvocationCount<string, int>(str => str.Length);

            var mappedOk = ok.Map(mapFuncCounter.WrappedDelegate);

            Assert.AreEqual(1, mapFuncCounter.InvocationCount);
            Assert.IsTrue(mappedOk.IsOk);
            Assert.AreEqual(OkValue.Length, mappedOk.Ok.Value);
        }

        [TestMethod]
        public void MapReturnsErrResultForErr()
        {
            var err = Err;
            var mapFuncCounter =
                DelegateHelpers.TrackInvocationCount<string, int>(str => str.Length);

            var mappedErr = err.Map(mapFuncCounter.WrappedDelegate);

            Assert.AreEqual(0, mapFuncCounter.InvocationCount);
            Assert.IsTrue(mappedErr.IsErr);
            Assert.AreEqual(ErrValue, mappedErr.Err.Value);
        }

        [TestMethod]
        public void MapOrReturnsInvocationResultForOk()
        {
            var ok = Ok;
            var mapFuncCounter =
                DelegateHelpers.TrackInvocationCount<string, int>(str => str.Length);

            var mapped = ok.MapOr(-1, mapFuncCounter.WrappedDelegate);

            Assert.AreEqual(1, mapFuncCounter.InvocationCount);
            Assert.AreEqual(OkValue.Length, mapped);
        }

        [TestMethod]
        public void MapOrReturnsFallbackForErr()
        {
            var err = Err;
            var mapFuncCounter =
                DelegateHelpers.TrackInvocationCount<string, int>(str => str.Length);

            var mapped = err.MapOr(-1, mapFuncCounter.WrappedDelegate);

            Assert.AreEqual(0, mapFuncCounter.InvocationCount);
            Assert.AreEqual(-1, mapped);
        }

        [TestMethod]
        public void MapOrElseReturnsInvocationResultForOk()
        {
            var ok = Ok;
            var fallbackFuncCounter =
                DelegateHelpers.TrackInvocationCount(() => -1);
            var mapFuncCounter =
                DelegateHelpers.TrackInvocationCount<string, int>(str => str.Length);

            var mapped = ok.MapOrElse(fallbackFuncCounter.WrappedDelegate, mapFuncCounter.WrappedDelegate);

            Assert.AreEqual(0, fallbackFuncCounter.InvocationCount);
            Assert.AreEqual(1, mapFuncCounter.InvocationCount);
            Assert.AreEqual(OkValue.Length, mapped);
        }

        [TestMethod]
        public void MapOrElseReturnsFallbackInvocationResultForErr()
        {
            var err = Err;
            var fallbackFuncCounter =
                DelegateHelpers.TrackInvocationCount(() => -1);
            var mapFuncCounter =
                DelegateHelpers.TrackInvocationCount<string, int>(str => str.Length);

            var mapped = err.MapOrElse(fallbackFuncCounter.WrappedDelegate, mapFuncCounter.WrappedDelegate);

            Assert.AreEqual(1, fallbackFuncCounter.InvocationCount);
            Assert.AreEqual(0, mapFuncCounter.InvocationCount);
            Assert.AreEqual(-1, mapped);
        }

        [TestMethod]
        public void MapErrReturnsOkResultForOk()
        {
            var ok = Ok;
            var funcCounter =
                DelegateHelpers.TrackInvocationCount<int, int>(n => n * n);

            var mappedOk = ok.MapErr(funcCounter.WrappedDelegate);

            Assert.AreEqual(0, funcCounter.InvocationCount);
            Assert.IsTrue(mappedOk.IsOk);
            Assert.AreEqual(OkValue, mappedOk.Ok.Value);
        }

        [TestMethod]
        public void MapErrReturnsInvocationResultForErr()
        {
            var err = Err;
            var funcCounter =
                DelegateHelpers.TrackInvocationCount<int, int>(n => n * n);

            var mappedErr = err.MapErr(funcCounter.WrappedDelegate);

            Assert.AreEqual(1, funcCounter.InvocationCount);
            Assert.IsTrue(mappedErr.IsErr);
            Assert.AreEqual(ErrValue * ErrValue, mappedErr.Err.Value);
        }

        [TestMethod]
        public void AndReturnsResForOk()
        {
            var ok1 = Result.Ok<string, int>(OkValue + '1');
            var ok2 = Result.Ok<string, int>(OkValue + '2');
            var err = Err;

            Assert.AreEqual(ok2, ok1.And(ok2));
            Assert.AreEqual(err, ok1.And(err));
        }

        [TestMethod]
        public void AndReturnsObjectForErr()
        {
            var err1 = Result.Err<string, int>(ErrValue);
            var err2 = Result.Err<string, int>(ErrValue - 1);
            var ok = Ok;

            Assert.AreEqual(err1, err1.And(ok));
            Assert.AreEqual(err1, err1.And(err2));
        }

        [TestMethod]
        public void AndThenReturnsInvocationResultForOk()
        {
            var ok = Ok;
            var op =
                DelegateHelpers.TrackInvocationCount((string s) => Result.Ok<int, int>(s.Length));

            Assert.AreEqual(Result.Ok<int, int>(OkValue.Length), ok.AndThen(op.WrappedDelegate));
            Assert.AreEqual(1, op.InvocationCount);
        }

        [TestMethod]
        public void AndThenReturnsErrorResultWithErrorValueForErr()
        {
            var err = Err;
            var op =
                DelegateHelpers.TrackInvocationCount((string s) => Result.Ok<int, int>(s.Length));

            Assert.AreEqual(Result.Err<int, int>(ErrValue), err.AndThen(op.WrappedDelegate));
            Assert.AreEqual(0, op.InvocationCount);
        }

        [TestMethod]
        public void OrReturnsFirstResultForOk()
        {
            var ok1 = Result.Ok<string, int>(OkValue + "1");
            var ok2 = Result.Ok<string, int>(OkValue + "2");

            Assert.AreEqual(ok1, ok1.Or(ok2));
            Assert.AreEqual(ok2, ok2.Or(ok1));
        }

        [TestMethod]
        public void OrReturnsSecondResultForErr()
        {
            var ok = Ok;
            var err = Err;

            Assert.AreEqual(ok, err.Or(ok));
        }

        [TestMethod]
        public void OrThenReturnsFirstResultForOk()
        {
            var ok1 = Result.Ok<string, int>(OkValue + '1');
            var ok2Func =
                DelegateHelpers.TrackInvocationCount((int err) => Result.Err<string, int>(err * err));

            Assert.AreEqual(ok1, ok1.OrThen(ok2Func.WrappedDelegate));
            Assert.AreEqual(0, ok2Func.InvocationCount);
        }

        [TestMethod]
        public void UnwrapOrReturnsOkValueForOk()
        {
            var ok = Ok;
            Assert.AreEqual(OkValue, ok.UnwrapOr("fallback"));
        }

        [TestMethod]
        public void UnwrapOrReturnsFallbackValueForErr()
        {
            var err = Err;
            Assert.AreEqual("fallback", err.UnwrapOr("fallback"));
        }

        [TestMethod]
        public void UnwrapOrElseReturnsOkValueForOk()
        {
            var ok = Ok;
            var fallbackFunc =
                DelegateHelpers.TrackInvocationCount(() => "fallback");

            Assert.AreEqual(OkValue, ok.UnwrapOrElse(fallbackFunc.WrappedDelegate));
            Assert.AreEqual(0, fallbackFunc.InvocationCount);
        }

        [TestMethod]
        public void UnwrapOrElseReturnsFallbackFunctionInvocationResultForErr()
        {
            var err = Err;
            var fallbackFunc =
                DelegateHelpers.TrackInvocationCount(() => "fallback");

            Assert.AreEqual("fallback", err.UnwrapOrElse(fallbackFunc.WrappedDelegate));
            Assert.AreEqual(1, fallbackFunc.InvocationCount);
        }
    }
}