namespace Tsu.BinaryParser;

public sealed class OptionalParser<T>(IBinaryParser<T> parser) : IBinaryParser<Option<T>>
{
    public void Serialize(ParsingContext context, Option<T> value)
    {
        throw new System.NotImplementedException();
    }

    public Result<Option<T>, DeserializeError> Deserialize(ParsingContext context)
    {
        throw new System.NotImplementedException();
    }
}