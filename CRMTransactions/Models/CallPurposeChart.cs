using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMTransactions.Models
{
    public class CallPurposeChart
    {
        public List<string> labs { get; set; }

        public List<string> purposes { get; set; }

        public Dictionary<string, List<ChartMetrics>> purposeData { get; set; }

        public List<List<string>> countData { get; set; }

        public List<string> sumData { get; set; }
    }
}
