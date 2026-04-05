using System.Collections.ObjectModel;

namespace IVSoftware.Portable.Common
{
    public enum TypeCacheStringCompare
    {
        StartsWith,
        Contains,
    }
    public class TypeCache
    {
        /// <summary>
        /// Initialize a Cache on demand and return types that match the index.
        /// </summary>
        public Type[] this[string key, TypeCacheStringCompare compare = TypeCacheStringCompare.StartsWith]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(key))
                    return [];

                // Merge view: instance overlay wins if duplicate keys exist
                var source = Cache
                    .Concat(Cache)
                    .GroupBy(kvp => kvp.Key)
                    .Select(g => g.Last().Value);

                IEnumerable<Type> query = compare switch
                {
                    TypeCacheStringCompare.StartsWith =>
                        source.Where(t => t.FullName!.StartsWith(key)),

                    TypeCacheStringCompare.Contains =>
                        source.Where(t => t.FullName!.Contains(key)),

                    _ => []
                };
                return query.ToArray();
            }
            internal set
            {
                TODO
            }
        }

        // TODO use static ctor instead.
        Dictionary<string, Type> Cache
        {
            get
            {
                if (_cache is null)
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
                return _cache;
            }
        }
        Dictionary<string, Type>? _cache = null;

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
    }
    public static class TypeCacheExtensions
    {
        private static TypeCache TypeCache
        {
            get
            {
                if (_typeCache is null)
                {
                    _typeCache = new TypeCache();
                }
                return _typeCache;
            }
        }
        static TypeCache? _typeCache = null;

        public static IReadOnlyDictionary<string, Type> AppendNamespaceToCache(
            this string @namespace,
            params string[] moreNamespaces)
        {
            var includes = new[] { @namespace }.Concat(moreNamespaces).ToArray();

            var added = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetExportedTypes())
                .Where(t =>
                    !t.IsAbstract &&
                    !t.IsGenericType &&
                    Nullable.GetUnderlyingType(t) is null &&
                    t.Namespace is not null &&
                    includes.Any(x => t.Namespace.StartsWith(x)))
                .ToDictionary(t => t.FullName!, t => t);

            // merge (last write wins, but keys should be unique anyway)
            foreach (var kvp in added)
            {
                TypeCache[kvp.Key] = kvp.Value;
            }
            return new ReadOnlyDictionary<string, Type>(added);
        }

        public static Type[] GetAppDomainTypes(this string @namespace)
        {
            TODO
        }
    }
}
