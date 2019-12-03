using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using TechnicalTest.Models;

namespace TechnicalTest.Controllers
{
    public class ImportTransactionController : Controller
    {
        // GET: ImportTransaction
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                string fileExtension = Path.GetExtension(postedFile.FileName);
                if(fileExtension == ".csv" || fileExtension == ".xml")
                {
                    try
                    {
                        
                        if (fileExtension == ".csv")
                        {
                            List<TransactionDetail> transactionList = new List<TransactionDetail>();
                            using (var reader = new StreamReader(postedFile.InputStream))
                            {
                                while (!reader.EndOfStream)
                                {
                                    string input = reader.ReadLine();
                                    string pattern = @"""\s*,\s*""";
                                    string[] rows = Regex.Split(input.Substring(1, input.Length - 2), pattern);
                                    transactionList.Add(new TransactionDetail
                                    {
                                        TransactionIdentificator = rows[0].ToString(),
                                        Amount = Convert.ToDecimal(rows[1].ToString()),
                                        CurrencyCode = rows[2].ToString(),
                                        TransactionDate = DateTime.ParseExact(rows[3].ToString(),"dd/MM/yyyy HH:mm:ss",null),
                                        Status = rows[4].ToString()
                                    });
                                }
                            }

                            using (TransactionMasterEntities db = new TransactionMasterEntities())
                            {
                                foreach (var i in transactionList)
                                {
                                    var v = db.TransactionDetails.Where(a => a.TransactionIdentificator.Equals(i.TransactionIdentificator)).FirstOrDefault();

                                    if (v != null)
                                    {
                                        v.TransactionIdentificator = i.TransactionIdentificator;
                                        v.Amount = i.Amount;
                                        v.CurrencyCode = i.CurrencyCode;
                                        v.TransactionDate = i.TransactionDate;
                                        v.Status = i.Status;
                                    }
                                    else
                                    {
                                        db.TransactionDetails.Add(i);
                                    }
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            var transactionList = new List<TransactionDetail>();
                            var xmlPath = Server.MapPath("~/FileUpload" + postedFile.FileName);
                            postedFile.SaveAs(xmlPath);
                            XDocument xDoc = XDocument.Load(xmlPath);
                            transactionList = xDoc.Descendants("Transaction").Select
                            (transactionDetail => new TransactionDetail
                            {
                                TransactionIdentificator = transactionDetail.Attribute("id").Value.ToString(),
                                Amount = Convert.ToDecimal(transactionDetail.Element("PaymentDetails").Element("Amount").Value),
                                CurrencyCode = transactionDetail.Element("PaymentDetails").Element("CurrencyCode").Value.ToString(),
                                TransactionDate = Convert.ToDateTime(transactionDetail.Element("TransactionDate").Value),
                                Status = transactionDetail.Element("Status").Value.ToString()
                            }).ToList();

                            using (TransactionMasterEntities db = new TransactionMasterEntities())
                            {
                                foreach (var i in transactionList)
                                {
                                    var v = db.TransactionDetails.Where(a => a.TransactionIdentificator.Equals(i.TransactionIdentificator)).FirstOrDefault();

                                    if (v != null)
                                    {
                                        v.TransactionIdentificator = i.TransactionIdentificator;
                                        v.Amount = i.Amount;
                                        v.CurrencyCode = i.CurrencyCode;
                                        v.TransactionDate = i.TransactionDate;
                                        v.Status = i.Status;
                                    }
                                    else
                                    {
                                        db.TransactionDetails.Add(i);
                                    }
                                    db.SaveChanges();
                                }
                            }
                        }
                        ViewBag.Success = "HTTP Code 200";
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Bad Request";
                    }
                
                }
                else
                {
                    ViewBag.Error = "Unknown Format";
                }
            }
            else
            {
                ViewBag.Error = "Bad Request";
            }
            
            return View("Index");
        }
    }
}