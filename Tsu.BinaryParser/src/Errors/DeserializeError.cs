namespace Tsu.BinaryParser;

/// <summary>
/// Base error returned while deserializing.
/// </summary>
/// <param name="position">The position the error was encountered at.</param>
public abstract class DeserializeError(long position)
{
    /// <summary>
    /// The position the error was encountered at.
    /// </summary>
    public long Position { get; } = position;
}

/// <summary>
/// Error that is generated if we hit the end of the stream while parsing.
/// </summary>
/// <param name="position"><inheritdoc/></param>
public sealed class EndOfStreamError(long position) : DeserializeError(position)
{
}

/// <summary>
/// Indicates that the type cannot hold the provided number.
/// </summary>
/// <param name="position"><inheritdoc/></param>
public sealed class OverflowError(long position) : DeserializeError(position)
{
}