using System.Collections.Generic;
using Test2.Data.Models;

namespace Test2.Models
{
    public class ParseResult
    {
        public Report Report { get; set; }
        public IReadOnlyList<Transaction> Transactions { get; set; }
    }
}
