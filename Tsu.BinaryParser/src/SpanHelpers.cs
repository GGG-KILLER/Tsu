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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsu.BinaryParser
{
    internal static class SpanHelpers
    {
        public static void Reverse(Span<byte> bytes)
        {
            switch (bytes.Length)
            {
                case 2:
                    Swap(ref bytes[0], ref bytes[1]);
                    break;
                case 4:
                    Swap(ref bytes[0], ref bytes[3]);
                    Swap(ref bytes[1], ref bytes[2]);
                    break;
                case 6:
                    Swap(ref bytes[0], ref bytes[5]);
                    Swap(ref bytes[1], ref bytes[4]);
                    Swap(ref bytes[2], ref bytes[3]);
                    break;
                case 8:
                    Swap(ref bytes[0], ref bytes[7]);
                    Swap(ref bytes[1], ref bytes[6]);
                    Swap(ref bytes[2], ref bytes[5]);
                    Swap(ref bytes[3], ref bytes[4]);
                    break;
                default:
                    for (var idx = 0; idx < bytes.Length / 2; idx++)
                        Swap(ref bytes[idx], ref bytes[bytes.Length - idx]);
                    break;
            }
        }

        public static void Swap(ref byte a, ref byte b) => (a, b) = (b, a);
    }
}
