namespace IVSoftware.Portable.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Formats an InternalsVisibleTo attribute for the assembly that defines this instance.
        /// </summary>
        /// <remarks>
        /// Emits the full strong-name public key (not the token).
        /// Throws if the assembly is not strong-named.
        /// Intended for generating friend assembly declarations.
        /// </remarks>
        public static string ToStrongNamedFriendAssembly(this object @this)
        {
            var asm = @this.GetType().Assembly;
            var name = asm.GetName();
            var publicKey = name.GetPublicKey();

            if (publicKey is null || publicKey.Length == 0)
                throw new InvalidOperationException("Assembly is not strong-named.");

            var hex = BitConverter
                .ToString(publicKey)
                .Replace("-", string.Empty)
                .ToLowerInvariant();

            return $@"[assembly: InternalsVisibleTo(""{name.Name}, PublicKey={hex}"")]";
        }
    }
}
