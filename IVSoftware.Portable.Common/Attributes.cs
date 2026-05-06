using IVSoftware.Portable.Common.Exceptions;

namespace IVSoftware.Portable.Common.Attributes
{
    #region I V S    C A N O N I C A L 
    [Canonical("Source for this library and other IVS NuGets")]
    public class CanonicalAttribute : Attribute
    {
        public CanonicalAttribute(string? canon = null)
        {
            Canon = canon ?? string.Empty;
        }
        public string Canon { get; }
    }

    public class CarefulAttribute : Attribute
    {
        public CarefulAttribute(string? ofWhat = null)
        {
            OfWhat = ofWhat ?? string.Empty;
        }

        public string OfWhat { get; }
    }

    public class ProbationaryAttribute : Attribute
    {
        public ProbationaryAttribute(string? reason = null)
        {
            Reason = reason ?? string.Empty;
        }

        public string Reason { get; }
    }

    public class ScaffoldingAttribute : Attribute { }

    public class UnsupportedAttribute : Attribute { }

    /// <summary>
    /// This exists to make arbitrary indexer overloads easier to locate.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IndexerAttribute : Attribute
    {
        public IndexerAttribute(string? description = null)
        {
            if (description is not null)
            {
                Description = description;
            }
        }
        public IndexerAttribute(Type tKey, Type tValue)
        {
            TKey = tKey;
            TValue = tValue;
        }
        public Type? TKey { get; }
        public Type? TValue { get; }
        public string Description { get; } = string.Empty;
    }

    /// <summary>
    /// Associates a policy enum with its enforcing exception type.
    /// </summary>
    /// <remarks>
    /// Applied at the enum level to declare the exception that should be
    /// constructed when a policy member invokes Throw().
    /// </remarks>
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class PolicyAttribute : Attribute
    {
        public PolicyAttribute(Type exceptionType)
        {
            if (!typeof(Exception).IsAssignableFrom(exceptionType))
                throw new ArgumentException(
                    $"Type must derive from {nameof(Exception)}.",
                    nameof(exceptionType));

            ExceptionType = exceptionType;
        }

        public Type ExceptionType { get; }
    }

    /// <summary>
    /// Declares the default enforcement level for a policy member.
    /// </summary>
    /// <remarks>
    /// Applied to enum fields to indicate whether the violation should
    /// throw or advise according to the active ThrowOrAdvise semantics.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PolicyEnforcementAttribute : Attribute
    {
        public PolicyEnforcementAttribute(ThrowOrAdvise level)
        {
            Level = level;
        }
        public ThrowOrAdvise Level { get; }
    }

    /// <summary>
    /// Indicates that an enum must be treated as a discrete value set rather than a bit-field.
    /// </summary>
    /// <remarks>
    /// This attribute explicitly marks an enum as incompatible with flag semantics.
    /// It is intended for validation scenarios where APIs accept enums that are
    /// sometimes used with bitwise combinations.
    ///
    /// When applied, helper methods such as <c>HasFlags</c> or similar flag-inspection
    /// utilities should treat usage as invalid and may report an advisory or throw
    /// an exception depending on the configured error policy.
    ///
    /// The attribute exists to guard against accidental misuse where a caller
    /// attempts to apply flag logic to enums that were designed to represent
    /// mutually exclusive states rather than combinable capabilities.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Enum)]
    public class NotFlagsAttribute : Attribute { }

    /// <summary>
    /// Attribute for README documentation linking.
    /// </summary>
    public class ClaimAttribute : Attribute
    {
        public ClaimAttribute(string guid)
        {
            GUID = guid;
        }
        public string GUID { get; }
    }

    /// <summary>
    /// Advisory for signatures that look like they need refactoring
    /// but are part of a published contract. Basically, DON'T DO IT!
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class PublishedContractAttribute : Attribute
    {
        // Do Not Change signature, return type, or argument names.
        // Do Not Obsolete (if you can help it).
        public PublishedContractAttribute(
            string? version = null,
            Type? type = null,
            string? assembly = null)
        {
            Version = version;
            Type = type;
            Assembly = assembly;
        }
        public string? Version { get; }
        public string? Assembly { get; }
        public Type? Type { get; }
    }

    #endregion I V S    C A N O N I C A L
}
