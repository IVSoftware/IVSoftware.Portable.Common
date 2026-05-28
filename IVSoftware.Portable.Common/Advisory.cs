namespace IVSoftware.Portable.Common.Exceptions
{
    public class Advisory : Throw
    {
        internal Advisory(
            Exception ex,
            string id)
            : base(ex, id, false)
        { }

        internal Advisory(
            Exception ex,
            string id,
            Enum? policyError)
            : base(ex, id, false, ThrowOrAdvise.Advisory, policyError)
        { }
    }
}
