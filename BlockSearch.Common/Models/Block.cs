using System.Collections.Generic;

namespace BlockSearch.Common.Models
{
    public class Block
    {
        public List<Transaction> Transactions { get; set; }

        public Block()
        {
            Transactions = new List<Transaction>();
        }
    }
}
