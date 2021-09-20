using System;

namespace BlockSearch.Common.Exceptions
{
    public class BlockNotFoundException : Exception
    {
        public BlockNotFoundException()
        {
        }

        public BlockNotFoundException(string message)
            : base(message)
        {
        }

        public BlockNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
