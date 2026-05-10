## IVSoftware.Portable.Common [GitHub](https://github.com/IVSoftware/IVSoftware.Portable.Common.git)

A small foundational library that provides shared exception-signaling primitives and metadata attributes for the IVSoftware ecosystem. It supports higher-level packages by supplying a consistent, observable way to surface errors, advisories, and diagnostics without requiring Debug builds.

A core purpose of this package is to let **Release-mode NuGet packages** expose services similar to `System.Diagnostics` (e.g., logging, tracing, caller identification, advisory messages) through a controlled signaling channel, without shipping alternate Debug variants.

---

### Exception Signaling Model

All signaling flows through a common `Throw` event.  
This allows end-user developers (EUDs) to log, intercept, suppress, or escalate conditions according to their own policies, even when consuming a Release-mode package.

#### ThrowOrAdvise

An enum representing the intent of the signal:

- **ThrowHard** - Standard failure path. Throws unless explicitly suppressed.  
- **ThrowSoft** - Non-fatal condition. Never throws unless escalated.  
- **ThrowFramework** - Framework-side fault, defaulting to throw but downgradeable.  
- **Advisory** - Informational. Never affects control flow.

Supporting enums (`ThrowableStatus`, `ThrowToStringFormat`) describe post-raise conditions and how a `Throw` object formats itself.

---

### ThrowExtensions

Extension methods that raise errors or advisories in a structured, observable way:

- `ThrowHard<T>()`  
- `ThrowSoft<T>()`  
- `ThrowFramework<T>()`  
- `RethrowHard(...)`, `RethrowSoft(...)`, `RethrowFramework(...)`  
- `Advisory(...)`

Each call:

- Captures the caller identity.  
- Wraps the exception in a `Throw` object that includes an ID, message, and `Handled` flag.  
- Routes the signal through a static event.  
- Honors the caller's choice to suppress or escalate.  
- Behaves the same whether running under Debug or Release.

This enables Release-mode diagnostics without relying on `Debug.WriteLine` availability or a Debug-only NuGet variant.

---

### Advisory

A lightweight subclass of `Throw` representing informational messages.  
Advisories use the same event channel as faults but are always non-throwing and default to diagnostic output if unhandled.

---

### Attributes

The package includes attribute types used across IVSoftware packages:

- `CanonicalAttribute`  
- `CarefulAttribute`  
- `ProbationaryAttribute`  
- `ScaffoldingAttribute`  
- `UnsupportedAttribute`  
- `IndexerAttribute`

These carry intent and documentation value without enforcing runtime behavior.

---

### Extensions

`ToStrongNamedFriendAssembly(...)` generates a copy-ready
`InternalsVisibleTo` declaration for the assembly associated with an
instance.

It is intended for source-authoring workflows where a caller needs the full
strong-name public key text for a friend assembly declaration. If called on a
`Type` instance, or on an assembly that is not strong-named, it returns a
policy-violation message instead.

---

### Type Cache

`TypeCache` provides a lightweight reflection index over the types currently
visible in the AppDomain. It is useful when a package needs to discover or
match types by name without taking a hard project reference.

This is especially helpful for optional lateral integrations, where one
assembly may want to recognize a domain type from another assembly only when
that assembly is already present.

Core pieces:

- `Common.TypeCache` exposes the current cache as an `ITypeCache`.
- `GetAppDomainTypes(...)` queries the cache using a match pattern.
- `AppendNamespaceToCache(...)` adds namespace-prefixed types from already
  loaded assemblies and records that namespace for future `AssemblyLoad`
  events.

Important behavior:

- `GetAppDomainTypes(...)` defaults to
  `TypeCacheMatchMode.NamespaceStartsWith`.
- If you are looking for a type by name, choose an explicit mode such as
  `TypeFullNameExact` or `TypeFullNameEndsWith`.
- `AppendNamespaceToCache(...)` does not load assemblies by itself. It only
  indexes assemblies that are already loaded into the current AppDomain, plus
  any later assemblies that raise `AssemblyLoad`.

Typical usage:

```csharp
"IVSoftware.Portable.Collections.Events"
    .AppendNamespaceToCache();

var typeChanging =
    "IVSoftware.Portable.Collections.Events.NotifyCollectionChangingEventArgs"
    .GetAppDomainTypes(TypeCacheMatchMode.TypeFullNameExact)
    .Single();
```

That pattern allows a caller to perform inheritance-aware matching against a
known runtime type without introducing a compile-time reference.

---

### Purpose

`IVSoftware.Portable.Common` is intentionally minimal.  
Its role is to provide the shared vocabulary needed for consistent behavior across multiple NuGet packages:

- Unified exception and advisory signaling in Release builds,  
- Predictable logging and diagnostics without Debug-specific assemblies,  
- Lightweight annotations for conveying API intent.

It serves as infrastructure for other IVSoftware packages rather than a framework layer of its own.

---
