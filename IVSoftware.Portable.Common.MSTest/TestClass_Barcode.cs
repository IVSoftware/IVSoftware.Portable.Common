using System.Globalization;
using System.Text.RegularExpressions;

namespace IVSoftware.Portable.Common.MSTest;

[TestClass]
public class TestClass_Barcode
{
    [TestMethod]
    public void DefaultEnumValueSelectsAdvertisedUtcTimestamp()
    {
        Assert.AreEqual(BarcodeFormat.UtcTimestamp, default(BarcodeFormat));
        var timestamp = new DateTimeOffset(2026, 10, 27, 1, 45, 0, TimeSpan.Zero);
        StringAssert.StartsWith(Barcode.Create(timestamp, default), "261027-0145-");
    }

    [TestMethod]
    public void RandomLettersPreservesGroupingWithoutTimestampDigits()
    {
        var timestamp = new DateTimeOffset(2026, 10, 27, 1, 45, 0, TimeSpan.Zero);
        for (var index = 0; index < 200; index++)
        {
            Assert.IsTrue(Regex.IsMatch(Barcode.Create(timestamp, BarcodeFormat.RandomLetters),
                "^[A-Z]{6}-[A-Z]{4}-[A-Z]{4}$"));
        }
        Assert.IsTrue(Regex.IsMatch(Barcode.NewBarcode(BarcodeFormat.RandomLetters),
            "^[A-Z]{6}-[A-Z]{4}-[A-Z]{4}$"));
    }

    [TestMethod]
    public void UnsupportedFormatIsRejectedAsCallerMisuse()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Barcode.NewBarcode((BarcodeFormat)99));
    }

    [TestMethod]
    public void FormatsUtcWithFourUppercaseLetters()
    {
        var timestamp = new DateTimeOffset(2026, 10, 27, 1, 45, 10, TimeSpan.Zero);
        for (var index = 0; index < 200; index++)
        {
            Assert.IsTrue(Regex.IsMatch(Barcode.Create(timestamp), "^261027-0145-[A-Z]{4}$"));
        }
    }

    [TestMethod]
    public void NormalizesOffsetAcrossDateBoundary()
    {
        var timestamp = new DateTimeOffset(2026, 12, 31, 23, 30, 0, TimeSpan.FromHours(-6));
        StringAssert.StartsWith(Barcode.Create(timestamp), "270101-0530-");
        StringAssert.StartsWith(Barcode.Create(timestamp.ToUniversalTime()), "270101-0530-");
    }

    [TestMethod]
    public void UsesGregorianDigitsRegardlessOfCurrentCulture()
    {
        var previousCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("th-TH");
            var timestamp = new DateTimeOffset(2026, 1, 2, 3, 4, 5, TimeSpan.Zero);
            StringAssert.StartsWith(Barcode.Create(timestamp), "260102-0304-");
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
        }
    }

    [TestMethod]
    public void SecondsAndFractionsAreOmittedWithoutRounding()
    {
        var timestamp = new DateTimeOffset(2026, 10, 27, 1, 45, 59, TimeSpan.Zero).AddTicks(9999999);
        StringAssert.StartsWith(Barcode.Create(timestamp), "261027-0145-");
    }

    [TestMethod]
    public void NewBarcodeCapturesCurrentUtcTime()
    {
        var before = DateTimeOffset.UtcNow.ToString("yyMMdd-HHmm", CultureInfo.InvariantCulture);
        var barcode = Barcode.NewBarcode();
        var after = DateTimeOffset.UtcNow.ToString("yyMMdd-HHmm", CultureInfo.InvariantCulture);
        Assert.IsTrue(Regex.IsMatch(barcode, "^[0-9]{6}-[0-9]{4}-[A-Z]{4}$"));
        var timestamp = barcode.Substring(0, 11);
        Assert.IsTrue(string.CompareOrdinal(before, timestamp) <= 0 && string.CompareOrdinal(timestamp, after) <= 0);
    }
}
