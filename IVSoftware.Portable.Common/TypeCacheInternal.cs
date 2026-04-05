using System.Collections.ObjectModel;

namespace IVSoftware.Portable.Common
{
    public enum TypeCacheMatchMode
    {
        NamespaceStartsWith,
        NamespaceContains,
        TypeFullNameEndsWith,
        TypeFullNameExact,
    }
    class TypeCacheInternal
    {
        public TypeCacheInternal()
        {
            _cache = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetExportedTypes())
            .Where(t =>
            !t.IsAbstract &&
            !t.IsGenericType &&
            Nullable.GetUnderlyingType(t) is null &&
            (t.Namespace is null || !Excludes.Any(x => t.Namespace.StartsWith(x))))
            .ToDictionary(t => t.FullName!, t => t);
        }
        Dictionary<string, Type> _cache;
        public Type[] this[string key, TypeCacheMatchMode compare = TypeCacheMatchMode.NamespaceStartsWith, bool ignoreCase = false]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(key))
                    return [];

                var comparison = ignoreCase
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal;

                var source = _cache.Values;

                IEnumerable<Type> query = compare switch
                {
                    TypeCacheMatchMode.NamespaceStartsWith =>
                        source.Where(t =>
                            t.Namespace is not null &&
                            t.Namespace.StartsWith(key, comparison)),

                    TypeCacheMatchMode.NamespaceContains =>
                        source.Where(t =>
                            t.Namespace is not null &&
                            t.Namespace.IndexOf(key, comparison) >= 0),

                    TypeCacheMatchMode.TypeFullNameEndsWith =>
                        source.Where(t =>
                            t.FullName!.EndsWith(key, comparison)),

                    TypeCacheMatchMode.TypeFullNameExact =>
                        source.Where(t =>
                            string.Equals(t.FullName, key, comparison)),

                    _ => []
                };

                return query.ToArray();
            }
        }

        private static readonly string[] Excludes =
        [
            "System",
            "Microsoft",
            "Windows",
            "MS",
            "SQLite",
            "Newtonsoft",
            "Azure",
            "Google",
            "Grpc",
            "Grpc.Core",
            "JetBrains",
            "Castle",
            "Autofac",
            "Serilog",
            "NLog",
            "xunit",
            "NUnit",
            "Moq",
        ];

        public IReadOnlyDictionary<string, Type> AppendNamespaceToCache(
            string @namespace,
            params string[] moreNamespaces)
        {
            if (string.IsNullOrWhiteSpace(@namespace) && (moreNamespaces is null || moreNamespaces.Length == 0))
                return new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>());

            var includes = new[] { @namespace }
                .Concat(moreNamespaces ?? [])
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            var added = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetExportedTypes())
                .Where(t =>
                    !t.IsAbstract &&
                    !t.IsGenericType &&
                    Nullable.GetUnderlyingType(t) is null &&
                    t.Namespace is not null &&
                    includes.Any(x => t.Namespace.StartsWith(x, StringComparison.Ordinal)))
                .ToDictionary(t => t.FullName!, t => t);

            // merge directly into cache (no indexer)
            foreach (var kvp in added)
            {
                _cache[kvp.Key] = kvp.Value;
            }

            return new ReadOnlyDictionary<string, Type>(added);
        }
    }
    public static class TypeCacheExtensions
    {
        private static TypeCacheInternal TypeCache
        {
            get
            {
                if (_typeCache is null)
                {
                    _typeCache = new TypeCacheInternal();
                }
                return _typeCache;
            }
        }
        static TypeCacheInternal? _typeCache = null;

        public static IReadOnlyDictionary<string, Type> AppendNamespaceToCache(
            this string @namespace,
            params string[] moreNamespaces)
        {
            return TypeCache.AppendNamespaceToCache(@namespace, moreNamespaces);
        }

        /// <summary>
        /// Initialize the cache on demand and return types that match the query.
        /// </summary>
        public static Type[] GetAppDomainTypes(
            this string @namespace,
            TypeCacheMatchMode mode = TypeCacheMatchMode.NamespaceStartsWith,
            bool ignoreCase = false)
        {
            if (string.IsNullOrWhiteSpace(@namespace))
                return [];

            return TypeCache[@namespace, mode, ignoreCase];
        }
    }
}
