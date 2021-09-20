using System;

namespace BlockSearch.Common.Exceptions
{
    public class ServiceNotImplementedException : Exception
    {
        public ServiceNotImplementedException()
        {
        }

        public ServiceNotImplementedException(string message)
            : base(message)
        {
        }

        public ServiceNotImplementedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
