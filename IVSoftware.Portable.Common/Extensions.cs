using IVSoftware.Portable.Common.Exceptions;

namespace IVSoftware.Portable.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Generates a copy-ready <c>InternalsVisibleTo</c> declaration for the assembly associated with the instance.
        /// </summary>
        /// <remarks>
        /// - Intended for ephemeral code-generation workflows where the resulting attribute text is copied into source.
        /// - Uses the assembly identity of <c>@this</c> to produce a friend assembly string.
        /// - Emits the full strong-name public key, not the public key token, as required by <c>InternalsVisibleTo</c>.
        /// - Returns a policy-violation sentinel string when called on an instance of <see cref="Type"/> or when the assembly is not strong-named.
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
