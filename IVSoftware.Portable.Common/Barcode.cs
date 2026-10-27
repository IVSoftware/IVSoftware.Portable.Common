using System.Globalization;
using System.Security.Cryptography;

namespace IVSoftware.Portable.Common
{
    /// <summary>
    /// Generates probabilistically unique payloads for infrequent physical-item scan labels.
    /// </summary>
    /// <remarks>
    /// - The default UtcTimestamp format advertises yyMMdd-HHmm followed by four random uppercase letters.
    /// - UTC time has whole-minute precision; neither the original offset nor century is retained.
    /// - The timestamp suffix provides 456,976 combinations per minute; uniqueness is not guaranteed.
    /// - RandomLetters uses fourteen random uppercase letters with the same 6-4-4 grouping and no timestamp.
    /// - Generate once and persist the opaque value; reuse it when rendering, printing, or exporting.
    /// - A barcode is optional application metadata, not a required identity for every record.
    /// - Returns payload text; barcode and QR rendering belong to a separate consumer layer.
    /// </remarks>
    public static class Barcode
    {
        /// <summary>
        /// Captures the current UTC time as a barcode payload.
        /// </summary>
        public static string NewBarcode() => NewBarcode(BarcodeFormat.UtcTimestamp);

        /// <summary>
        /// Creates a barcode in the selected format, capturing current UTC time when applicable.
        /// </summary>
        public static string NewBarcode(BarcodeFormat format) => Create(DateTimeOffset.UtcNow, format);

        /// <summary>
        /// Formats the supplied instant in UTC and appends a freshly generated random suffix.
        /// </summary>
        public static string Create(DateTimeOffset timestamp)
            => Create(timestamp, BarcodeFormat.UtcTimestamp);

        /// <summary>
        /// Creates a barcode in the selected format; RandomLetters does not encode the supplied timestamp.
        /// </summary>
        public static string Create(DateTimeOffset timestamp, BarcodeFormat format)
        {
            switch (format)
            {
                case BarcodeFormat.UtcTimestamp:
                    return timestamp.UtcDateTime.ToString("yyMMdd-HHmm-", CultureInfo.InvariantCulture)
                        + CreateRandomLetters(4);
                case BarcodeFormat.RandomLetters:
                    var letters = CreateRandomLetters(14);
                    return letters.Substring(0, 6) + "-" + letters.Substring(6, 4) + "-" + letters.Substring(10, 4);
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, "Unsupported barcode format.");
            }
        }

        private static string CreateRandomLetters(int count)
        {
            using (var random = RandomNumberGenerator.Create())
            {
                var suffix = new char[count];
                var sample = new byte[1];
                for (var index = 0; index < suffix.Length; index++)
                {
                    // Reject the partial group so each of the 26 letters is equally likely.
                    do
                    {
                        random.GetBytes(sample);
                    }
                    while (sample[0] >= 234);
                    suffix[index] = (char)('A' + sample[0] % 26);
                }
                return new string(suffix);
            }
        }
    }
}
