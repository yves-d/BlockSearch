using System;

namespace BlockSearch.Application.Exceptions
{
    public class InitialisationFailureException : Exception
    {
        public InitialisationFailureException()
        {
        }

        public InitialisationFailureException(string message)
            : base(message)
        {
        }

        public InitialisationFailureException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
