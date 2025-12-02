using IVSoftware.Portable.Common.Attributes;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace IVSoftware.Portable.Common.Exceptions
{
    /// <summary>
    /// Provides a static entry point for raising exceptions that end-user 
    /// developers can intercept and custom route. For example, in dev's
    /// own code this can be raised as a debug assert only, even
    /// when running against the compiled NuGet package. 
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public class Throw : HandledEventArgs
    {
        internal Throw(
            Exception ex,
            string messageId,
            bool? @throw,
            [CallerMemberName] string? caller = null)
        {
            Exception = ex;
            Message = ex?.Message ?? "No information is available about this error.";
            MessageId = messageId!;
            ThrowRequestedAtCallSite = @throw;
            Mode =
                Enum.TryParse(caller, out ThrowOrAdvise mode)
                ? mode
                : 0;
        }

        public ThrowOrAdvise Mode { get; }

        /// <summary>
        /// Internal use only.
        /// </summary>
        internal Action<object?, Throw> RaiseSelf => (o, e) => BeginThrowOrAdvise?.Invoke(o, e);

        public static EventHandler<Throw>? BeginThrowOrAdvise;
        public Exception? Exception { get; }

        [Careful("Do 'not' generate automatically.")]
        public string MessageId { get; } = "[Caller]";
        public string Message { get; }
        public string FormattedMessage => $"{MessageId} | {Message}";
        public bool? ThrowRequestedAtCallSite { get; }

        public override string ToString()
            => FormattedMessage;

        public string ToString(ThrowToStringFormat format = ThrowToStringFormat.MessageId | ThrowToStringFormat.Message)
        {
            var builder = new List<string>();
            if (format.HasFlag(ThrowToStringFormat.Mode))
            {
                builder.Add(Mode.ToString());
            }
            if (format.HasFlag(ThrowToStringFormat.ExceptionType) && Exception is not null)
            {
                builder.Add($"Type: {Exception.GetType().Name}");
            }
            if (format.HasFlag(ThrowToStringFormat.MessageId) && !string.IsNullOrWhiteSpace(MessageId))
            {
                builder.Add($"Id: {MessageId}");
            }
            if (format.HasFlag(ThrowToStringFormat.Message) && !string.IsNullOrWhiteSpace(Message))
            {
                builder.Add(Message);
            }
            if (format.HasFlag(ThrowToStringFormat.StackTrace) && Exception is not null)
            {
                builder.Add(Exception.StackTrace ?? string.Empty);
            }
            if (format.HasFlag(ThrowToStringFormat.InnerException) && Exception?.InnerException != null)
            {
                builder.Add($"Inner: {Exception.InnerException.Message}");
            }
            var preview = string.Join(Environment.NewLine, builder);
            return preview;
        }
    }
}
