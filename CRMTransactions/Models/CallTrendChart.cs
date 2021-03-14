using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMTransactions.Models
{
    public class CallTrendChart
    {
        public List<string> labs { get; set; }
        public List<string> callTypes { get; set; }
        public List<string> period { get; set; }
        public string labName { get; set; }
        public Dictionary<string, List<ChartMetrics>> trendData { get; set; }

        public List<List<string>> countData { get; set; }
    }
}
