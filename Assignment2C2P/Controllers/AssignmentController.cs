using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Assignment2C2P.Common;
using Assignment2C2P.CustomModels;
using Assignment2C2P.Dao;
using Assignment2C2P.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;
using static Assignment2C2P.CommonString;

namespace Assignment2C2P.Controllers
{
    public class AssignmentController : Controller
    {
        private IWebHostEnvironment Environment;

        private readonly ILogger<AssignmentController> _logger;

        public AssignmentController(IWebHostEnvironment _environment, ILogger<AssignmentController> logger)
        {
            Environment = _environment;
            _logger = logger;
        }

        public IActionResult Assignment()
        {
            return View();
        }

        [ActionName("upload")]
        [HttpPost]
        public JsonResult UploadFile()
        {
            //TransactionResponseModel lst = new TransactionDao().GetTransactions();
            //_logger.LogInformation(lst.ToJson(true));
            ResponseModel model = new ResponseModel();
            TransactionResponseModel resultModel = new TransactionResponseModel();
            if (HttpContext.Request.Form.Files.Any())
            {
                var files = HttpContext.Request.Form.Files["file"];
                string fileName = Path.GetFileName(files.FileName);
                string fileExtension = Path.GetExtension(files.FileName);
                if (fileExtension == FileExtension.xml.GetDescription())
                {
                    resultModel = CheckXmlFileValidation(files);
                }
                else if (fileExtension == FileExtension.csv.GetDescription())
                {
                    resultModel = CheckCSVFileValidation(files);
                }
                else
                {
                    model.RespCode = "000";
                    model.RespDesp = "Unknown Format!";
                    return Json(model);
                }
            }

            model.RespCode = "000";
            model.RespDesp = "Success";
            //lst.Response = model;
            return Json(resultModel);
        }

        [ActionName("byCurrency")]
        [HttpPost]
        public JsonResult GetTransactionsByCurrency(TransactionModel reqModel)
        {
            TransactionResponseModel lst = new TransactionDao().GetTransactionsByCurrency(reqModel);
            return Json(lst);
        }

        [ActionName("byStatus")]
        [HttpPost]
        public JsonResult GetTransactionsByStatus(TransactionModel reqModel)
        {
            TransactionResponseModel lst = new TransactionDao().GetTransactionsByStatus(reqModel);
            return Json(lst);
        }

        [ActionName("byStateRange")]
        [HttpPost]
        public JsonResult GetTransactionsByDateRange(TransactionModel reqModel)
        {
            TransactionResponseModel lst = new TransactionDao().GetTransactionsByDateRange(reqModel);
            return Json(lst);
        }

