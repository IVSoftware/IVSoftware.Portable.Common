namespace IVSoftware.Portable.Common.Exceptions
{
    public class Advisory : Throw
    {
        internal Advisory(
            Exception ex,
            string id)
            : base(ex, id, false)
        { }
    }
}
