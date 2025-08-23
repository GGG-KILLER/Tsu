using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Tsu.BinaryParser;

/// <summary>
/// Class that parses integers
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="isUnsigned"></param>
public class IntegerParser<T>(bool isUnsigned) : IBinaryParser<T> where T : unmanaged, IBinaryInteger<T>
{
    private static readonly int s_byteLength = Marshal.SizeOf<T>();

    /// <inheritdoc/>
    public void Serialize(ParsingContext context, T value)
    {
        Span<byte> buffer = stackalloc byte[s_byteLength];

        if (context.Endianess == Endianess.BigEndian)
            value.WriteBigEndian(buffer);
        else if (context.Endianess == Endianess.LittleEndian)
            value.WriteLittleEndian(buffer);
        else
            throw new InvalidOperationException("Endianess hasn't been defined.");

        context.Stream.Write(buffer);
    }

    /// <inheritdoc/>
    public Result<T, DeserializeError> Deserialize(ParsingContext context)
    {
        Span<byte> buffer = stackalloc byte[s_byteLength];
        var pos = context.Stream.Position;
        var len = context.Stream.Read(buffer);
        if (len != buffer.Length)
            return Result.Err<T, DeserializeError>(new EndOfStreamError(pos));

        T value;
        if (context.Endianess == Endianess.BigEndian)
        {
            if (!T.TryReadBigEndian(buffer, isUnsigned, out value))
                return Result.Err<T, DeserializeError>(new OverflowError(pos));
        }
        else if (context.Endianess == Endianess.LittleEndian)
        {
            if (!T.TryReadLittleEndian(buffer, isUnsigned, out value))
                return Result.Err<T, DeserializeError>(new OverflowError(pos));
        }
        else
        {
            throw new InvalidOperationException("Endianess hasn't been defined.");
        }

        return Result.Ok<T, DeserializeError>(value);
    }
}