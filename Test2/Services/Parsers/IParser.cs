using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Test2.Data.Models;
using Test2.Models;

namespace Test2.Services.Parsers
{
    public interface IParser
    {
        IReadOnlyList<Transaction> Transactions { get; }
        Report Report { get; }

        Task<ReportStatus> ParseAsync(string fileName, Stream data);
    }
}
