using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRMTransactions.Models
{
    public class MissedCall
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string LabName { get; set; }

        public string LabPhoneNumber { get; set; }

        public string CustomerMobileNumber { get; set; }

        public DateTime EventTime { get; set; }

        public int? ValidCallId { get; set; }

        [ForeignKey("ValidCallId")]
        public ValidCall ValidCall { get; set; }
    }
}
