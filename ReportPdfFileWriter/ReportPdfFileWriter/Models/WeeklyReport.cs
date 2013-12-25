using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportPdfFileWriter.Models
{
    public class WeeklyReport
    {
        public string Logo { get; set; }
        public string ReportName { get; set; }
        public string LabelMonth { get; set; }
        public string LabelRevenue { get; set; }
        public string LabelForecast { get; set; }
        public string Currency { get; set; }
        public string LabelFooter { get; set; }
        public DateTime GeneratedOn { get; set; }
        public List<MonthData> Months { get; set; }

        public WeeklyReport()
        {
            this.Months = new List<MonthData>();
        }
    }

    public class MonthData
    {
        public string Month { get; set; }
        public decimal Revenue { get; set; }
        public decimal Forecast { get; set; }
    }
}