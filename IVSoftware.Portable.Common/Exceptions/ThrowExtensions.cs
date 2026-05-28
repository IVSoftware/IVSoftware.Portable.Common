using IVSoftware.Portable.Common.Attributes;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace IVSoftware.Portable.Common.Exceptions
{
    /// <summary>
    /// Extension methods for raising Throw and Advisory events. 
    /// These allow exceptions to be surfaced in a controlled way 
    /// that end-user developers can intercept, suppress, log, or 
    /// escalate. The goal is to centralize error signaling so that 
    /// framework calls never fail silently.
    /// 
    /// The goal is to create a best case scenario for the consumer of this 
    /// library: issues are always surfaced so nothing fails silently, yet the 
    /// choice to log, suppress, or escalate remains entirely theirs.
    /// 
    /// All flavors (Throw, ThrowSoft, Advisory) raise through the same 
    /// static event. Consumers can pattern match on the event args to 
    /// distinguish severity while maintaining a single point of contact.
    /// </summary>
    [Canonical("For this NuGet and others")]
    public static class ThrowExtensions
    {
        // COMMON USAGES
        // this.ThrowHard<NotImplementedException>("250926.A", "Some reason");
        // this.ThrowHard<NotImplementedException>("Some reason");

        /// <summary>
        /// Raises an exception of the specified type in a controlled way. 
        /// Wraps the created exception in a Throw object that tracks the 
        /// message, the caller identity, and whether the exception was 
        /// handled. 
        /// </summary>
        /// <remarks> 
        /// The 'Id' option allows text searching as an alternative to call stack tracing.
        /// Contrast with ThrowSoft:
        /// - ThrowHard raises the exception by default. To suppress requires 
        ///   explicitly passing @throw = false at the call site.
        /// 
        /// - ThrowSoft never raises the exception itself; it always 
        ///   returns a Throw object, leaving any decision to rethrow 
        ///   entirely to the caller.
        ///   
        /// Why it matters:
        /// - Provides a single point to raise exceptions consistently. 
        /// - Ensures the caller identity is always captured even when a 
        ///   message override is supplied. 
        /// - Allows subscribers to observe or intercept via the Throw 
        ///   event before the exception escapes. 
        /// - Honors the project failure policy by throwing only when 
        ///   requested or when not explicitly suppressed.
        ///   </remarks>
        public static Throw ThrowHard<T>(
            this object? sender,
            string? messageOrId = null,
            string? messageOnly = null,
            bool? @throw = null,
            [CallerMemberName] string? caller = null)
        {
            if (string.IsNullOrWhiteSpace(messageOrId))
            {
                messageOrId = typeof(T).Name;
            }
            string? id = messageOnly is null ? caller! : messageOrId;
            string? msg = messageOnly ?? messageOrId;

            var ex = Activator.CreateInstance(typeof(T), [msg]) as Exception ?? new Exception(msg);
            var e = new Throw(ex, id, @throw);
            e.RaiseSelf(sender, e);
            if (!e.Handled && @throw != false)
            {
                throw ex;
            }
            return e;
        }

        /// <summary>
        /// Rethrow an existing exception.
        /// </summary>
        /// <remarks>
        /// The 'Id' option allows easier text serching as an alternative to call stack tracing.
        /// </remarks>
        public static Throw RethrowHard(
            this object? sender,
            Exception ex,
            string? messageOrId = null,
            string? messageOnly = null,
            bool? @throw = null,
            [CallerMemberName] string? caller = null)
        {
            if (string.IsNullOrWhiteSpace(messageOrId))
            {
                messageOrId = ex.GetType().Name;
            }
            string? id = messageOnly is null ? caller! : messageOrId;
            string? msg = messageOnly ?? messageOrId;
            var e = new Throw(ex, id, @throw);
            e.RaiseSelf(sender, e);
            if (!e.Handled && @throw != false)
            {
                throw ex;
            }
            return e;
        }

        /// <summary>
        /// Raises an exception of the specified type in a controlled way 
        /// but never actually throws it. The exception is wrapped in a 
        /// Throw object that carries the message, caller identity, and 
        /// optional handling state.
        /// </summary>
        /// <remarks>
        /// The 'Id' option allows text searching as an alternative to call stack tracing.
        /// Contrast with ThrowHard:
        /// - ThrowSoft never raises the exception itself and cannot be coerced to do so.
        /// - ThrowHard, on the other hand, will raise the exception whenever it is unhandled 
        ///   unless suppression is explicitly requested with @throw = false.
        /// 
        /// Why it matters:
        /// - Useful for logging, diagnostics, or advisory flows where 
        ///   consistency of exception creation is desired but control 
        ///   flow should never be interrupted. 
        /// - Preserves the same event and tracking mechanism without 
        ///   risking a runtime fault.
        /// </remarks>
        public static Throw ThrowSoft<T>(
            this object? sender,
            string? messageOrId = null,
            string? messageOnly = null,
            bool? @throw = null,
            [CallerMemberName] string? caller = null)
        {
            if (string.IsNullOrWhiteSpace(messageOrId))
            {
                messageOrId = typeof(T).Name;
            }
            string? id = messageOnly is null ? caller! : messageOrId;
            string? msg = messageOnly ?? messageOrId;

            var ex = Activator.CreateInstance(typeof(T), [msg]) as Exception ?? new Exception(msg);
            var e = new Throw(ex, id, @throw);

            // HANDLED By Default
            e.Handled = @throw != true;
            e.RaiseSelf(sender, e);

            if (!e.Handled)
            {
                throw ex;
            }
            return e;
        }

        /// <summary>
        /// Rethrow an existing exception.
        /// </summary>
        public static Throw RethrowSoft(
            this object? sender,
            Exception ex,
            string? messageOrId = null,
            string? messageOnly = null,
            bool? @throw = null,
            [CallerMemberName] string? caller = null)
        {
            if (string.IsNullOrWhiteSpace(messageOrId))
            {
                messageOrId = ex.GetType().Name;
            }
            string? id = messageOnly is null ? caller! : messageOrId;
            string? msg = messageOnly ?? messageOrId;
            var e = new Throw(ex, id, @throw);

            // HANDLED By Default
            e.Handled = @throw != true;
            e.RaiseSelf(sender, e);

            if (!e.Handled)
            {
                throw ex;
            }
            return e;
        }

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
        public static Throw ThrowFramework<T>(
            this object? sender,
            string? messageOrId = null,
            string? messageOnly = null,
            bool @throw = true,
            [CallerMemberName] string? caller = null)
        {
            if (string.IsNullOrWhiteSpace(messageOrId))
            {
                messageOrId = typeof(T).Name;
            }
            string? id = messageOnly is null ? caller! : messageOrId;
            string? msg = messageOnly ?? messageOrId;

            var ex = Activator.CreateInstance(typeof(T), [msg]) as Exception ?? new Exception(msg);
            var e = new Throw(ex, id, @throw);

            // ThrowFramework defaults to a hard throw
            // but is downgradeable to soft via @throw.
            if(@throw == false)
            {
                e.Handled = true;
            }
            e.RaiseSelf(sender, e);

            if (!e.Handled)
            {
                throw ex;
            }
            return e;
        }

        /// <summary>
        /// Rethrow an existing exception.
        /// </summary>
        public static Throw RethrowFramework(
            this object? sender,
            Exception ex,
            string? messageOrId = null,
            string? messageOnly = null,
            bool? @throw = null,
            [CallerMemberName] string? caller = null)
        {
            if (string.IsNullOrWhiteSpace(messageOrId))
            {
                messageOrId = ex.GetType().Name;
            }
            string? id = messageOnly is null ? caller! : messageOrId;
            string? msg = messageOnly ?? messageOrId;
            var e = new Throw(ex, id, @throw);

            // HANDLED By Default
            e.Handled = @throw != true;
            e.RaiseSelf(sender, e);

            if (!e.Handled)
            {
                throw ex;
            }
            return e;
        }

        // Most common. this.Advisory<NotImplementedException>("250926.A", "Some reason");
        // Also common. this.Advisory<NotImplementedException>("Some reason");

        /// <summary>
        /// Raises an advisory event using the same static Throw channel, 
        /// but without any intent to disrupt control flow. The advisory 
        /// wraps the message in an Exception for consistency, yet is 
        /// always treated as informational.
        /// </summary>
        /// <remarks>
        /// Contrast with Throw / ThrowSoft:
        /// - Throw may raise an exception into control flow if not suppressed. 
        /// - ThrowSoft never raises but still signals a potential error. 
        /// - Advisory never represents an error condition at all; it is a 
        ///   diagnostic or guidance signal that logs by default when unhandled. 
        /// 
        /// Why it matters:
        /// - Lets framework code communicate advisories through the same 
        ///   interception path as real errors. 
        /// - Gives end-user developers a unified hook for both faults and 
        ///   guidance, while keeping the advisory flavor explicitly non-fatal.
        /// </remarks>
        public static Advisory Advisory(
            this object sender,
            string messageOrId,
            string? messageOnly = null,
            [CallerMemberName] string? caller = null)
        {
            string id = messageOnly is null ? caller! : messageOrId;
            string msg = messageOnly ?? messageOrId;

            var ex = new Exception(msg);
            var e = new Advisory(ex, id);
            e.RaiseSelf(sender, e);
            if (!e.Handled)
            {
                Debug.WriteLine(e.FormattedMessage);
            }
            return e;
        }

        /// <summary>
        /// Raises the exception or advisory defined by a policy enum member.
        /// </summary>
        /// <remarks>
        /// The enum type may declare the exception type with
        /// <see cref="PolicyAttribute"/>, while the member may declare the
        /// enforcement level with <see cref="PolicyEnforcementAttribute"/>
        /// and the message with <see cref="DescriptionAttribute"/>.
        /// </remarks>
        public static Throw ThrowPolicyException(
            this object? sender,
            Enum policyMember,
            [CallerMemberName] string? caller = null)
        {
            sender ??= policyMember;

            var enumType = policyMember.GetType();

            // Policy enums must not be [Flags]
            if (enumType.GetCustomAttribute<FlagsAttribute>() is not null)
            {
                return sender.ThrowHard<InvalidOperationException>(
                    messageOnly: $"Policy enum {enumType.Name} must not be marked with [Flags].",
                    caller: caller);
            }

            // Enforcement level
            var policyEnforcement =
                policyMember.GetCustomAttribute<PolicyEnforcementAttribute>() is { } attrP
                ? attrP.Level
                : ThrowOrAdvise.ThrowHard;

            // Exception type declared at enum level
            var policyExceptionType =
                enumType.GetCustomAttribute<PolicyAttribute>() is { } attrE
                ? attrE.ExceptionType
                : typeof(InvalidOperationException);

            // Message
            var msg =
                policyMember.GetCustomAttribute<DescriptionAttribute>() is { } attrM
                ? attrM.Description
                : policyMember.ToString();

            // Stable searchable id
            var id = $"{enumType.Name}.{policyMember}";

            if (policyEnforcement == ThrowOrAdvise.Advisory)
            {
                var advisory = new Advisory(new Exception(msg), id, policyMember);
                advisory.RaiseSelf(sender, advisory);
                if (!advisory.Handled)
                {
                    Debug.WriteLine(advisory.FormattedMessage);
                }
                return advisory;
            }

            var ex =
                Activator.CreateInstance(policyExceptionType, [msg]) as Exception
                ?? new Exception(msg);

            var e = new Throw(ex, id, null, policyEnforcement, policyMember);

            if (policyEnforcement == ThrowOrAdvise.ThrowSoft)
            {
                e.Handled = true;
            }

            e.RaiseSelf(sender, e);

            if (!e.Handled)
            {
                throw ex;
            }

            return e;
        }

        /// <summary>
        /// Retrieves a single attribute applied to the enum member.
        /// </summary>
        /// <remarks>
        /// Intended for non-composite enum values.
        /// 
        /// If the enum is marked with [Flags] and represents a combined value,
        /// member resolution via ToString() may not map to a declared field,
        /// and this method will surface through the Throw channel.
        /// </remarks>
        internal static T? GetCustomAttribute<T>(this Enum @this) where T : Attribute
        {
            var type = @this.GetType();

            var members = type
                .GetMember(@this.ToString())
                .ToArray();

            switch (members.Length)
            {
                case 0:
                    @this.ThrowHard<InvalidOperationException>(
                        $"Member not found: {type.Name}.{@this}");
                    return null;

                case 1:
                    var candidates = members[0]
                        .GetCustomAttributes(typeof(T), inherit: false)
                        .Cast<T>()
                        .ToArray();

                    switch (candidates.Length)
                    {
                        case 0:
                            return null;

                        case 1:
                            return candidates[0];

                        default:
                            @this.ThrowHard<InvalidOperationException>(
                                $"Multiple attributes of type {typeof(T).Name} on {type.Name}.{@this}");
                            return null;
                    }
                default:
                    @this.ThrowHard<InvalidOperationException>(
                        $"Member is ambiguous: {type.Name}.{@this}");
                    return null;
            }
        }
    }
}
