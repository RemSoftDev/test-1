using System;
using System.Collections.Generic;
using System.Linq;

namespace Test2.Models
{
    public enum ReportStatus
    {
        Done,
        Failed,
        UnknownFile
    }

    public class Report
    {
        private readonly List<ReportItem> _items = new List<ReportItem>();

        public ReportStatus Status => _items.Any(i => i.Type == ReportItemType.Error) ? ReportStatus.Failed :
            _items.Any() ? ReportStatus.Done : ReportStatus.UnknownFile;
        public IReadOnlyList<ReportItem> Items => _items.AsReadOnly();

        public void AddError(string message) => _items.Add(new ReportItem { Type = ReportItemType.Error, Message = message});
        public void AddWarning(string message) => _items.Add(new ReportItem { Type = ReportItemType.Warning, Message = message });
        public void AddInfo(string message) => _items.Add(new ReportItem { Type = ReportItemType.Info, Message = message });

        public void Concat(Report report)
        {
            _items.AddRange(report._items);
        }
    }
}
