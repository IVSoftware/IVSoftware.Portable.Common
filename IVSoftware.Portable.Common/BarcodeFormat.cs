namespace IVSoftware.Portable.Common
{
    /// <summary>
    /// Selects the stable 6-4-4 payload format for a generated barcode.
    /// </summary>
    public enum BarcodeFormat
    {
        /// <summary>
        /// Advertises UTC date and minute followed by four random letters: yyMMdd-HHmm-AAAA.
        /// </summary>
        UtcTimestamp = 0,

        /// <summary>
        /// Uses fourteen random uppercase letters with the same grouping: AAAAAA-AAAA-AAAA.
        /// </summary>
        RandomLetters = 1,
    }
}
