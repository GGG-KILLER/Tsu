using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Tsu.Buffers
{
    /// <summary>
    /// A class with helpers to deal with Spans and Arrays being used as bit vectors.
    /// </summary>
    [GeneratedCode ( "BitVectorHelpers.Generated.tt", "1.0" )]
    public static partial class BitVectorHelpers
    {
        #region Byte

        /// <summary>
        /// The amount we shift by to convert to/from bits in a <see cref="Byte"/>-backed bit vector.
        /// </summary>
        public const Int32 ByteShiftAmount = 3;

        /// <summary>
        /// The bitmask we bitwise and with to get the remainder bit count in a <see cref="Byte"/>-backed bit vector.
        /// </summary>
        public const Int32 ByteRemainderMask = 0b111;

        /// <summary>
        /// Gets the index and the offset of the provided bit on a <see cref="Byte"/>-backed bit vector.
        /// </summary>
        /// <param name="bitIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Int32 GetByteVectorIndexAndOffset ( Int32 bitIndex, out Int32 offset )
        {
            offset = bitIndex & ByteRemainderMask;
            return bitIndex >> ByteShiftAmount;
        }

        /// <summary>
        /// Gets the value of a bit in a <see cref="Byte"/>-backed bit vector.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Boolean GetByteVectorBitValue ( IReadOnlyList<Byte> bytes, Int32 bitIndex )
        {
            if ( bytes is null )
                throw new ArgumentNullException ( nameof ( bytes ) );
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );

            var index = GetByteVectorIndexAndOffset ( bitIndex, out var offset );
            return ( bytes[index] & ( 1U << offset ) ) != 0;
        }

#if HAS_SPAN
        /// <summary>
        /// Gets the value of a bit in a <see cref="Byte"/>-backed bit vector.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Boolean GetByteVectorBitValue ( ReadOnlySpan<Byte> bytes, Int32 bitIndex )
        {
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );

            var index = GetByteVectorIndexAndOffset ( bitIndex, out var offset );
            return ( bytes[index] & ( 1U << offset ) ) != 0;
        }
#endif

        /// <summary>
        /// Sets the value of a bit in a <see cref="Byte"/>-backed bit vector.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void SetByteVectorBitValue ( IList<Byte> bytes, Int32 bitIndex, Boolean value )
        {
            if ( bytes is null )
                throw new ArgumentNullException ( nameof ( bytes ) );
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );
            var index = GetByteVectorIndexAndOffset ( bitIndex, out var offset );
            if ( value )
                bytes[index] |= ( Byte ) ( 1U << offset );
            else
                bytes[index] &= ( Byte ) ~( 1U << offset );
        }

#if HAS_SPAN
        /// <summary>
        /// Sets the value of a bit in a <see cref="Byte"/>-backed bit vector.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static void SetByteVectorBitValue ( Span<Byte> bytes, Int32 bitIndex, Boolean value )
        {
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );
            var index = GetByteVectorIndexAndOffset ( bitIndex, out var offset );
            if ( value )
                bytes[index] |= ( Byte ) ( 1U << offset );
            else
                bytes[index] &= ( Byte ) ~( 1U << offset );
        }
#endif

        #endregion Byte

        #region UInt16

        /// <summary>
        /// The amount we shift by to convert to/from bits in a <see cref="UInt16"/>-backed bit vector.
        /// </summary>
        public const Int32 UInt16ShiftAmount = 4;

        /// <summary>
        /// The bitmask we bitwise and with to get the remainder bit count in a <see cref="UInt16"/>-backed bit vector.
        /// </summary>
        public const Int32 UInt16RemainderMask = 0b1111;

        /// <summary>
        /// Gets the index and the offset of the provided bit on a <see cref="UInt16"/>-backed bit vector.
        /// </summary>
        /// <param name="bitIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Int32 GetUInt16VectorIndexAndOffset ( Int32 bitIndex, out Int32 offset )
        {
            offset = bitIndex & UInt16RemainderMask;
            return bitIndex >> UInt16ShiftAmount;
        }

        /// <summary>
        /// Gets the value of a bit in a <see cref="UInt16"/>-backed bit vector.
        /// </summary>
        /// <param name="uint16s"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Boolean GetUInt16VectorBitValue ( IReadOnlyList<UInt16> uint16s, Int32 bitIndex )
        {
            if ( uint16s is null )
                throw new ArgumentNullException ( nameof ( uint16s ) );
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );

            var index = GetUInt16VectorIndexAndOffset ( bitIndex, out var offset );
            return ( uint16s[index] & ( 1U << offset ) ) != 0;
        }

