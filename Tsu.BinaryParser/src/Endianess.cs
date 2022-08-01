namespace Tsu.BinaryParser;

/// <summary>
/// The endianess used to parse a field.
/// </summary>
public enum Endianess
{
    /// <summary>
    /// Force little endianess parsing.
    /// </summary>
    LittleEndian,

    /// <summary>
    /// Force big endianess parsing.
    /// </summary>
    BigEndian
}