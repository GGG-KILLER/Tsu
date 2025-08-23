using System.IO;

namespace Tsu.BinaryParser;

/// <summary>
/// Interface for a class that parses binary content from a <see cref="System.IO.Stream"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBinaryParser<T>
{
    void Serialize(ParsingContext context, T value);

    Result<T, DeserializeError> Deserialize(ParsingContext context);
}