#if HAS_SPAN
        /// <summary>
        /// Gets the value of a bit in a <see cref="UInt16"/>-backed bit vector.
        /// </summary>
        /// <param name="uint16s"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Boolean GetUInt16VectorBitValue ( ReadOnlySpan<UInt16> uint16s, Int32 bitIndex )
        {
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );

            var index = GetUInt16VectorIndexAndOffset ( bitIndex, out var offset );
            return ( uint16s[index] & ( 1U << offset ) ) != 0;
        }
#endif

        /// <summary>
        /// Sets the value of a bit in a <see cref="UInt16"/>-backed bit vector.
        /// </summary>
        /// <param name="uint16s"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void SetUInt16VectorBitValue ( IList<UInt16> uint16s, Int32 bitIndex, Boolean value )
        {
            if ( uint16s is null )
                throw new ArgumentNullException ( nameof ( uint16s ) );
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );
            var index = GetUInt16VectorIndexAndOffset ( bitIndex, out var offset );
            if ( value )
                uint16s[index] |= ( UInt16 ) ( 1U << offset );
            else
                uint16s[index] &= ( UInt16 ) ~( 1U << offset );
        }

#if HAS_SPAN
        /// <summary>
        /// Sets the value of a bit in a <see cref="UInt16"/>-backed bit vector.
        /// </summary>
        /// <param name="uint16s"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static void SetUInt16VectorBitValue ( Span<UInt16> uint16s, Int32 bitIndex, Boolean value )
        {
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );
            var index = GetUInt16VectorIndexAndOffset ( bitIndex, out var offset );
            if ( value )
                uint16s[index] |= ( UInt16 ) ( 1U << offset );
            else
                uint16s[index] &= ( UInt16 ) ~( 1U << offset );
        }
#endif

        #endregion UInt16

        #region UInt32

        /// <summary>
        /// The amount we shift by to convert to/from bits in a <see cref="UInt32"/>-backed bit vector.
        /// </summary>
        public const Int32 UInt32ShiftAmount = 5;

        /// <summary>
        /// The bitmask we bitwise and with to get the remainder bit count in a <see cref="UInt32"/>-backed bit vector.
        /// </summary>
        public const Int32 UInt32RemainderMask = 0b11111;

        /// <summary>
        /// Gets the index and the offset of the provided bit on a <see cref="UInt32"/>-backed bit vector.
        /// </summary>
        /// <param name="bitIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Int32 GetUInt32VectorIndexAndOffset ( Int32 bitIndex, out Int32 offset )
        {
            offset = bitIndex & UInt32RemainderMask;
            return bitIndex >> UInt32ShiftAmount;
        }

        /// <summary>
        /// Gets the value of a bit in a <see cref="UInt32"/>-backed bit vector.
        /// </summary>
        /// <param name="uint32s"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Boolean GetUInt32VectorBitValue ( IReadOnlyList<UInt32> uint32s, Int32 bitIndex )
        {
            if ( uint32s is null )
                throw new ArgumentNullException ( nameof ( uint32s ) );
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );

            var index = GetUInt32VectorIndexAndOffset ( bitIndex, out var offset );
            return ( uint32s[index] & ( 1U << offset ) ) != 0;
        }

#if HAS_SPAN
        /// <summary>
        /// Gets the value of a bit in a <see cref="UInt32"/>-backed bit vector.
        /// </summary>
        /// <param name="uint32s"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Boolean GetUInt32VectorBitValue ( ReadOnlySpan<UInt32> uint32s, Int32 bitIndex )
        {
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );

            var index = GetUInt32VectorIndexAndOffset ( bitIndex, out var offset );
            return ( uint32s[index] & ( 1U << offset ) ) != 0;
        }
