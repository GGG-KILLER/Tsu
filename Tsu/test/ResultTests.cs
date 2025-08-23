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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Const.")]
        private const string OkValue = "ok";

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Const.")]
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
            var mappedOk = Ok.Map(str => str.Length);

            Assert.IsTrue(mappedOk.IsOk);
            Assert.AreEqual(OkValue.Length, mappedOk.Ok.Value);
        }

        [TestMethod]
        public void MapReturnsErrResultForErr()
        {
            var mappedErr = Err.Map<int>(s => throw new InvalidOperationException("Test failure."));

            Assert.IsTrue(mappedErr.IsErr);
            Assert.AreEqual(ErrValue, mappedErr.Err.Value);
        }

        [TestMethod]
        public void MapOrReturnsInvocationResultForOk()
        {
            var mapped = Ok.MapOr(-1, str => str.Length);

            Assert.AreEqual(OkValue.Length, mapped);
        }

        [TestMethod]
        public void MapOrReturnsFallbackForErr()
        {
            var mapped = Err.MapOr(-1, str => throw new InvalidOperationException("Test failure."));

            Assert.AreEqual(-1, mapped);
        }

        [TestMethod]
        public void MapOrElseReturnsInvocationResultForOk()
        {
            var mapped = Ok.MapOrElse(() => throw new InvalidCastException("Test failure."), str => str.Length);

            Assert.AreEqual(OkValue.Length, mapped);
        }

        [TestMethod]
        public void MapOrElseReturnsFallbackInvocationResultForErr()
        {
            var mapped = Err.MapOrElse(() => -1, str => throw new InvalidOperationException("Test failure."));

            Assert.AreEqual(-1, mapped);
        }

        [TestMethod]
        public void MapErrReturnsOkResultForOk()
        {
            var mappedOk = Ok.MapErr<int>(n => throw new InvalidOperationException("Test failure."));

            Assert.IsTrue(mappedOk.IsOk);
            Assert.AreEqual(OkValue, mappedOk.Ok.Value);
        }

        [TestMethod]
        public void MapErrReturnsInvocationResultForErr()
        {
            var mappedErr = Err.MapErr(n => n * n);

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
        public void AndThenReturnsInvocationResultForOk() =>
            Assert.AreEqual(
                Result.Ok<int, int>(OkValue.Length),
                Ok.AndThen(s => Result.Ok<int, int>(s.Length)));

        [TestMethod]
        public void AndThenReturnsErrorResultWithErrorValueForErr() =>
            Assert.AreEqual(
                Result.Err<int, int>(ErrValue),
                Err.AndThen<int>(s => throw new InvalidOperationException("Test failure.")));

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
            var ok = Result.Ok<string, int>(OkValue + '1');

            Assert.AreEqual(ok, ok.OrThen<int>(err => throw new InvalidOperationException("Test failure.")));
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
        public void UnwrapOrElseReturnsOkValueForOk() =>
            Assert.AreEqual(
                OkValue,
                Ok.UnwrapOrElse(() => throw new InvalidOperationException("Test failure.")));

        [TestMethod]
        public void UnwrapOrElseReturnsFallbackFunctionInvocationResultForErr() =>
            Assert.AreEqual("fallback", Err.UnwrapOrElse(() => "fallback"));
    }
}
