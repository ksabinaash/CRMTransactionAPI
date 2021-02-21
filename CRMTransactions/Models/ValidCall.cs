using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRMTransactions.Models
{
    public class ValidCall
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ValidCallId { get; set; }

        public string LabName { get; set; }

        public string LabPhoneNumber { get; set; }

        public string CustomerMobileNumber { get; set; }

        public DateTime EventTime { get; set; }

        public int CallDuration { get; set; }

        public string CallType { get; set; }

        public ICollection<MissedCall> MissedCalls { get; set; }
    }
}
