
using System.IO;
using System.Threading.Tasks;
using Tsu.BinaryParser.Parsers;

namespace Tsu.BinaryParser.Tests.Parsers;

public class ChangeEndianessParserTests
{
    [Fact]
    public void ChangeEndianessParser_HasZeroByteCount()
    {
        // Given
        var parser = new ChangeEndianessParser(Endianess.LittleEndian);

        // When
        var min = parser.MinimumByteCount;
        var max = parser.MaxmiumByteCount;

        // Then
        Assert.True(min.IsSome);
        Assert.Equal(0, min.Value);
        Assert.True(max.IsSome);
        Assert.Equal(0, max.Value);
    }

    [Fact]
    public void ChangeEndianessParser_HasFixedSize()
    {
        // Given
        var parser = new ChangeEndianessParser(Endianess.LittleEndian);

        // When
        var isFixedSize = parser.IsFixedSize;

        // Then
        Assert.True(isFixedSize);
    }

    [Fact]
    public void ChangeEndianessParser_CalculateSize_AlwaysReturnsZero()
    {
        // Given
        var parser = new ChangeEndianessParser(Endianess.LittleEndian);

        // When
        var size = parser.CalculateSize(Unit.Value);

        // Then
        Assert.Equal(0, size);
    }

    [Fact]
    public void ChangeEndianessParser_Deserialize_ChangesContext()
    {
        // Given
        using var stream = new MemoryStream();
        using var reader = new BinaryReader(stream);
        var context = new BinaryParsingContext(Endianess.BigEndian);
        var parser = new ChangeEndianessParser(Endianess.LittleEndian);

        // When
        parser.Deserialize(reader, context);

        // Then
        Assert.Equal(Endianess.LittleEndian, context.Endianess);
    }

    [Fact]
    public void ChangeEndianessParser_Deserialize_DoesNotConsumeFromStream()
    {
        // Given
        using var stream = TestData.RandomStream(128);
        using var reader = new BinaryReader(stream);
        var context = new BinaryParsingContext(Endianess.BigEndian);
        var parser = new ChangeEndianessParser(Endianess.LittleEndian);

        // When
        parser.Deserialize(reader, context);

        // Then
        Assert.Equal(0, stream.Position);
    }

    [Fact]
    public async Task ChangeEndianessParser_DeserializeAsync_ChangesContext()
    {
        // Given
        using var stream = new MemoryStream();
        using var reader = new BinaryReader(stream);
        var context = new BinaryParsingContext(Endianess.BigEndian);
        var parser = new ChangeEndianessParser(Endianess.LittleEndian);

        // When
        await parser.DeserializeAsync(reader, context);

        // Then
        Assert.Equal(Endianess.LittleEndian, context.Endianess);
    }

    [Fact]
    public async Task ChangeEndianessParser_DeserializeAsync_DoesNotConsumeFromStream()
    {
        // Given
        using var stream = TestData.RandomStream(128);
        using var reader = new BinaryReader(stream);
        var context = new BinaryParsingContext(Endianess.BigEndian);
        var parser = new ChangeEndianessParser(Endianess.LittleEndian);

        // When
        await parser.DeserializeAsync(reader, context);

        // Then
        Assert.Equal(0, stream.Position);
    }

    [Fact]
    public void ChangeEndianessParser_Serialize_ChangesContext()
    {
        // Given
        using var stream = new MemoryStream();
        var context = new BinaryParsingContext(Endianess.BigEndian);
        var parser = new ChangeEndianessParser(Endianess.LittleEndian);

        // When
        parser.Serialize(stream, context, Unit.Value);

        // Then
        Assert.Equal(Endianess.LittleEndian, context.Endianess);
    }

    [Fact]
    public void ChangeEndianessParser_Serialize_DoesNotConsumeFromStream()
    {
        // Given
        using var stream = new MemoryStream();
        var context = new BinaryParsingContext(Endianess.BigEndian);
        var parser = new ChangeEndianessParser(Endianess.LittleEndian);

        // When
        parser.Serialize(stream, context, Unit.Value);

        // Then
        Assert.Equal(0, stream.Position);
    }

    [Fact]
    public async Task ChangeEndianessParser_SerializeAsync_ChangesContext()
    {
        // Given
        using var stream = new MemoryStream();
        var context = new BinaryParsingContext(Endianess.BigEndian);
        var parser = new ChangeEndianessParser(Endianess.LittleEndian);

        // When
        await parser.SerializeAsync(stream, context, Unit.Value);

        // Then
        Assert.Equal(Endianess.LittleEndian, context.Endianess);
    }

    [Fact]
    public async Task ChangeEndianessParser_SerializeAsync_DoesNotConsumeFromStream()
    {
        // Given
        using var stream = TestData.RandomStream(128);
        var context = new BinaryParsingContext(Endianess.BigEndian);
        var parser = new ChangeEndianessParser(Endianess.LittleEndian);

        // When
        await parser.SerializeAsync(stream, context, Unit.Value);

        // Then
        Assert.Equal(0, stream.Position);
    }
}