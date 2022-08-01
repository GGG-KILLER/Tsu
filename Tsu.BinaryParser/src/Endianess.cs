namespace Tsu.BinaryParser;

/// <summary>
/// The endianess used to parse a field.
/// </summary>
public enum Endianess
{
    /// <summary>
    /// Indicates that the parsers should operate using little endian.
    /// </summary>
    LittleEndian,

    /// <summary>
    /// Indicates that the parsers should operate using big endian.
    /// </summary>
    BigEndian
}