using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GUtils.Numerics.Tests
{
    [TestClass]
    public class SITests
    {
        [DataTestMethod]
        [DataRow ( 1, "" )]
        [DataRow ( SI.Yotta, "Y" )]
        [DataRow ( SI.Zetta, "Z" )]
        [DataRow ( SI.Exa, "E" )]
        [DataRow ( SI.Peta, "P" )]
        [DataRow ( SI.Tera, "T" )]
        [DataRow ( SI.Giga, "G" )]
        [DataRow ( SI.Mega, "M" )]
        [DataRow ( SI.Kilo, "k" )]
        [DataRow ( SI.Milli, "m" )]
        [DataRow ( SI.Micro, "μ" )]
        [DataRow ( SI.Nano, "n" )]
        [DataRow ( SI.Pico, "p" )]
        [DataRow ( SI.Femto, "f" )]
        [DataRow ( SI.Atto, "a" )]
        [DataRow ( SI.Zepto, "z" )]
        [DataRow ( SI.Yocto, "y" )]
        public void GetFormatPair_ReturnsCorrectValues ( Double scale, String expectedSuffix )
        {
            foreach ( var expectedScaled in new[] { 1, 1.5, 250, 500, 750, 900 } )
            {
                SI.GetFormatPair ( expectedScaled * scale, out var gottenScaled, out var gottenSuffix );
                Assert.AreEqual ( expectedScaled, gottenScaled, 0.0001, $"Expected {expectedScaled}{expectedSuffix} but got {gottenScaled}{gottenSuffix} (different numbers)" );
                Assert.AreEqual ( expectedSuffix, gottenSuffix, $"Expected {expectedScaled}{expectedSuffix} but got {gottenScaled}{gottenSuffix} (different suffixes)" );
            }
        }
    }
}
