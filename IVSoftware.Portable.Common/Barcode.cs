using System.Globalization;
using System.Security.Cryptography;

namespace IVSoftware.Portable.Common
{
    /// <summary>
    /// Generates human-readable barcode payloads with UTC date/time and four random uppercase letters.
    /// </summary>
    /// <remarks>
    /// - Format: yyMMdd-HHmm-AAAA, where AAAA represents independently sampled letters A through Z.
    /// - Uses a 24-hour UTC clock at whole-minute precision; the original UTC offset is not retained.
    /// - The random suffix provides 456,976 combinations per minute; uniqueness is not guaranteed.
    /// - The two-digit year does not identify a century.
    /// - Returns payload text; barcode image rendering belongs to the consumer.
    /// </remarks>
    public static class Barcode
    {
        /// <summary>
        /// Captures the current UTC time as a barcode payload.
        /// </summary>
        public static string NewBarcode() => Create(DateTimeOffset.UtcNow);

        /// <summary>
        /// Formats the supplied instant in UTC and appends a freshly generated random suffix.
        /// </summary>
        public static string Create(DateTimeOffset timestamp)
        {
            using (var random = RandomNumberGenerator.Create())
            {
                var suffix = new char[4];
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
                return timestamp.UtcDateTime.ToString("yyMMdd-HHmm-", CultureInfo.InvariantCulture)
                    + new string(suffix);
            }
        }
    }
}
