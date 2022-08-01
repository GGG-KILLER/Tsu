using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tsu.BinaryParser.Parsers;

/// <summary>
/// The interface binary object parsers.
/// </summary>
/// <typeparam name="T">The type that this binary parser handles.</typeparam>
public interface IBinaryParser<T>
{
    /// <summary>
    /// The minimum byte count for the type this parser is responsible for (or None if unable to determine).
    /// </summary>
    Option<int> MinimumByteCount { get; }

    /// <summary>
    /// The maximum byte count for the type this parser is responsible for (or None if unable to determine).
    /// </summary>
    Option<int> MaxmiumByteCount { get; }

    /// <summary>
    /// Whether the type that this parser is responsible for has a fixed size binary encoding.
    /// </summary>
    bool IsFixedSize { get; }

    /// <summary>
    /// Serializes the provided object into the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the object into.</param>
    /// <param name="endianess">The endianess to use when writing the type.</param>
    /// <param name="value">The value to write onto the stream.</param>
    void Serialize(Stream stream, Endianess endianess, T value);

    /// <summary>
    /// Serializes the provided object into the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the object into.</param>
    /// <param name="endianess">The endianess to use when writing the type.</param>
    /// <param name="value">The value to write onto the stream.</param>
    /// <param name="cancellationToken">Cancellation token to stop the parsing.</param>
    Task SerializeAsync(Stream stream, Endianess endianess, T value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deserializes the provided object from the stream.
    /// </summary>
    /// <param name="stream">The stream to read the object from.</param>
    /// <param name="endianess">The endianess to use when reading the type.</param>
    /// <returns>The parsed type.</returns>
    T Deserialize(Stream stream, Endianess endianess);

    /// <summary>
    /// Deserializes the provided object from the stream.
    /// </summary>
    /// <param name="stream">The stream to read the object from.</param>
    /// <param name="endianess">The endianess to use when reading the type.</param>
    /// <param name="cancellationToken">Cancellation token to stop the parsing.</param>
    /// <returns>The parsed type.</returns>
    Task<T> DeserializeAsync(Stream stream, Endianess endianess, CancellationToken cancellationToken = default);
}
