# Dynamic assembly discovery probe

The Android debugger stack captured on October 27, 2026 stopped in
`AssemblyBuilder.GetExportedTypes`, called by Common's `AssemblyLoad` handler
while the modeled collection compiled a LINQ expression. Dynamic assemblies
are transient implementation details and are excluded from discovery.

This standalone, synthetic desktop probe checks public transient types created
before and after cache initialization, explicit namespace append, preservation
of normal public type discovery, and first-chance unsupported-operation
exceptions. Run the built executable twice, once with `before` and once without
arguments. Windows does not reproduce Android's unsupported-reflection behavior;
these checks establish discovery policy, not Android runtime equivalence.
The `before` case failed against cached `1.1.2-beta` with "Transient dynamic types
entered the discovery cache" and passed against the patched assembly. Both
patched process-order cases passed.

Restore using the installed package cache, then build Debug with
`/p:SignAssembly=false /p:GeneratePackageOnBuild=false
/p:TreatWarningsAsErrors=false /p:WarningsAsErrors=`. No network or app data is
required.

Common's project now declares `1.1.2-rc`; Dalikt references that standard package
version. The temporary `1.1.2-beta.dynamicfix001` package and Dalikt-specific feed
configuration are retired. Publication remains a separate, human-approved step.

The same scenarios are also covered by focused methods in the normal
`TestClass_TypeCache` MSTest fixture. The standalone probe remains useful for
checking cache initialization in a fresh process.