﻿using BlockSearch.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlockSearch.MVC.Models
{
    public class TransactionSearchModel
    {
        [Display(Name = "Block Number")]
        [Required(ErrorMessage = "Block Number is Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Block Number must be a positive number")]
        public int? BlockNumber { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Crypto Type")]
        public CryptoType? Crypto { get; set; }

        public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();

        public string ErrorMessage { get; set; }
    }
}
