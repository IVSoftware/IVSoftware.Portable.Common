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
    #endregion I V S    C A N O N I C A L
}
