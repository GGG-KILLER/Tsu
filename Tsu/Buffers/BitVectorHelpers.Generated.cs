using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Tsu.Buffers
{
    /// <summary>
    /// A class with helpers to deal with Spans and Arrays being used as bit vectors.
    /// </summary>
    [GeneratedCode("BitVectorHelpers.Generated.tt", "1.0")]
    public static partial class BitVectorHelpers
    {
        #region byte

        /// <summary>
        /// The amount we shift by to convert to/from bits in a <see cref="byte"/>-backed bit vector.
        /// </summary>
        public const int ByteShiftAmount = 3;

        /// <summary>
        /// The bitmask we bitwise and with to get the remainder bit count in a <see cref="byte"/>-backed bit vector.
        /// </summary>
        public const int ByteRemainderMask = 0b111;

        /// <summary>
        /// Gets the index and the offset of the provided bit on a <see cref="byte"/>-backed bit vector.
        /// </summary>
        /// <param name="bitIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetByteVectorIndexAndOffset(int bitIndex, out int offset)
        {
            offset = bitIndex & ByteRemainderMask;
            return bitIndex >> ByteShiftAmount;
        }

        /// <summary>
        /// Gets the value of a bit in a <see cref="byte"/>-backed bit vector.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetByteVectorBitValue(IReadOnlyList<byte> bytes, int bitIndex)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));

            var index = GetByteVectorIndexAndOffset(bitIndex, out var offset);
            return (bytes[index] & (1U << offset)) != 0;
        }

#if HAS_SPAN
        /// <summary>
        /// Gets the value of a bit in a <see cref="byte"/>-backed bit vector.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetByteVectorBitValue(ReadOnlySpan<byte> bytes, int bitIndex)
        {
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));

            var index = GetByteVectorIndexAndOffset(bitIndex, out var offset);
            return (bytes[index] & (1U << offset)) != 0;
        }
#endif

        /// <summary>
        /// Sets the value of a bit in a <see cref="byte"/>-backed bit vector.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void SetByteVectorBitValue(IList<byte> bytes, int bitIndex, bool value)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));
            var index = GetByteVectorIndexAndOffset(bitIndex, out var offset);
            if (value)
                bytes[index] |= (byte) (1U << offset);
            else
                bytes[index] &= (byte) ~(1U << offset);
        }

#if HAS_SPAN
        /// <summary>
        /// Sets the value of a bit in a <see cref="byte"/>-backed bit vector.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetByteVectorBitValue(Span<byte> bytes, int bitIndex, bool value)
        {
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));
            var index = GetByteVectorIndexAndOffset(bitIndex, out var offset);
            if (value)
                bytes[index] |= (byte) (1U << offset);
            else
                bytes[index] &= (byte) ~(1U << offset);
        }
#endif

        #endregion byte

        #region ushort

        /// <summary>
        /// The amount we shift by to convert to/from bits in a <see cref="ushort"/>-backed bit vector.
        /// </summary>
        public const int UInt16ShiftAmount = 4;

        /// <summary>
        /// The bitmask we bitwise and with to get the remainder bit count in a <see cref="ushort"/>-backed bit vector.
        /// </summary>
        public const int UInt16RemainderMask = 0b1111;

        /// <summary>
        /// Gets the index and the offset of the provided bit on a <see cref="ushort"/>-backed bit vector.
        /// </summary>
        /// <param name="bitIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetUInt16VectorIndexAndOffset(int bitIndex, out int offset)
        {
            offset = bitIndex & UInt16RemainderMask;
            return bitIndex >> UInt16ShiftAmount;
        }

        /// <summary>
        /// Gets the value of a bit in a <see cref="ushort"/>-backed bit vector.
        /// </summary>
        /// <param name="uint16s"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetUInt16VectorBitValue(IReadOnlyList<ushort> uint16s, int bitIndex)
        {
            if (uint16s is null)
                throw new ArgumentNullException(nameof(uint16s));
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));

            var index = GetUInt16VectorIndexAndOffset(bitIndex, out var offset);
            return (uint16s[index] & (1U << offset)) != 0;
        }

#if HAS_SPAN
        /// <summary>
        /// Gets the value of a bit in a <see cref="ushort"/>-backed bit vector.
        /// </summary>
        /// <param name="uint16s"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetUInt16VectorBitValue(ReadOnlySpan<ushort> uint16s, int bitIndex)
        {
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));

            var index = GetUInt16VectorIndexAndOffset(bitIndex, out var offset);
            return (uint16s[index] & (1U << offset)) != 0;
        }
#endif

        /// <summary>
        /// Sets the value of a bit in a <see cref="ushort"/>-backed bit vector.
        /// </summary>
        /// <param name="uint16s"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void SetUInt16VectorBitValue(IList<ushort> uint16s, int bitIndex, bool value)
        {
            if (uint16s is null)
                throw new ArgumentNullException(nameof(uint16s));
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));
            var index = GetUInt16VectorIndexAndOffset(bitIndex, out var offset);
            if (value)
                uint16s[index] |= (ushort) (1U << offset);
            else
                uint16s[index] &= (ushort) ~(1U << offset);
        }

#if HAS_SPAN
        /// <summary>
        /// Sets the value of a bit in a <see cref="ushort"/>-backed bit vector.
        /// </summary>
        /// <param name="uint16s"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetUInt16VectorBitValue(Span<ushort> uint16s, int bitIndex, bool value)
        {
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));
            var index = GetUInt16VectorIndexAndOffset(bitIndex, out var offset);
            if (value)
                uint16s[index] |= (ushort) (1U << offset);
            else
                uint16s[index] &= (ushort) ~(1U << offset);
        }
