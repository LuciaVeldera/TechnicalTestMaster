using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace TechnicalTest.Models
{
    [Serializable]
    [XmlRoot("TransactionDetail")]
    public class TransactionMetaData
    {
        [XmlElement("TransactionIdentificator")]
        public int TransactionIdentificator { get; set; }
        [XmlElement("Amount")]
        public int Amount { get; set; }
        [XmlElement("CurrencyCode")]
        public int CurrencyCode { get; set; }
        [XmlElement("TransactionDate")]
        public int TransactionDate { get; set; }
        [XmlElement("Status")]
        public int Status { get; set; }
    }

    [MetadataType(typeof(TransactionMetaData))]
    public partial class TransactionDetail
    {
    }
}