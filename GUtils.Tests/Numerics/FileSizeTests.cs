using System;
using GUtils.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GUtils.Tests.Numerics
{
    [TestClass]
    public class FileSizeTests
    {
        [DataTestMethod]
        [DataRow ( 512L, 512L, "B" )]
        [DataRow ( 1L * FileSize.KiB, 1L, "KiB" )]
        [DataRow ( 500L * FileSize.KiB, 500L, "KiB" )]
        [DataRow ( 1L * FileSize.MiB, 1L, "MiB" )]
        [DataRow ( 500L * FileSize.MiB, 500L, "MiB" )]
        [DataRow ( 1L * FileSize.GiB, 1L, "GiB" )]
        [DataRow ( 500L * FileSize.GiB, 500L, "GiB" )]
        [DataRow ( 1L * FileSize.TiB, 1L, "TiB" )]
        [DataRow ( 500L * FileSize.TiB, 500L, "TiB" )]
        [DataRow ( 1L * FileSize.EiB, 1L, "EiB" )]
        [DataRow ( 7L * FileSize.EiB, 7L, "EiB" )]
        public void GetFormatPairInt64_ReturnsCorrectValues ( Int64 size, Int64 expectedScaled, String expectedSuffix )
        {
            (var scaled, var suffix) = FileSize.GetFormatPair ( size );
            Assert.AreEqual ( expectedScaled, scaled );
            Assert.AreEqual ( expectedSuffix, suffix );
        }

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
        public void GetFormatPairDouble_ReturnsCorrectValues ( Double size, Double expectedScaled, String expectedSuffix )
        {
            (var scaled, var suffix) = FileSize.GetFormatPair ( size );
            Assert.AreEqual ( expectedScaled, scaled );
            Assert.AreEqual ( expectedSuffix, suffix );
        }
    }
}