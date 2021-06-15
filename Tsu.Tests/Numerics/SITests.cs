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

namespace Tsu.Numerics.Tests
{
    [TestClass]
    public class SITests
    {
        [DataTestMethod]
        [DataRow(1, "")]
        [DataRow(SI.Yotta, "Y")]
        [DataRow(SI.Zetta, "Z")]
        [DataRow(SI.Exa, "E")]
        [DataRow(SI.Peta, "P")]
        [DataRow(SI.Tera, "T")]
        [DataRow(SI.Giga, "G")]
        [DataRow(SI.Mega, "M")]
        [DataRow(SI.Kilo, "k")]
        [DataRow(SI.Milli, "m")]
        [DataRow(SI.Micro, "μ")]
        [DataRow(SI.Nano, "n")]
        [DataRow(SI.Pico, "p")]
        [DataRow(SI.Femto, "f")]
        [DataRow(SI.Atto, "a")]
        [DataRow(SI.Zepto, "z")]
        [DataRow(SI.Yocto, "y")]
        public void GetFormatPair_ReturnsCorrectValues(double scale, string expectedSuffix)
        {
            foreach (var expectedScaled in new[] { 1, 1.5, 250, 500, 750, 900 })
            {
                SI.GetFormatPair(expectedScaled * scale, out var gottenScaled, out var gottenSuffix);
                Assert.AreEqual(expectedScaled, gottenScaled, 0.0001, $"Expected {expectedScaled}{expectedSuffix} but got {gottenScaled}{gottenSuffix} (different numbers)");
                Assert.AreEqual(expectedSuffix, gottenSuffix, $"Expected {expectedScaled}{expectedSuffix} but got {gottenScaled}{gottenSuffix} (different suffixes)");
            }
        }
    }
}
