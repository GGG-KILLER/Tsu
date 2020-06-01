using System;
using GUtils.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GUtils.Tests
{
    [TestClass]
    public class OptionTests
    {
        [TestMethod]
        public void NoneReturnsEmptyOption ( )
        {
            var none = Option.None<String> ( );
            Assert.IsFalse ( none.IsSome );
            Assert.IsTrue ( none.IsNone );
        }

        [TestMethod]
        public void SomeReturnsFilledOption ( )
        {
            var some = Option.Some ( "some" );
            Assert.IsTrue ( some.IsSome );
            Assert.IsFalse ( some.IsNone );
            Assert.AreEqual ( "some", some.Value );
        }

        [TestMethod]
        public void ValueThrowsOnNoneValueAccess ( )
        {
            var none = Option.None<String> ( );
            Assert.ThrowsException<InvalidOperationException> ( ( ) => none.Value );
        }

        [TestMethod]
        public void UnwrapOrReturnsValueForSome ( )
        {
            var some = Option.Some ( "some" );
            Assert.AreEqual ( "some", some.UnwrapOr ( "other" ) );
        }

        [TestMethod]
        public void UnwrapOrReturnsFallbackForNone ( )
        {
            var none = Option.None<String> ( );
            Assert.AreEqual ( "other", none.UnwrapOr ( "other" ) );
        }

        [TestMethod]
        public void UnwrapOrElseReturnsValueForSome ( )
        {
            var some = Option.Some ( "some" );
            DelegateInvocationCounter<Func<String>> counter = DelegateHelpers.TrackInvocationCount ( ( ) => "else" );

            Assert.AreEqual ( "some", some.UnwrapOrElse ( counter.WrappedDelegate ) );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }

        [TestMethod]
        public void UnwrapOrElseReturnsFallbackForNone ( )
        {
            var none = Option.None<String> ( );
            DelegateInvocationCounter<Func<String>> counter = DelegateHelpers.TrackInvocationCount ( ( ) => "else" );

            Assert.AreEqual ( "else", none.UnwrapOrElse ( counter.WrappedDelegate ) );
            Assert.AreEqual ( 1, counter.InvocationCount );
        }

        [TestMethod]
        public void MapReturnsInvocationResultForSome ( )
        {
            var some = Option.Some ( "some" );
            DelegateInvocationCounter<Func<String, Int32>> counter =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => s.Length );

            Assert.AreEqual ( Option.Some ( 4 ), some.Map ( counter.WrappedDelegate ) );
            Assert.AreEqual ( 1, counter.InvocationCount );
        }

        [TestMethod]
        public void MapReturnsNoneForNone ( )
        {
            var none = Option.None<String> ( );
            DelegateInvocationCounter<Func<String, Int32>> counter =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => s.Length );

            Assert.AreEqual ( Option.None<Int32> ( ), none.Map ( counter.WrappedDelegate ) );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }

        [TestMethod]
        public void MapOrReturnsInvocationResultForSome ( )
        {
            var some = Option.Some ( "some" );
            DelegateInvocationCounter<Func<String, Int32>> counter =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => s.Length );

            Assert.AreEqual ( 4, some.MapOr ( 8, counter.WrappedDelegate ) );
            Assert.AreEqual ( 1, counter.InvocationCount );
        }

        [TestMethod]
        public void MapOrReturnsFallbackForNone ( )
        {
            var none = Option.None<String> ( );
            DelegateInvocationCounter<Func<String, Int32>> counter =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => s.Length );

            Assert.AreEqual ( 8, none.MapOr ( 8, counter.WrappedDelegate ) );
            Assert.AreEqual ( 0, counter.InvocationCount );
        }

        [TestMethod]
        public void MapOrElseReturnsInvocationResultForSome ( )
        {
            var some = Option.Some ( "some" );
            DelegateInvocationCounter<Func<Int32>> fallbackCounter =
                DelegateHelpers.TrackInvocationCount ( ( ) => 8 );
            DelegateInvocationCounter<Func<String, Int32>> funcCounter =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => s.Length );

            Assert.AreEqual ( 4, some.MapOrElse ( fallbackCounter.WrappedDelegate, funcCounter.WrappedDelegate ) );
            Assert.AreEqual ( 0, fallbackCounter.InvocationCount );
            Assert.AreEqual ( 1, funcCounter.InvocationCount );
        }

        [TestMethod]
        public void MapOrElseReturnsFallbackInvocationResultForNone ( )
        {
            var none = Option.None<String> ( );
            DelegateInvocationCounter<Func<Int32>> fallbackCounter =
                DelegateHelpers.TrackInvocationCount ( ( ) => 8 );
            DelegateInvocationCounter<Func<String, Int32>> funcCounter =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => s.Length );

            Assert.AreEqual ( 8, none.MapOrElse ( fallbackCounter.WrappedDelegate, funcCounter.WrappedDelegate ) );
            Assert.AreEqual ( 1, fallbackCounter.InvocationCount );
            Assert.AreEqual ( 0, funcCounter.InvocationCount );
        }

        [TestMethod]
        public void AndReturnsOtherForSome ( )
        {
            var some1 = Option.Some ( "some1" );
            var some2 = Option.Some ( "some2" );

            Assert.AreEqual ( some2, some1.And ( some2 ) );
            Assert.AreEqual ( some1, some2.And ( some1 ) );
        }

        [TestMethod]
        public void AndReturnsNoneForNone ( )
        {
            var none = Option.None<String> ( );
            var some = Option.Some ( "some" );

            Assert.AreEqual ( none, none.And ( some ) );
            Assert.AreEqual ( none, some.And ( none ) );
            Assert.AreEqual ( none, none.And ( none ) );
        }

        [TestMethod]
        public void AndThenReturnsInvocationFunctionResultForSome ( )
        {
            var some = Option.Some ( "some" );
            DelegateInvocationCounter<Func<String, Option<Int32>>> funcCounter =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => Option.Some ( s.Length ) );

            Assert.AreEqual ( Option.Some ( 4 ), some.AndThen ( funcCounter.WrappedDelegate ) );
            Assert.AreEqual ( 1, funcCounter.InvocationCount );
        }

        [TestMethod]
        public void AndThenReturnsNoneForNone ( )
        {
            var none = Option.None<String> ( );
            DelegateInvocationCounter<Func<String, Option<Int32>>> funcCounter =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => Option.Some ( s.Length ) );

            Assert.AreEqual ( Option.None<Int32> ( ), none.AndThen ( funcCounter.WrappedDelegate ) );
            Assert.AreEqual ( 0, funcCounter.InvocationCount );
        }

        [TestMethod]
        public void FilterReturnsValueWhenPredicatePassesOnSome ( )
        {
            var some = Option.Some ( "some" );
            DelegateInvocationCounter<Func<String, Boolean>> filterCounter =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => s.Equals ( "some" ) );

            Assert.AreEqual ( some, some.Filter ( filterCounter.WrappedDelegate ) );
            Assert.AreEqual ( 1, filterCounter.InvocationCount );
        }

        [TestMethod]
        public void FilterReturnsNoneWhenPredicateDoesNotPassesOnSome ( )
        {
            var some = Option.Some ( "some" );
            DelegateInvocationCounter<Func<String, Boolean>> filterCounter =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => s.Equals ( "" ) );

            Assert.AreEqual ( Option.None<String> ( ), some.Filter ( filterCounter.WrappedDelegate ) );
            Assert.AreEqual ( 1, filterCounter.InvocationCount );
        }

        [TestMethod]
        public void FilterReturnsNoneForNone ( )
        {
            var none = Option.None<String> ( );
            DelegateInvocationCounter<Func<String, Boolean>> filterCounter =
                DelegateHelpers.TrackInvocationCount ( ( String s ) => s.Equals ( "" ) );

            Assert.AreEqual ( none, none.Filter ( filterCounter.WrappedDelegate ) );
            Assert.AreEqual ( 0, filterCounter.InvocationCount );
        }

        [TestMethod]
        public void OrReturnsFirstOptionIfSome ( )
        {
            var some1 = Option.Some ( "some1" );
            var some2 = Option.Some ( "some2" );
            var none = Option.None<String> ( );

            Assert.AreEqual ( some1, some1.Or ( some2 ) );
            Assert.AreEqual ( some1, some1.Or ( none ) );
        }

        [TestMethod]
        public void OrThenSecondOptionIfFirstIsNone ( )
        {
            var none = Option.None<String> ( );
            var some = Option.Some ( "some" );

            Assert.AreEqual ( some, none.Or ( some ) );
            Assert.AreEqual ( none, none.Or ( none ) );
        }

        [TestMethod]
        public void OrThenReturnsFirsOptionIfSome ( )
        {
            var some1 = Option.Some ( "some1" );
            var some2 = Option.Some ( "some2" );
            var none = Option.None<String> ( );

            DelegateInvocationCounter<Func<Option<String>>> funcSome2Counter =
                DelegateHelpers.TrackInvocationCount ( ( ) => some2 );
            DelegateInvocationCounter<Func<Option<String>>> funcNoneCounter =
                DelegateHelpers.TrackInvocationCount ( ( ) => none );

            Assert.AreEqual ( some1, some1.OrElse ( funcSome2Counter.WrappedDelegate ) );
            Assert.AreEqual ( 0, funcSome2Counter.InvocationCount );
            Assert.AreEqual ( some1, some1.OrElse ( funcNoneCounter.WrappedDelegate ) );
            Assert.AreEqual ( 0, funcNoneCounter.InvocationCount );
        }

        [TestMethod]
        public void OrElseReturnsResultOfInvocationIfNone ( )
        {
            var none = Option.None<String> ( );
            var some = Option.Some ( "some" );

            DelegateInvocationCounter<Func<Option<String>>> funcSome =
                DelegateHelpers.TrackInvocationCount ( ( ) => some );
            DelegateInvocationCounter<Func<Option<String>>> funcNone =
                DelegateHelpers.TrackInvocationCount ( ( ) => none );
            
            Assert.AreEqual ( some, none.OrElse ( funcSome.WrappedDelegate ) );
            Assert.AreEqual ( 1, funcSome.InvocationCount );
            Assert.AreEqual ( none, none.OrElse ( funcNone.WrappedDelegate ) );
            Assert.AreEqual ( 1, funcNone.InvocationCount );
        }

        [TestMethod]
        public void XorReturnsEitherResultIfOnlyOneOfThemIsSome ( )
        {
            var some = Option.Some ( "some" );
            var none = Option.None<String> ( );

            Assert.AreEqual ( some, some.Xor ( none ) );
            Assert.AreEqual ( some, none.Xor ( some ) );
        }

        [TestMethod]
        public void XorReturnsNoneIfBothResultsAreSomeOrNone ( )
        {
            var some1 = Option.Some ( "some1" );
            var some2 = Option.Some ( "some2" );
            var none = Option.None<String> ( );

            Assert.AreEqual ( none, some1.Xor ( some2 ) );
            Assert.AreEqual ( none, none.Xor ( none ) );
        }

        [TestMethod]
        public void FlattenReturnsNestedOption ( )
        {
            var inner = Option.Some ( "some" );
            var outer = Option.Some ( inner );

            Assert.AreEqual ( inner, outer.Flatten ( ) );
        }

        [TestMethod]
        public void EqualsImplementedCorrectlyForOtherOptions ( )
        {
            var some1_1 = Option.Some ( "some1" );
            var some1_2 = Option.Some ( "some1" );
            var some2 = Option.Some ( "some2" );

            var none1 = Option.None<String> ( );
            var none2 = Option.None<String> ( );

            Assert.AreEqual ( some1_1, some1_1 );
            Assert.AreEqual ( some1_1, some1_2 );

            Assert.AreEqual ( none1, none1 );
            Assert.AreEqual ( none1, none2 );

            Assert.AreNotEqual ( some1_1, some2 );
            Assert.AreNotEqual ( some1_1, none1 );
        }

        [TestMethod]
        public void EqualsImplementedCorrectlyForValuesOfWrappedType ( )
        {
            var optSome = Option.Some<String?> ( "some" );
            var none = Option.None<String?> ( );
            // Using concat to avoid interned strings
            var strSome = String.Concat ( "so", "me" );

            Assert.AreEqual ( optSome, strSome );
            Assert.AreNotEqual ( none, strSome );
            Assert.IsFalse ( none.Equals ( null ) );
        }

        [TestMethod]
        public void GetHashCodeImplementedCorrectly ( )
        {
            var optSome1 = Option.Some ( "some" );
            var optSome2 = Option.Some ( String.Concat ( "so", "me" ) );
            var none1 = Option.None<String> ( );
            var none2 = Option.None<String> ( );

            Assert.AreEqual ( optSome1.GetHashCode ( ), optSome2.GetHashCode ( ) );
            Assert.AreEqual ( none1.GetHashCode ( ), none2.GetHashCode ( ) );
        }
    }
}