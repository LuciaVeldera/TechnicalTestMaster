using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TechnicalTest.Models;

namespace TechnicalTest.Controllers
{
    public class TransactionDisplayController : ApiController
    {
        public HttpResponseMessage Get()
        {
            List<TransactionDetail> transactionList = new List<TransactionDetail>();
            using (TransactionMasterEntities dc = new TransactionMasterEntities())
            {
                transactionList = dc.TransactionDetails.OrderBy(a => a.TransactionIdentificator).ToList();
                HttpResponseMessage response;
                response = Request.CreateResponse(HttpStatusCode.OK, transactionList);
                return response;
            }
        }
    }
}
