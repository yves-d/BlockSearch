using System;

namespace BlockSearch.Application.Exceptions
{
    public class ClientNotImplementedException : Exception
    {
        public ClientNotImplementedException()
        {
        }

        public ClientNotImplementedException(string message)
            : base(message)
        {
        }

        public ClientNotImplementedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
