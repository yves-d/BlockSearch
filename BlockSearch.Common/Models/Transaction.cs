﻿namespace BlockSearch.Common.Models
{
    public class Transaction
    {
        public string BlockHash { get; set; }
        public string BlockNumber { get; set; }
        public string Gas { get; set; }
        public string Hash { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Value { get; set; }
    }
}
