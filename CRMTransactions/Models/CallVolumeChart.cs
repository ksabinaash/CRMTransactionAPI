using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMTransactions.Models
{
    public class CallVolumeChart
    {
        public List<ChartMetrics> MissedCalls { get; set; }
        public List<ChartMetrics> IncomingCalls { get; set; }
        public List<ChartMetrics> OutgoingCalls { get; set; }
        public List<string> Labs { get; set; }
        public List<string> MissedCallsCount { get; set; }
        public List<string> IncomingCallsCount { get; set; }
        public List<string> OutgoingCallsCount { get; set; }

    }

}
