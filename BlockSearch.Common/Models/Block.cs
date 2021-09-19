using BlockSearch.Common.Enums;
using System.Collections.Generic;

namespace BlockSearch.Common.Models
{
    public class Block
    {
        public string Hash { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public CryptoType Crypto { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
