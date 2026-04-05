using IVSoftware.Portable.Common.Attributes;
using System.Collections.ObjectModel;
using System.Reflection;

namespace IVSoftware.Portable.Common
{
    public interface ITypeCache : IReadOnlyDictionary<string, Type>
    {
        Type[] this[string key, TypeCacheMatchMode compare = TypeCacheMatchMode.NamespaceStartsWith, bool ignoreCase = false] { get; }

        IReadOnlyDictionary<string, Type> AppendNamespaceToCache(string @namespace, bool ignoreCase = false);
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
                    if (Excludes.Any(x => _.Namespace.StartsWith(x, StringComparison.Ordinal)))
                    {
                        return false;
                    }
                    return true;
                })
                .ToArray();

            _cache = exportedTypes
                .Where(t => t.FullName is not null)
                .ToDictionary(t => t.FullName!, t => t);

            AppDomain.CurrentDomain.AssemblyLoad += (sender, e) =>
            {
                Type[] types;

                try
                {
                    types = e.LoadedAssembly.GetExportedTypes();
                }
                catch (ReflectionTypeLoadException rtle)
                {
                    types = rtle.Types?.Where(t => t is not null).ToArray()!;
                }
                catch
                {
                    return; // skip problematic assemblies entirely
                }

                foreach (var t in types)
                {
                    if (t is null) continue;

                    var ns = t.Namespace;
                    if (ns is null) continue;

                    var isIncluded = Includes.Any(x => ns.StartsWith(x, StringComparison.Ordinal));
                    var isExcluded = Excludes.Any(x => ns.StartsWith(x, StringComparison.Ordinal));

                    if (!isIncluded && isExcluded)
                        continue;

                    var fullName = t.FullName;
                    if (fullName is null) continue;

                    _cache[fullName] = t;
                }
            };
        }

        internal Dictionary<string, Type> _cache;

        // -------------------------------
        // ITypeCache indexer (your custom)
        // -------------------------------
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

        // -------------------------------
        // IReadOnlyDictionary implementation
        // -------------------------------

        public Type this[string key] => _cache[key];

        public IEnumerable<string> Keys => _cache.Keys;

        public IEnumerable<Type> Values => _cache.Values;

        public int Count => _cache.Count;

        public bool ContainsKey(string key) => _cache.ContainsKey(key);

        public bool TryGetValue(string key, out Type value) => _cache.TryGetValue(key, out value!);

        public IEnumerator<KeyValuePair<string, Type>> GetEnumerator() => _cache.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _cache.GetEnumerator();

        // -------------------------------
        // Append
        // -------------------------------

        public IReadOnlyDictionary<string, Type> AppendNamespaceToCache(string @namespace, bool ignoreCase = false)
            => AppendNamespaceToCache(@namespace, null, ignoreCase);

        [Canonical]
        public IReadOnlyDictionary<string, Type> AppendNamespaceToCache(
            string @namespace,
            string[]? moreNamespaces,
            bool ignoreCase)
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

            // Persist includes
            foreach (var ns in includes)
            {
                Includes.Add(ignoreCase ? ns.ToLowerInvariant() : ns);
            }

            var added = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetExportedTypes())
                .Where(t =>
                {
                    var ns = t.Namespace;
                    if (ns is null) return false;

                    return includes.Any(x => ns.StartsWith(x, comparison));
                })
                .Where(t => t.FullName is not null)
                .ToDictionary(t => t.FullName!, t => t);

            foreach (var kvp in added)
            {
                _cache[kvp.Key] = kvp.Value;
            }

            return new ReadOnlyDictionary<string, Type>(added);
        }

        private HashSet<string> Includes { get; } = new(StringComparer.OrdinalIgnoreCase);

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
    public static class Common
    {
        public static ITypeCache TypeCache => TypeCacheExtensions.TypeCache;
    }

    public static class TypeCacheExtensions
    {
        internal static ITypeCache TypeCache
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
