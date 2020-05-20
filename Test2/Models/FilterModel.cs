using System;
using Test2.Data.Models;

namespace Test2.Models
{
    public class FilterModel
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public TransactionStatus? Status { get; set; }
        public string Currency { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
