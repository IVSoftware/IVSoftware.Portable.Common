using System.Collections.ObjectModel;

namespace IVSoftware.Portable.Common
{
    public interface ITypeCache : IReadOnlyDictionary<string, Type>
    {
        Type[] this[string key, TypeCacheMatchMode compare = TypeCacheMatchMode.NamespaceStartsWith, bool ignoreCase = false] { get; }
    }
    public enum TypeCacheMatchMode
    {
        NamespaceStartsWith,
        NamespaceContains,
        TypeFullNameEndsWith,
        TypeFullNameExact,
    }
    class TypeCacheInternal : ITypeCache
    {
        public TypeCacheInternal()
        {
            var exportedTypes = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(_ => _.GetExportedTypes())
            .Where(_ => 
            {
                if (_.Namespace is null) return false;
                if(Excludes.Any(x => _.Namespace.StartsWith(x)))
                {
                    return false;
                }
                return true;
            })
            .ToArray();

            _cache = exportedTypes
                .Where(t => t.FullName is not null)
                .ToDictionary(t => t.FullName!, t => t);
        }
        internal Dictionary<string, Type> _cache;
        public Type[] this[string key, TypeCacheMatchMode compare = TypeCacheMatchMode.NamespaceStartsWith, bool ignoreCase = false]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(key) || key == "*")
                    return _cache.Values.ToArray();

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
            string[]? moreNamespaces = null,
            bool ignoreCase = false)
        {
            var comparison = ignoreCase
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            var includes = new[] { @namespace }
                .Concat(moreNamespaces ?? [])
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            if (includes.Length == 0)
                return new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>());

            var added = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetExportedTypes())
                .Where(t =>
                    t.Namespace is not null &&
                    includes.Any(x => t.Namespace.StartsWith(x, comparison)))
                .Where(t => t.FullName is not null)
                .ToDictionary(t => t.FullName!, t => t);

            foreach (var kvp in added)
            {
                _cache[kvp.Key] = kvp.Value;
            }

            return new ReadOnlyDictionary<string, Type>(added);
        }
    }
    public static class Common
    {
        public static ITypeCache TypeCache => TypeCacheExtensions.TypeCache;
    }

    public static class TypeCacheExtensions
    {
        internal static TypeCacheInternal TypeCache
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
            this string @namespace)
        {
            return TypeCache.AppendNamespaceToCache(@namespace);
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
