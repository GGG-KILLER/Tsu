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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tsu.Tests
{
    [TestClass]
    public class OptionTests
    {
        [TestMethod]
        public void NoneReturnsEmptyOption()
        {
            var none = Option.None<string>();
            Assert.IsFalse(none.IsSome);
            Assert.IsTrue(none.IsNone);
        }

        [TestMethod]
        public void SomeReturnsFilledOption()
        {
            var some = Option.Some("some");
            Assert.IsTrue(some.IsSome);
            Assert.IsFalse(some.IsNone);
            Assert.AreEqual("some", some.Value);
        }

        [TestMethod]
        public void ValueThrowsOnNoneValueAccess()
        {
            var none = Option.None<string>();
            Assert.ThrowsExactly<InvalidOperationException>(() => none.Value);
        }

        [TestMethod]
        public void UnwrapOrReturnsValueForSome()
        {
            var some = Option.Some("some");
            Assert.AreEqual("some", some.UnwrapOr("other"));
        }

        [TestMethod]
        public void UnwrapOrReturnsFallbackForNone()
        {
            var none = Option.None<string>();
            Assert.AreEqual("other", none.UnwrapOr("other"));
        }

        [TestMethod]
        public void UnwrapOrElseReturnsValueForSome()
        {
            var some = Option.Some("some");

            Assert.AreEqual("some", some.UnwrapOrElse(() => throw new InvalidOperationException("Test failure.")));
        }

        [TestMethod]
        public void UnwrapOrElseReturnsFallbackForNone()
        {
            var none = Option.None<string>();

            Assert.AreEqual("else", none.UnwrapOrElse(() => "else"));
        }

        [TestMethod]
        public void MapReturnsInvocationResultForSome()
        {
            var some = Option.Some("some");

            Assert.AreEqual(Option.Some(4), some.Map(s => s.Length));
        }

        [TestMethod]
        public void MapReturnsNoneForNone()
        {
            var none = Option.None<string>();

            Assert.AreEqual(Option.None<int>(), none.Map<int>(s => throw new InvalidOperationException("Test failure.")));
        }

        [TestMethod]
        public void MapOrReturnsInvocationResultForSome()
        {
            var some = Option.Some("some");

            Assert.AreEqual(4, some.MapOr(8, s => s.Length));
        }

        [TestMethod]
        public void MapOrReturnsFallbackForNone()
        {
            var none = Option.None<string>();

            Assert.AreEqual(8, none.MapOr(8, s => throw new InvalidOperationException("Test failure.")));
        }

        [TestMethod]
        public void MapOrElseReturnsInvocationResultForSome()
        {
            var some = Option.Some("some");

            Assert.AreEqual(4, some.MapOrElse(() => throw new InvalidOperationException("Test failure."), s => s.Length));
        }

        [TestMethod]
        public void MapOrElseReturnsFallbackInvocationResultForNone()
        {
            var none = Option.None<string>();

            Assert.AreEqual(8, none.MapOrElse(() => 8, s => throw new InvalidOperationException("Test failure.")));
        }

        [TestMethod]
        public void AndReturnsOtherForSome()
        {
            var some1 = Option.Some("some1");
            var some2 = Option.Some("some2");

            Assert.AreEqual(some2, some1.And(some2));
            Assert.AreEqual(some1, some2.And(some1));
        }

        [TestMethod]
        public void AndReturnsNoneForNone()
        {
            var none = Option.None<string>();
            var some = Option.Some("some");

            Assert.AreEqual(none, none.And(some));
            Assert.AreEqual(none, some.And(none));
            Assert.AreEqual(none, none.And(none));
        }

        [TestMethod]
        public void AndThenReturnsInvocationFunctionResultForSome()
        {
            var some = Option.Some("some");

            Assert.AreEqual(Option.Some(4), some.AndThen(s => Option.Some(s.Length)));
        }

        [TestMethod]
        public void AndThenReturnsNoneForNone()
        {
            var none = Option.None<string>();

            Assert.AreEqual(Option.None<int>(), none.AndThen<int>(s => throw new InvalidOperationException("Test failure.")));
        }

        [TestMethod]
        public void FilterReturnsValueWhenPredicatePassesOnSome()
        {
            var some = Option.Some("some");

            Assert.AreEqual(some, some.Filter(s => s.Equals("some", StringComparison.Ordinal)));
        }

        [TestMethod]
        public void FilterReturnsNoneWhenPredicateDoesNotPassesOnSome()
        {
            var some = Option.Some("some");

            Assert.AreEqual(Option.None<string>(), some.Filter(s => s.Equals("a", StringComparison.Ordinal)));
        }

        [TestMethod]
        public void FilterReturnsNoneForNone()
        {
            var none = Option.None<string>();

            Assert.AreEqual(none, none.Filter(s => throw new InvalidOperationException("Test failure.")));
        }

        [TestMethod]
        public void OrReturnsFirstOptionIfSome()
        {
            var some1 = Option.Some("some1");
            var some2 = Option.Some("some2");
            var none = Option.None<string>();

            Assert.AreEqual(some1, some1.Or(some2));
            Assert.AreEqual(some1, some1.Or(none));
        }

        [TestMethod]
        public void OrThenSecondOptionIfFirstIsNone()
        {
            var none = Option.None<string>();
            var some = Option.Some("some");

            Assert.AreEqual(some, none.Or(some));
            Assert.AreEqual(none, none.Or(none));
        }

        [TestMethod]
        public void OrThenReturnsFirsOptionIfSome()
        {
            var some = Option.Some("some1");

            Assert.AreEqual(some, some.OrElse(() => throw new InvalidOperationException()));
        }

        [TestMethod]
        public void OrElseReturnsResultOfInvocationIfNone()
        {
            var none = Option.None<string>();
            var some = Option.Some("some");

            Assert.AreEqual(some, none.OrElse(() => some));
        }

        [TestMethod]
        public void XorReturnsEitherResultIfOnlyOneOfThemIsSome()
        {
            var some = Option.Some("some");
            var none = Option.None<string>();

            Assert.AreEqual(some, some.Xor(none));
            Assert.AreEqual(some, none.Xor(some));
        }

        [TestMethod]
        public void XorReturnsNoneIfBothResultsAreSomeOrNone()
        {
            var some1 = Option.Some("some1");
            var some2 = Option.Some("some2");
            var none = Option.None<string>();

            Assert.AreEqual(none, some1.Xor(some2));
            Assert.AreEqual(none, none.Xor(none));
        }

        [TestMethod]
        public void FlattenReturnsNestedOption()
        {
            var inner = Option.Some("some");
            var outer = Option.Some(inner);

            Assert.AreEqual(inner, outer.Flatten());
        }

        [TestMethod]
        public void EqualsImplementedCorrectlyForOtherOptions()
        {
            var some1_1 = Option.Some("some1");
            var some1_2 = Option.Some("some1");
            var some2 = Option.Some("some2");

            var none1 = Option.None<string>();
            var none2 = Option.None<string>();

            Assert.AreEqual(some1_1, some1_1);
            Assert.AreEqual(some1_1, some1_2);

            Assert.AreEqual(none1, none1);
            Assert.AreEqual(none1, none2);

            Assert.AreNotEqual(some1_1, some2);
            Assert.AreNotEqual(some1_1, none1);
        }

        [TestMethod]
        public void EqualsImplementedCorrectlyForValuesOfWrappedType()
        {
            var optSome = Option.Some<string?>("some");
            var none = Option.None<string?>();
            // Using concat to avoid interned strings
            var strSome = string.Concat("so", "me");

            Assert.AreEqual(optSome, strSome);
            Assert.AreNotEqual(none, strSome);
            Assert.IsFalse(none.Equals(null));
        }

        [TestMethod]
        public void GetHashCodeImplementedCorrectly()
        {
            var optSome1 = Option.Some("some");
            var optSome2 = Option.Some(string.Concat("so", "me"));
            var none1 = Option.None<string>();
            var none2 = Option.None<string>();

            Assert.AreEqual(optSome1.GetHashCode(), optSome2.GetHashCode());
            Assert.AreEqual(none1.GetHashCode(), none2.GetHashCode());
        }

        [TestMethod]
        public void TrueOperatorReturnsTheCorrectValue()
        {
            var none = Option.None<string>();
            var some = Option.Some("some");

            if (none) Assert.Fail("true operator returned true for None.");

            if (some) return;
            Assert.Fail("true operator returned false for Some(T).");
        }

        [TestMethod]
        public void FalseOperatorReturnsTheCorrectValue()
        {
            var none = Option.None<string>();
            var some = Option.Some("some");

            if (none) Assert.Fail("true operator returned true for None.");
            else goto falseop_returned_true_for_none;
            Assert.Fail("false operator returned false for None.");

        falseop_returned_true_for_none:
            if (some) return;
            Assert.Fail("false operator returned");
        }
    }
}
