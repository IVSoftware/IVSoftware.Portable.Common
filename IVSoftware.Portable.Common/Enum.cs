namespace IVSoftware.Portable.Common.Exceptions
{
    /// <summary>
    /// Reports error conditions and debug trace messages 
    /// to End User Developer (EUD) - the package client.
    /// Supports debugging of the Release Mode NuGet package.
    /// </summary>
    public enum ThrowOrAdvise
    {
        /// <summary>
        /// Framework does not expect to be able to recover.
        /// If EUD disagrees, they can handle.
        /// Otherwise, framework will throw upon return.
        /// </summary>
        ThrowHard = 1,

        /// <summary>
        /// Useful for Try patterns, with or without their 
        /// own @throw argument. This event is Handled when raised.
        /// It represents a non-critical condition, and will not
        /// throw upon return unless the EUD un-handles it. The
        /// exception can also be thrown by the EUD on their end.
        /// </summary>
        ThrowSoft,

        /// <summary>
        /// Indicates an error on the part of the Internal Framework Developer.
        /// </summary>
        /// <remarks>
        /// This is a self-own that tells the End User Developer that they need 
        /// to be aware of a critical condition that isn't their fault. 
        /// Main options here are:
        /// - Protect your data
        /// - Find a different option at the call site.
        /// - Report issue on the GitHub repo.
        /// </remarks>
        ThrowFramework,

        /// <summary>
        /// A log item or rough equivalent of Debug.WriteLine.
        /// </summary>
        Advisory,
    }

    [Flags]
    public enum ThrowableStatus
    {
        OK = 0,
        Cancel = 0x1,
        Thrown = Cancel << 1,
        Retry = Thrown << 1,
    }

    [Flags]
    public enum ThrowToStringFormat
    {
        Mode = 0x01,
        ExceptionType = Mode << 1,
        MessageId = ExceptionType << 1,
        Message = MessageId << 1,
        StackTrace = Message << 1,
        InnerException = StackTrace << 1,

        Basic = MessageId | Message,
        MSTest = Mode | ExceptionType | MessageId | Message,
        Forensic = Mode | ExceptionType | MessageId | Message | StackTrace | InnerException,
    }

    public enum Win32Message
    {
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSELEAVE = 0x02A3,
        WM_NCMOUSEMOVE = 0x00A0,
        WM_MOUSEHOVER = 0x02A1,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_CONTEXTMENU = 0x007B,
    }
}