        public TransactionResponseModel CheckXmlFileValidation(IFormFile file)
        {
            TransactionResponseModel resultModel = new TransactionResponseModel();
            try
            {
                XmlDocument doc = new XmlDocument();
                List<TransactionModel> model = new List<TransactionModel>();
                string fs = null;
                using (var rr = new StreamReader(file.OpenReadStream()))
                {
                    fs = rr.ReadToEnd();
                }
                doc.LoadXml(fs);
                foreach (XmlNode node in doc.SelectNodes("/Transactions/Transaction"))
                {
                    TransactionModel item = new TransactionModel();
                    if (node.Attributes["id"].Value.Length > 50)
                    {
                        _logger.LogInformation("Transaction id is not correct! that is : " + node.Attributes["id"].Value);
                        resultModel.Response.RespDesp = "Data Not Correct!";
                        resultModel.Response.RespType = Msg_ME;
                        resultModel.Response.SuccessStatausCode = ExceptionCode;
                        goto Result;
                    }
                    item.Id = node.Attributes["id"].Value;
                    if (!node["TransactionDate"].InnerText.isXmlCorrectDateFormat())
                    {
                        _logger.LogInformation("Transaction Date is not correct! that is : " + node["TransactionDate"].InnerText);
                        resultModel.Response.RespDesp = "Data Not Correct!";
                        resultModel.Response.RespType = Msg_ME;
                        resultModel.Response.SuccessStatausCode = ExceptionCode;
                        goto Result;
                    }
                    item.TransactionDate = Convert.ToDateTime(node["TransactionDate"].InnerText);
                    if (!node["Status"].InnerText.isStatus())
                    {
                        _logger.LogInformation("Status is not correct! that is : " + node["Status"].InnerText);
                        resultModel.Response.RespDesp = "Data Not Correct!";
                        resultModel.Response.RespType = Msg_ME;
                        resultModel.Response.SuccessStatausCode = ExceptionCode;
                        goto Result;
                    }
                    item.Status = node["Status"].InnerText;
                    XmlNodeList addrNodes = node.SelectNodes("PaymentDetails");
                    foreach (XmlNode addrn in addrNodes)
                    {
                        bool? isCurencyCode = DevCode.TryGetCurrency(addrn["CurrencyCode"].InnerText);
                        if (isCurencyCode == null || isCurencyCode == false)
                        //if (!addrn["CurrencyCode"].InnerText.isCurrencyCode())
                        {
                            _logger.LogInformation("Currency Code is not correct! that is : " + addrn["CurrencyCode"].InnerText);
                            resultModel.Response.RespDesp = "Data Not Correct!";
                            resultModel.Response.RespType = Msg_ME;
                            resultModel.Response.SuccessStatausCode = ExceptionCode;
                            goto Result;
                        }
                        item.CurrencyCode = addrn["CurrencyCode"].InnerText;
                        if (!addrn["Amount"].InnerText.isDecimal())
                        {
                            _logger.LogInformation("Amount is not correct! that is : " + addrn["Amount"].InnerText);
                            resultModel.Response.RespDesp = "Data Not Correct!";
                            resultModel.Response.RespType = Msg_ME;
                            resultModel.Response.SuccessStatausCode = ExceptionCode;
                            goto Result;
                        }
                        item.Amount = Convert.ToDecimal(addrn["Amount"].InnerText);
                    }
                    model.Add(item);
                }
                resultModel.lstTransaction = model;
                for (int i = 0; i < resultModel.lstTransaction.Count; i++)
                {
                    //string Status = resultModel.lstTransaction[i].Status;
                    //XmlStatus xmlStatus = Enum.Parse<XmlStatus>(resultModel.lstTransaction[i].Status);
                    //switch (xmlStatus)
                    //{
                    //    case XmlStatus.Approved:
                    //        Status = "A";
                    //        break;
                    //    case XmlStatus.Rejected:
                    //        Status = "R";
                    //        break;
                    //    case XmlStatus.Done:
                    //        Status = "D";
                    //        break;
                    //    default:
                    //        break;
                    //}
                    resultModel.lstResult.Add(new ResultModel()
                    {
                        id = resultModel.lstTransaction[i].Id,
                        payment = resultModel.lstTransaction[i].Amount.ToString() + " " + resultModel.lstTransaction[i].CurrencyCode,
                        Status = Enum.Parse<EnumStatus>(resultModel.lstTransaction[i].Status).GetDescription(),
                    });
                }
                var res = new TransactionDao().AddTransaction(resultModel);
                resultModel.Response.RespDesp = "Success Upload!";
                resultModel.Response.RespType = Msg_MS;
                resultModel.Response.SuccessStatausCode = SuccessStatausCode;
            }
            catch (Exception ex)
            {
                resultModel.Response.RespDesp = "Data Not Correct!";
                resultModel.Response.RespType = Msg_ME;
                resultModel.Response.SuccessStatausCode = ExceptionCode;
            }
        Result:
            return resultModel;
        }

