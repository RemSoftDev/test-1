using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test2.Models
{
    public enum ReportItemType
    {
        Info,
        Warning,
        Error
    }

    public class ReportItem
    {
        public ReportItemType Type { get; set; }
        public string Message { get; set; }
    }
}
