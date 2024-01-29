// Copyright © 2024 GGG KILLER <gggkiller2@gmail.com>
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

using System.Text;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal sealed class StringBuilderReader(StringBuilder builder) : TextReader
{
    private int _position = 0;

    public override int Peek() => _position < builder.Length ? builder[_position] : -1;
    public override int Read() => _position < builder.Length ? builder[_position++] : -1;
    public override int Read(char[] buffer, int index, int count)
    {
        count = Math.Min(count, builder.Length - _position);
        if (count > 0)
        {
            builder.CopyTo(_position, buffer, index, count);
            _position += count;
        }
        return count;
    }
    public override Task<int> ReadAsync(char[] buffer, int index, int count) => Task.FromResult(Read(buffer, index, count));
    public override int ReadBlock(char[] buffer, int index, int count) => Read(buffer, index, count);
    public override Task<int> ReadBlockAsync(char[] buffer, int index, int count) => ReadAsync(buffer, index, count);
    public override string ReadLine()
    {
        var lineBreak = IndexOf('\n');

        var crlf = false;
        if (builder[lineBreak - 1] == '\r')
            crlf = true;

        var str = builder.ToString(_position, lineBreak - _position - (crlf ? 1 : 0));
        _position = lineBreak + 1;

        return str;
    }
    public override Task<string> ReadLineAsync() => Task.FromResult(ReadLine());
    public override string ReadToEnd()
    {
        var rest = builder.ToString(_position, builder.Length - _position);
        _position = builder.Length;
        return rest;
    }
    public override Task<string> ReadToEndAsync() => Task.FromResult(ReadToEnd());

    private int IndexOf(char ch)
    {
        for (var idx = _position; idx < builder.Length; idx++)
        {
            if (builder[idx] == ch)
                return idx;
        }
        return -1;
    }
}