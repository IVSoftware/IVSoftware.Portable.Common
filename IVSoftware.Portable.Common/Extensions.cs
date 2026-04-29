using IVSoftware.Portable.Common.Exceptions;

namespace IVSoftware.Portable.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Generates an InternalsVisibleTo declaration for the runtime assembly of the instance.
        /// </summary>
        /// <remarks>
        /// - Uses the assembly identity of <c>@this</c> to produce a friend assembly string.
        /// - Emits the full strong-name public key (not the token) as required by the attribute.
        /// - Throws if the assembly is not strong-named.
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
