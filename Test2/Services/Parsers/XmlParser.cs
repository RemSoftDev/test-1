using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Test2.Data.Models;
using Test2.Models;

namespace Test2.Services.Parsers
{
    public class XmlParser : IParser
    {
        private readonly List<Transaction> _transactions = new List<Transaction>();

        public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();
        public Report Report { get; } = new Report();

        public async Task<ReportStatus> ParseAsync(string fileName, Stream data)
        {
            if (Path.GetExtension(fileName).ToLower() != ".xml") return ReportStatus.UnknownFile;

            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(data);
            }
            catch (Exception e)
            {
                Report.AddError($"{fileName} filed load xml : {e.Message}");
                return ReportStatus.Failed;
            }

            foreach (XmlNode node in xml.GetElementsByTagName("Transaction"))
            {
                var report = new Report();
                var id = node.Attributes["Id"]?.Value;
                DateTime? date = null;
                decimal? amount = null;
                string currency = null;
                TransactionStatus? status = null;

                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (subNode.Name == "TransactionDate")
                    {
                        if (DateTime.TryParseExact(subNode.InnerText, "", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out var d))
                            date = d;
                    }
                    else if (subNode.Name == "PaymentDetails")
                    {
                        foreach (XmlNode paymentNode in subNode.ChildNodes)
                        {
                            if (paymentNode.Name == "Amount")
                            {
                                if (decimal.TryParse(paymentNode.InnerText, out var t))
                                    amount = t;
                            }
                            else if (paymentNode.Name == "CurrencyCode")
                            {
                                if (paymentNode.InnerText.Length == 3)
                                    currency = paymentNode.InnerText;
                            }
                            else
                            {
                                report.AddError($"{fileName} unexpected tag {paymentNode.Name}");
                            }
                        }
                    }
                    else if (subNode.Name == "Status")
                    {
                        switch (subNode.InnerText)
                        {
                            case "Approved":
                                status = TransactionStatus.Approved;
                                break;
                            case "Rejected":
                                status = TransactionStatus.Rejected;
                                break;
                            case "Done":
                                status = TransactionStatus.Done;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        report.AddError($"{fileName} unexpected tag {subNode.Name}");
                    }
                }

                if (string.IsNullOrWhiteSpace(id))
                    report.AddError($"{fileName} Transaction Id missing");
                if (!date.HasValue)
                    report.AddError($"{fileName} Date missing");
                if (!amount.HasValue)
                    report.AddError($"{fileName} Amount missing");
                if (string.IsNullOrWhiteSpace(currency))
                    report.AddError($"{fileName} Currency missing");
                if (!status.HasValue)
                    report.AddError($"{fileName} Status missing");

                if (report.Status != ReportStatus.Failed)
                {
                    _transactions.Add(new Transaction
                    {
                        Id = id,
                        Amount = amount.Value,
                        Currency = currency,
                        Date = date.Value,
                        Status = status.Value
                    });
                }

                Report.Concat(report);
            }

            if (Report.Status != ReportStatus.Failed && _transactions.Count > 0)
                Report.AddInfo($"{_transactions.Count} transactions imported from '{fileName}");

            return Report.Status;
        }
    }
}
