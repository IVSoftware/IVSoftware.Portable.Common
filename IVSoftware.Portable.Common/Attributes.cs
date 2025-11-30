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

    public class ScaffoldingAttribute : Attribute
    {
    }

    public class UnsupportedAttribute : Attribute
    {
    }

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
    #endregion I V S    C A N O N I C A L
}
