using Assignment2C2P.Common;
using Assignment2C2P.CustomModels;
using Assignment2C2P.Models;
using Assignment2C2P.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Assignment2C2P.CommonString;

namespace Assignment2C2P.Dao
{
    public class TransactionDao
    {
        private readonly AssignmentContext db;

        public TransactionDao()
        {
            db = new DBService().db;
        }

        public TransactionResponseModel GetTransactions()
        {
            List<TransactionModel> lst = new List<TransactionModel>();
            lst = db.TblTransactions.Select(x => x.Change()).ToList();
            var lstresult = db.TblTransactions.Select(x => x.ChangeResult()).ToList();
            //if (requestModel.IsSelect)
            //{
            //    lst.Insert(0, new S_VarientCategoryModel { VarientCategoryId = 0, VarientCategoryName = "--Select One--" });
            //}
            return new TransactionResponseModel { lstTransaction = lst, lstResult = lstresult };
        }

        public TransactionResponseModel GetTransactionsByDateRange(TransactionModel reqModel)
        {
            List<TransactionModel> lst = new List<TransactionModel>();
            lst = db.TblTransactions.Where(x => x.TransactionDate >= reqModel.FromDate && x.TransactionDate <= reqModel.ToDate).Select(x => x.Change()).ToList();
            var lstresult = db.TblTransactions.Select(x => x.ChangeResult()).ToList();
            return new TransactionResponseModel { lstTransaction = lst, lstResult = lstresult };
        }

        public TransactionResponseModel GetTransactionsByCurrency(TransactionModel reqModel)
        {
            List<TransactionModel> lst = new List<TransactionModel>();
            lst = db.TblTransactions.Where(x => x.CurrencyCode == reqModel.CurrencyCode).Select(x => x.Change()).ToList();
            //if (requestModel.IsSelect)
            //{
            //    lst.Insert(0, new S_VarientCategoryModel { VarientCategoryId = 0, VarientCategoryName = "--Select One--" });
            //}
            return new TransactionResponseModel { lstTransaction = lst };
        }

        public TransactionResponseModel GetTransactionsByStatus(TransactionModel reqModel)
        {
            List<TransactionModel> lst = new List<TransactionModel>();
            lst = db.TblTransactions.Where(x => x.Status == reqModel.Status).Select(x => x.Change()).ToList();
            var lstresult = db.TblTransactions.Select(x => x.ChangeResult()).ToList();
            return new TransactionResponseModel { lstTransaction = lst, lstResult = lstresult };
        }

        public TransactionResponseModel AddTransaction(TransactionResponseModel requestModel)
        {
            TransactionResponseModel model = new TransactionResponseModel();
            db.AddRange(requestModel.lstTransaction.Select(x => x.Change()));
            bool res = db.SaveChanges().ToBoolFromDB();
            model.Response = new ResponseModel() { RespType = Msg_MS };
            return model;
        }
    }
}
