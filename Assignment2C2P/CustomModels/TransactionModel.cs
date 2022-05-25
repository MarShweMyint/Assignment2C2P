using Assignment2C2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2C2P.CustomModels
{
    public class TransactionModel
    {
        public int TransactionId { get; set; }
        public string Id { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Status { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class TransactionResponseModel
    {
        public ResponseModel Response { get; set; } = new ResponseModel();
        public TransactionModel Transaction { get; set; } = new TransactionModel();
        public List<TransactionModel> lstTransaction { get; set; } = new List<TransactionModel>();
        public List<ResultModel> lstResult { get; set; } = new List<ResultModel>();
    }

    public class ResultModel
    {
        public string id { get; set; }
        public string payment { get; set; }
        public string Status { get; set; }
    }
}
