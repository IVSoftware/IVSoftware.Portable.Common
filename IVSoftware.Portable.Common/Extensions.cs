using IVSoftware.Portable.Common.Exceptions;

namespace IVSoftware.Portable.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Generates a copy-ready <c>InternalsVisibleTo</c> declaration for the assembly associated with the instance.
        /// </summary>
        /// <remarks>
        /// - Development-time source authoring: copy the generated InternalsVisibleTo annotation into source.
        ///   This helper is not intended for production runtime policy enforcement.
        /// - Uses the assembly identity of <c>@this</c> to produce a friend assembly string.
        /// - Emits the full strong-name public key, not the public key token, as required by <c>InternalsVisibleTo</c>.
        /// - Pass an instance from the intended assembly, not typeof(...): GetType() on a Type object
        ///   identifies its runtime implementation rather than the assembly represented by that Type.
        /// - Type objects and unsigned assemblies return "Policy Violation:" text instead of an annotation.
        ///   These diagnostics do not raise Throw.BeginThrowOrAdvise; inspect the result before copying it.
        /// - Introduced April 5, 2026 (9110d06), originally throwing for unsigned assemblies.
        ///   April 29 (ef9e285) added the intentional Type guard and replaced the unsigned-assembly
        ///   exception with diagnostic text. That exception-to-string change affects behavioral compatibility.
        /// </remarks>
        public static string ToStrongNamedFriendAssembly(this object @this)
        {
            if (@this is Type)
            {
                return "Policy Violation: Cannot make a friend class designation for an instance of Type.";
            }

            var asm = @this.GetType().Assembly;
            var name = asm.GetName();
            var publicKey = name.GetPublicKey();

            if (publicKey is null || publicKey.Length == 0)
            {
                return "Policy Violation: Assembly must be strong-named.";
            }

            var hex = BitConverter
                .ToString(publicKey)
                .Replace("-", string.Empty)
                .ToLowerInvariant();

            return $@"[assembly: InternalsVisibleTo(""{name.Name}, PublicKey={hex}"")]";
        }
    }
}
