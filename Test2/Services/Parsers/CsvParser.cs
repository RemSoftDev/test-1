using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Test2.Data.Models;
using Test2.Models;

namespace Test2.Services.Parsers
{
    public class CsvParser : IParser
    {
        private readonly List<Transaction> _transactions = new List<Transaction>();

        public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();
        public Report Report { get; } = new Report();

        public async Task<ReportStatus> ParseAsync(string fileName, Stream data)
        {
            if (Path.GetExtension(fileName).ToLower() != ".csv") return ReportStatus.UnknownFile;

            var reader = new StreamReader(data);
            var row = 0;
            while (!reader.EndOfStream)
            {
                row++;
                var report = new Report();
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var values = line.Split(',');
                if (values.Length != 5)
                    report.AddError($"{fileName} Row {row}: incorrect values count");
                else
                {
                    if (string.IsNullOrWhiteSpace(values[0]))
                        report.AddError($"{fileName} Row {row}: Transaction Id missing");
                    if (!decimal.TryParse(values[1], out var amount))
                        report.AddError($"{fileName} Row {row}: incorrect amount");
                    if (string.IsNullOrWhiteSpace(values[2]) || values[2].Length != 3)
                        report.AddError($"{fileName} Row {row}: incorrect currency");
                    if (!DateTime.TryParseExact(values[3], "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                        report.AddError($"{fileName} Row {row}: incorrect date");
                    TransactionStatus status = TransactionStatus.Unknown;
                    switch (values[4])
                    {
                        case "Approved":
                            status = TransactionStatus.Approved;
                            break;
                        case "Failed":
                            status = TransactionStatus.Rejected;
                            break;
                        case "Finished":
                            status = TransactionStatus.Done;
                            break;
                        default:
                            report.AddError($"{fileName} Row {row}: incorrect status");
                            break;
                    }

                    if (report.Status != ReportStatus.Failed)
                    {
                        _transactions.Add(new Transaction
                        {
                            Id = values[0],
                            Amount = amount,
                            Currency = values[2],
                            Date = date,
                            Status = status
                        });
                    }
                }

                Report.Concat(report);
            }
            if (Report.Status != ReportStatus.Failed && _transactions.Count > 0)
                Report.AddInfo($"{_transactions.Count} transactions imported from '{fileName}");

            return Report.Status;
        }
    }
}