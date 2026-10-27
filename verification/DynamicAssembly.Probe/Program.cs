using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using IVSoftware.Portable.Common;

// Observe first-chance exceptions as well as failures escaping discovery: the
// original handler swallowed the exception after breaking an attached debugger.
int unsupported = 0;
AppDomain.CurrentDomain.FirstChanceException += Observe;
void Observe(object? sender, FirstChanceExceptionEventArgs e)
{
    if (e.Exception is NotSupportedException)
        unsupported++;
}

bool beforeWarmup = args.Contains("before");
void DefineTransientType(string name)
{
    var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(name), AssemblyBuilderAccess.Run);
    assembly.DefineDynamicModule(name).DefineType("IVSoftware.TypeCacheProbe." + name,
        TypeAttributes.Public).CreateType();
}

if (beforeWarmup)
    DefineTransientType("Before");

var cache = Common.TypeCache;
if (!cache.ContainsKey(typeof(ITypeCache).FullName!))
    throw new Exception("Normal public type discovery failed.");

DefineTransientType("After");
cache.AppendNamespaceToCache("IVSoftware");
if (cache.Values.Any(type => type.Assembly.IsDynamic))
    throw new Exception("Transient dynamic types entered the discovery cache.");
if (!cache.ContainsKey(typeof(ITypeCache).FullName!))
    throw new Exception("Normal discovery was lost after a dynamic assembly load.");

AppDomain.CurrentDomain.FirstChanceException -= Observe;
if (unsupported != 0)
    throw new Exception($"Dynamic assembly discovery raised {unsupported} unsupported-operation exceptions.");
Console.WriteLine($"PASS: dynamic assemblies {(beforeWarmup ? "before and after" : "after")} cache initialization; no unsupported reflection exceptions.");
