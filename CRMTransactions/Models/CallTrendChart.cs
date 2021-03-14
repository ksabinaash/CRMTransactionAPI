using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMTransactions.Models
{
    public class CallTrendChart
    {
        public List<string> Labs { get; set; }
        public List<string> Period { get; set; }
        public string LabName { get; set; }
        public Dictionary<string, List<ChartMetrics>> trendData { get; set; }

        public List<List<string>> countData { get; set; }
    }
}
