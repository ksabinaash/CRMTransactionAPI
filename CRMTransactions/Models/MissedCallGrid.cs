using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMTransactions.Models
{
    public class MissedCallGrid
    {
        public int Id { get; set; }

        public string LabName { get; set; }

        public string LabPhoneNumber { get; set; }

        public string CustomerMobileNumber { get; set; }

        public DateTime EventTime { get; set; }

        public string CallBackStatus { get; set; }

        public string RespondedTime { get; set; }

        public string RespondedLabName { get; set; }

        public string RespondedLabPhoneNumber { get; set; }

        public string RespondedCustomerMobileNumber { get; set; }

        public DateTime RespondedEventTime { get; set; }

        public int RespondedCallDuration { get; set; }

        public string RespondedCallType { get; set; }

        public string CallPurpose { get; set; }

        public string Action { get; set; }

        public string Comment { get; set; }
    }
}
