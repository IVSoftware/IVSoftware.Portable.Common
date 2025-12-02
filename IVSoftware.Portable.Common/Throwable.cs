using IVSoftware.Portable.Common.Exceptions;

namespace IVSoftware.Portable.Common
{
    /// <summary>
    /// An IThrowable object has some agency in terms of being 
    /// able to resolve its own issues during the throw flow.
    /// </summary>
    public interface IThrowable
    {
        public void AppendThrow(Throw thrown);
        public Throw[] GetThrows();
        void ClearThrows();
        ThrowableStatus GetThrowableStatus();
    }
    public class Throwable : IThrowable
    {
        public Throw[] GetThrows() => _throws.ToArray();
        public ThrowableStatus GetThrowableStatus()
        {
            ThrowableStatus status = 0;
            if (Cancel) status |= ThrowableStatus.Cancel;
            if (_throwCount != 0) status |= ThrowableStatus.Thrown;
            if (_retryCount != 0) status |= ThrowableStatus.Retry;
            return status;
        }
        public void ClearThrows()
        {
            _throws.Clear();
            _retryCount++;
            _throwCount = 0;
        }
        public void AppendThrow(Throw thrown)
        {
            _throws.Add(thrown);
            _throwCount++;
        }

        private readonly List<Throw> _throws = new();
        int _throwCount = 0;
        int _retryCount = 0;

        public bool Cancel { get; set; }
    }
}
