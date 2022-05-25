using Assignment2C2P.Common;
using Assignment2C2P.CustomModels;
using Assignment2C2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2C2P
{
    public static class DbModelToCustomModelService
    {
        public static TransactionModel Change(this TblTransaction item)
        {
            TransactionModel model = new TransactionModel
            {
                TransactionId = item.TransactionId,
                TransactionDate = item.TransactionDate.ToEFItem<DateTime>(),
                Amount = item.Amount.ToEFItem<Decimal>(),
                CurrencyCode = item.CurrencyCode,
                Status = item.Status,
                Id = item.Id,
            };
            return model;
        }

        public static TblTransaction Change(this TransactionModel item)
        {
            TblTransaction model = new TblTransaction();
            model.TransactionId = item.TransactionId;
            model.TransactionDate = item.TransactionDate.ToEFItem<DateTime>();
            model.Amount = item.Amount.ToEFItem<Decimal>();
            model.CurrencyCode = item.CurrencyCode;
            model.Status = item.Status;
            model.Id = item.Id;

            return model;
        }

        public static ResultModel ChangeResult(this TblTransaction item)
        {
            ResultModel model = new ResultModel
            {
                id = item.Id,
                payment = $@"{item.Amount.ToEFItem<Decimal>().ToString() } {item.CurrencyCode}",
                Status = Enum.Parse<EnumStatus>(item.Status).GetDescription(),
            };
            return model;
        }
    }
}
