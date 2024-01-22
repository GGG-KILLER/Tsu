using System;
using System.IO;

namespace Tsu.BinaryParser.Tests;

internal static class TestData
{
    public static MemoryStream RandomStream(int length)
    {
        var buffer = new byte[length];
        var random = new Random(0xA23B6FA);
        random.NextBytes(buffer);
        return new MemoryStream(buffer);
    }
}