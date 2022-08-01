// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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
    /// <param name="context">The current context for the serializing process.</param>
    /// <param name="value">The value to write onto the stream.</param>
    void Serialize(Stream stream, IBinaryParsingContext context, T value);

    /// <summary>
    /// Serializes the provided object into the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the object into.</param>
    /// <param name="context">The current context for the serializing process.</param>
    /// <param name="value">The value to write onto the stream.</param>
    /// <param name="cancellationToken">Cancellation token to stop the parsing.</param>
    ValueTask SerializeAsync(Stream stream, IBinaryParsingContext context, T value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deserializes the provided object from the stream.
    /// </summary>
    /// <param name="stream">The stream to read the object from.</param>
    /// <param name="context">The current context for the deserializing process.</param>
    /// <returns>The parsed type.</returns>
    T Deserialize(Stream stream, IBinaryParsingContext context);

    /// <summary>
    /// Deserializes the provided object from the stream.
    /// </summary>
    /// <param name="stream">The stream to read the object from.</param>
    /// <param name="context">The current context for the deserializing process.</param>
    /// <param name="cancellationToken">Cancellation token to stop the parsing.</param>
    /// <returns>The parsed type.</returns>
    ValueTask<T> DeserializeAsync(Stream stream, IBinaryParsingContext context, CancellationToken cancellationToken = default);
}
