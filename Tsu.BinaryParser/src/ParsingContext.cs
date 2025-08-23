using System;
using System.IO;
using System.Threading;

namespace Tsu.BinaryParser;

/// <summary>
/// The parsing context used while parsing.
/// </summary>
/// <param name="endianess"></param>
public class ParsingContext(Stream stream, Endianess endianess = Endianess.Undefined)
{
    private int _endianess = (int)endianess;

    /// <summary>
    /// The stream that's used for serializing/deserializing.
    /// </summary>
    public Stream Stream { get; } = stream;

    /// <summary>
    /// The current endianess used while parsing.
    /// </summary>
    public Endianess Endianess
    {
        get => (Endianess)_endianess;
        set
        {
            if (Interlocked.CompareExchange(ref _endianess, (int) value, (int) Endianess.Undefined) != (int) Endianess.Undefined)
                throw new InvalidOperationException("Cannot set endianess after it has already been set.");
        }
    }
}

/// <summary>
/// Determines the kind of endianess we're (de)serializing with.
/// </summary>
public enum Endianess
{
    /// <summary>
    /// Endianess hasn't been set.
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// All operations should be done using Big Endian.
    /// </summary>
    BigEndian,

    /// <summary>
    /// All operations should be done using Little Endian.
    /// </summary>
    LittleEndian,
}