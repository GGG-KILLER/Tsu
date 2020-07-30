using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GUtils.Numerics.Tests
{
    [TestClass]
    public class FileSizeTests
    {
        [DataTestMethod]
        [DataRow ( 512.0, 512, "B" )]
        [DataRow ( 1.0 * FileSize.KiB, 1, "KiB" )]
        [DataRow ( 1.5 * FileSize.KiB, 1.5, "KiB" )]
        [DataRow ( 500d * FileSize.KiB, 500, "KiB" )]
        [DataRow ( 1.0 * FileSize.MiB, 1, "MiB" )]
        [DataRow ( 1.5 * FileSize.MiB, 1.5, "MiB" )]
        [DataRow ( 500d * FileSize.MiB, 500, "MiB" )]
        [DataRow ( 1.0 * FileSize.GiB, 1, "GiB" )]
        [DataRow ( 1.5 * FileSize.GiB, 1.5, "GiB" )]
        [DataRow ( 500d * FileSize.GiB, 500, "GiB" )]
        [DataRow ( 1.0 * FileSize.TiB, 1, "TiB" )]
        [DataRow ( 1.5 * FileSize.TiB, 1.5, "TiB" )]
        [DataRow ( 500d * FileSize.TiB, 500, "TiB" )]
        [DataRow ( 1.0 * FileSize.EiB, 1, "EiB" )]
        [DataRow ( 1.5 * FileSize.EiB, 1.5, "EiB" )]
        [DataRow ( 500d * FileSize.EiB, 500, "EiB" )]
        public void GetFormatPair_ReturnsCorrectValues ( Double size, Double expectedScaled, String expectedSuffix )
        {
            (var scaled, var suffix) = FileSize.GetFormatPair ( size );
            Assert.AreEqual ( expectedScaled, scaled );
            Assert.AreEqual ( expectedSuffix, suffix );
        }
    }
}