#endif

        /// <summary>
        /// Sets the value of a bit in a <see cref="UInt32"/>-backed bit vector.
        /// </summary>
        /// <param name="uint32s"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void SetUInt32VectorBitValue ( IList<UInt32> uint32s, Int32 bitIndex, Boolean value )
        {
            if ( uint32s is null )
                throw new ArgumentNullException ( nameof ( uint32s ) );
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );
            var index = GetUInt32VectorIndexAndOffset ( bitIndex, out var offset );
            if ( value )
                uint32s[index] |= ( UInt32 ) ( 1U << offset );
            else
                uint32s[index] &= ( UInt32 ) ~( 1U << offset );
        }

#if HAS_SPAN
        /// <summary>
        /// Sets the value of a bit in a <see cref="UInt32"/>-backed bit vector.
        /// </summary>
        /// <param name="uint32s"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static void SetUInt32VectorBitValue ( Span<UInt32> uint32s, Int32 bitIndex, Boolean value )
        {
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );
            var index = GetUInt32VectorIndexAndOffset ( bitIndex, out var offset );
            if ( value )
                uint32s[index] |= ( UInt32 ) ( 1U << offset );
            else
                uint32s[index] &= ( UInt32 ) ~( 1U << offset );
        }
#endif

        #endregion UInt32

        #region UInt64

        /// <summary>
        /// The amount we shift by to convert to/from bits in a <see cref="UInt64"/>-backed bit vector.
        /// </summary>
        public const Int32 UInt64ShiftAmount = 6;

        /// <summary>
        /// The bitmask we bitwise and with to get the remainder bit count in a <see cref="UInt64"/>-backed bit vector.
        /// </summary>
        public const Int32 UInt64RemainderMask = 0b111111;

        /// <summary>
        /// Gets the index and the offset of the provided bit on a <see cref="UInt64"/>-backed bit vector.
        /// </summary>
        /// <param name="bitIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Int32 GetUInt64VectorIndexAndOffset ( Int32 bitIndex, out Int32 offset )
        {
            offset = bitIndex & UInt64RemainderMask;
            return bitIndex >> UInt64ShiftAmount;
        }

        /// <summary>
        /// Gets the value of a bit in a <see cref="UInt64"/>-backed bit vector.
        /// </summary>
        /// <param name="uint64s"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Boolean GetUInt64VectorBitValue ( IReadOnlyList<UInt64> uint64s, Int32 bitIndex )
        {
            if ( uint64s is null )
                throw new ArgumentNullException ( nameof ( uint64s ) );
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );

            var index = GetUInt64VectorIndexAndOffset ( bitIndex, out var offset );
            return ( uint64s[index] & ( 1U << offset ) ) != 0;
        }

#if HAS_SPAN
        /// <summary>
        /// Gets the value of a bit in a <see cref="UInt64"/>-backed bit vector.
        /// </summary>
        /// <param name="uint64s"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static Boolean GetUInt64VectorBitValue ( ReadOnlySpan<UInt64> uint64s, Int32 bitIndex )
        {
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );

            var index = GetUInt64VectorIndexAndOffset ( bitIndex, out var offset );
            return ( uint64s[index] & ( 1U << offset ) ) != 0;
        }
#endif

        /// <summary>
        /// Sets the value of a bit in a <see cref="UInt64"/>-backed bit vector.
        /// </summary>
        /// <param name="uint64s"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void SetUInt64VectorBitValue ( IList<UInt64> uint64s, Int32 bitIndex, Boolean value )
        {
            if ( uint64s is null )
                throw new ArgumentNullException ( nameof ( uint64s ) );
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );
            var index = GetUInt64VectorIndexAndOffset ( bitIndex, out var offset );
            if ( value )
                uint64s[index] |= ( UInt64 ) ( 1U << offset );
            else
                uint64s[index] &= ( UInt64 ) ~( 1U << offset );
        }

#if HAS_SPAN
        /// <summary>
        /// Sets the value of a bit in a <see cref="UInt64"/>-backed bit vector.
        /// </summary>
        /// <param name="uint64s"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public static void SetUInt64VectorBitValue ( Span<UInt64> uint64s, Int32 bitIndex, Boolean value )
        {
            if ( bitIndex < 0 )
                throw new ArgumentOutOfRangeException ( nameof ( bitIndex ) );
            var index = GetUInt64VectorIndexAndOffset ( bitIndex, out var offset );
            if ( value )
                uint64s[index] |= ( UInt64 ) ( 1U << offset );
            else
                uint64s[index] &= ( UInt64 ) ~( 1U << offset );
        }
#endif

        #endregion UInt64

    }
}