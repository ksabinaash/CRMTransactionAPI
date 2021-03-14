using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMTransactions.Models
{
    public class CallVolumeChart
    {
        public List<string> labs { get; set; }

        public List<string> callTypes { get; set; }

        public Dictionary<string, List<ChartMetrics>> volumeData { get; set; }

        public List<List<string>> countData { get; set; }
    }

}
