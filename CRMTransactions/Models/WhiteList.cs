using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRMTransactions.Models
{
    public class WhiteList
    {
        [Key]
        public string MobileNumber { get; set; }

        public string Name { get; set; }
    }
}
