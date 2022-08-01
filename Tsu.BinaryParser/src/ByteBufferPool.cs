using System.Buffers;

namespace Tsu.BinaryParser;

internal static class ByteBufferPool
{
    public static ByteBuffer Rent(int desiredLength, bool clearWhenDone = false) =>
#if HAS_SPAN
        new ByteBuffer(MemoryPool<byte>.Shared.Rent(desiredLength), desiredLength, clearWhenDone);
#else
        new ByteBuffer(ArrayPool<byte>.Shared.Rent(desiredLength), desiredLength, clearWhenDone);
#endif
}