        public TransactionResponseModel CheckCSVFileValidation(IFormFile file)
        {
            TransactionResponseModel resultModel = new TransactionResponseModel();
            try
            {
                if (file != null)
                {
                    var result = new StringBuilder();
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        while (reader.Peek() >= 0)
                            result.AppendLine(reader.ReadLine());
                    }
                    string csvData = result.ToString();
                    TextFieldParser parser = new TextFieldParser(new StringReader(csvData));
                    parser.HasFieldsEnclosedInQuotes = true;
                    parser.SetDelimiters(",");

                    string[] fields;

                    while (!parser.EndOfData)
                    {
                        fields = parser.ReadFields();
                        if (fields[0].Length > 50)
                        {
                            _logger.LogInformation("Transaction id is not correct! that is : " + fields[0]);
                            resultModel.Response.RespDesp = "Data Not Correct!";
                            resultModel.Response.RespType = Msg_ME;
                            resultModel.Response.SuccessStatausCode = ExceptionCode;
                            goto Result;
                        }
                        if (!fields[1].isDecimal())
                        {
                            _logger.LogInformation("Amount is not correct! that is : " + fields[1]);
                            resultModel.Response.RespDesp = "Data Not Correct!";
                            resultModel.Response.RespType = Msg_ME;
                            resultModel.Response.SuccessStatausCode = ExceptionCode;
                            goto Result;
                        }
                        bool? isCurencyCode = DevCode.TryGetCurrency(fields[2]);
                        if (isCurencyCode == null || isCurencyCode == false )
                        //if (!fields[2].isCurrencyCode())
                        {
                            _logger.LogInformation("Currency is not correct! that is : " + fields[2]);
                            resultModel.Response.RespDesp = "Data Not Correct!";
                            resultModel.Response.RespType = Msg_ME;
                            resultModel.Response.SuccessStatausCode = ExceptionCode;
                            goto Result;
                        }
                        if (!fields[3].isCsvCorrectDateFormat())
                        {
                            _logger.LogInformation("Transaction Date is not correct! that is : " + fields[3]);
                            resultModel.Response.RespDesp = "Data Not Correct!";
                            resultModel.Response.RespType = Msg_ME;
                            resultModel.Response.SuccessStatausCode = ExceptionCode;
                            goto Result;
                        }
                        if (!fields[4].isStatus())
                        {
                            _logger.LogInformation("Status is not correct! that is : " + fields[4]);
                            resultModel.Response.RespDesp = "Data Not Correct!";
                            resultModel.Response.RespType = Msg_ME;
                            resultModel.Response.SuccessStatausCode = ExceptionCode;
                            goto Result;
                        }
                        resultModel.lstTransaction.Add(new TransactionModel
                        {
                            Id = fields[0],
                            Amount = Convert.ToDecimal(fields[1]),
                            CurrencyCode = fields[2],
                            TransactionDate = DateTime.ParseExact(fields[3], "dd/MM/yyyy hh:mm:ss", null),
                            Status = fields[4],
                        });
                    }
                    parser.Close();
                }
                for (int i = 0; i < resultModel.lstTransaction.Count; i++)
                {
                    //string Status = resultModel.lstTransaction[i].Status;
                    //CSVStatus csvStatus = Enum.Parse<CSVStatus>(Status);
                    //switch (csvStatus)
                    //{
                    //    case CSVStatus.Approved:
                    //        Status = "A";
                    //        break;
                    //    case CSVStatus.Failed:
                    //        Status = "R";
                    //        break;
                    //    case CSVStatus.Finished:
                    //        Status = "D";
                    //        break;
                    //    default:
                    //        break;
                    //}
                    resultModel.lstResult.Add(new ResultModel()
                    {
                        id = resultModel.lstTransaction[i].Id,
                        payment = resultModel.lstTransaction[i].Amount.ToString() + " " + resultModel.lstTransaction[i].CurrencyCode,
                        Status = Enum.Parse<EnumStatus>(resultModel.lstTransaction[i].Status).GetDescription(),
                    });
                }
                var res = new TransactionDao().AddTransaction(resultModel);
                resultModel.Response.RespDesp = "Success Upload!";
                resultModel.Response.RespType = Msg_MS;
                resultModel.Response.SuccessStatausCode = SuccessStatausCode;
            }
            catch (Exception ex)
            {
                resultModel.Response.RespDesp = "Data Not Correct!";
                resultModel.Response.RespType = Msg_ME;
            }
        Result:
            return resultModel;
        }
    }
}
