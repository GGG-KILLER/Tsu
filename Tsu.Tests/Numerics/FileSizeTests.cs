// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tsu.Numerics;

namespace Tsu.Tests.Numerics
{
    [TestClass]
    public class FileSizeTests
    {
        [DataTestMethod]
        [DataRow(512L, 512L, "B")]
        [DataRow(1L * FileSize.KiB, 1L, "KiB")]
        [DataRow(500L * FileSize.KiB, 500L, "KiB")]
        [DataRow(1L * FileSize.MiB, 1L, "MiB")]
        [DataRow(500L * FileSize.MiB, 500L, "MiB")]
        [DataRow(1L * FileSize.GiB, 1L, "GiB")]
        [DataRow(500L * FileSize.GiB, 500L, "GiB")]
        [DataRow(1L * FileSize.TiB, 1L, "TiB")]
        [DataRow(500L * FileSize.TiB, 500L, "TiB")]
        [DataRow(1L * FileSize.EiB, 1L, "EiB")]
        [DataRow(7L * FileSize.EiB, 7L, "EiB")]
        public void GetFormatPairInt64_ReturnsCorrectValues(long size, long expectedScaled, string expectedSuffix)
        {
            FileSize.GetFormatPair(size, out var scaled, out var suffix);
            Assert.AreEqual(expectedScaled, scaled);
            Assert.AreEqual(expectedSuffix, suffix);
        }

        [DataTestMethod]
        [DataRow(512.0, 512, "B")]
        [DataRow(1.0 * FileSize.KiB, 1, "KiB")]
        [DataRow(1.5 * FileSize.KiB, 1.5, "KiB")]
        [DataRow(500d * FileSize.KiB, 500, "KiB")]
        [DataRow(1.0 * FileSize.MiB, 1, "MiB")]
        [DataRow(1.5 * FileSize.MiB, 1.5, "MiB")]
        [DataRow(500d * FileSize.MiB, 500, "MiB")]
        [DataRow(1.0 * FileSize.GiB, 1, "GiB")]
        [DataRow(1.5 * FileSize.GiB, 1.5, "GiB")]
        [DataRow(500d * FileSize.GiB, 500, "GiB")]
        [DataRow(1.0 * FileSize.TiB, 1, "TiB")]
        [DataRow(1.5 * FileSize.TiB, 1.5, "TiB")]
        [DataRow(500d * FileSize.TiB, 500, "TiB")]
        [DataRow(1.0 * FileSize.EiB, 1, "EiB")]
        [DataRow(1.5 * FileSize.EiB, 1.5, "EiB")]
        [DataRow(500d * FileSize.EiB, 500, "EiB")]
        public void GetFormatPairDouble_ReturnsCorrectValues(double size, double expectedScaled, string expectedSuffix)
        {
            FileSize.GetFormatPair(size, out var scaled, out var suffix);
            Assert.AreEqual(expectedScaled, scaled);
            Assert.AreEqual(expectedSuffix, suffix);
        }
    }
}