#endif

        #endregion ushort

        #region uint

        /// <summary>
        /// The amount we shift by to convert to/from bits in a <see cref="uint"/>-backed bit vector.
        /// </summary>
        public const int UInt32ShiftAmount = 5;

        /// <summary>
        /// The bitmask we bitwise and with to get the remainder bit count in a <see cref="uint"/>-backed bit vector.
        /// </summary>
        public const int UInt32RemainderMask = 0b11111;

        /// <summary>
        /// Gets the index and the offset of the provided bit on a <see cref="uint"/>-backed bit vector.
        /// </summary>
        /// <param name="bitIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetUInt32VectorIndexAndOffset(int bitIndex, out int offset)
        {
            offset = bitIndex & UInt32RemainderMask;
            return bitIndex >> UInt32ShiftAmount;
        }

        /// <summary>
        /// Gets the value of a bit in a <see cref="uint"/>-backed bit vector.
        /// </summary>
        /// <param name="uint32s"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetUInt32VectorBitValue(IReadOnlyList<uint> uint32s, int bitIndex)
        {
            if (uint32s is null)
                throw new ArgumentNullException(nameof(uint32s));
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));

            var index = GetUInt32VectorIndexAndOffset(bitIndex, out var offset);
            return (uint32s[index] & (1U << offset)) != 0;
        }

#if HAS_SPAN
        /// <summary>
        /// Gets the value of a bit in a <see cref="uint"/>-backed bit vector.
        /// </summary>
        /// <param name="uint32s"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetUInt32VectorBitValue(ReadOnlySpan<uint> uint32s, int bitIndex)
        {
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));

            var index = GetUInt32VectorIndexAndOffset(bitIndex, out var offset);
            return (uint32s[index] & (1U << offset)) != 0;
        }
#endif

        /// <summary>
        /// Sets the value of a bit in a <see cref="uint"/>-backed bit vector.
        /// </summary>
        /// <param name="uint32s"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void SetUInt32VectorBitValue(IList<uint> uint32s, int bitIndex, bool value)
        {
            if (uint32s is null)
                throw new ArgumentNullException(nameof(uint32s));
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));
            var index = GetUInt32VectorIndexAndOffset(bitIndex, out var offset);
            if (value)
                uint32s[index] |= (uint) (1U << offset);
            else
                uint32s[index] &= (uint) ~(1U << offset);
        }

#if HAS_SPAN
        /// <summary>
        /// Sets the value of a bit in a <see cref="uint"/>-backed bit vector.
        /// </summary>
        /// <param name="uint32s"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetUInt32VectorBitValue(Span<uint> uint32s, int bitIndex, bool value)
        {
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));
            var index = GetUInt32VectorIndexAndOffset(bitIndex, out var offset);
            if (value)
                uint32s[index] |= (uint) (1U << offset);
            else
                uint32s[index] &= (uint) ~(1U << offset);
        }
#endif

        #endregion uint

        #region ulong

        /// <summary>
        /// The amount we shift by to convert to/from bits in a <see cref="ulong"/>-backed bit vector.
        /// </summary>
        public const int UInt64ShiftAmount = 6;

        /// <summary>
        /// The bitmask we bitwise and with to get the remainder bit count in a <see cref="ulong"/>-backed bit vector.
        /// </summary>
        public const int UInt64RemainderMask = 0b111111;

        /// <summary>
        /// Gets the index and the offset of the provided bit on a <see cref="ulong"/>-backed bit vector.
        /// </summary>
        /// <param name="bitIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetUInt64VectorIndexAndOffset(int bitIndex, out int offset)
        {
            offset = bitIndex & UInt64RemainderMask;
            return bitIndex >> UInt64ShiftAmount;
        }

        /// <summary>
        /// Gets the value of a bit in a <see cref="ulong"/>-backed bit vector.
        /// </summary>
        /// <param name="uint64s"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetUInt64VectorBitValue(IReadOnlyList<ulong> uint64s, int bitIndex)
        {
            if (uint64s is null)
                throw new ArgumentNullException(nameof(uint64s));
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));

            var index = GetUInt64VectorIndexAndOffset(bitIndex, out var offset);
            return (uint64s[index] & (1U << offset)) != 0;
        }

#if HAS_SPAN
        /// <summary>
        /// Gets the value of a bit in a <see cref="ulong"/>-backed bit vector.
        /// </summary>
        /// <param name="uint64s"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetUInt64VectorBitValue(ReadOnlySpan<ulong> uint64s, int bitIndex)
        {
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));

            var index = GetUInt64VectorIndexAndOffset(bitIndex, out var offset);
            return (uint64s[index] & (1U << offset)) != 0;
        }
#endif

        /// <summary>
        /// Sets the value of a bit in a <see cref="ulong"/>-backed bit vector.
        /// </summary>
        /// <param name="uint64s"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void SetUInt64VectorBitValue(IList<ulong> uint64s, int bitIndex, bool value)
        {
            if (uint64s is null)
                throw new ArgumentNullException(nameof(uint64s));
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));
            var index = GetUInt64VectorIndexAndOffset(bitIndex, out var offset);
            if (value)
                uint64s[index] |= (ulong) (1U << offset);
            else
                uint64s[index] &= (ulong) ~(1U << offset);
        }

#if HAS_SPAN
        /// <summary>
        /// Sets the value of a bit in a <see cref="ulong"/>-backed bit vector.
        /// </summary>
        /// <param name="uint64s"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetUInt64VectorBitValue(Span<ulong> uint64s, int bitIndex, bool value)
        {
            if (bitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));
            var index = GetUInt64VectorIndexAndOffset(bitIndex, out var offset);
            if (value)
                uint64s[index] |= (ulong) (1U << offset);
            else
                uint64s[index] &= (ulong) ~(1U << offset);
        }
#endif

        #endregion ulong

    }
}