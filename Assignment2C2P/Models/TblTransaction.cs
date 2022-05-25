using System;
using System.Collections.Generic;

#nullable disable

namespace Assignment2C2P.Models
{
    public partial class TblTransaction
    {
        public int TransactionId { get; set; }
        public string Id { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Status { get; set; }
    }
}
