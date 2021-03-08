using CRMTransactions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMTransactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallsIntelligenceController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger logger;
        private readonly IConfiguration config;

        public CallsIntelligenceController(AppDbContext context, ILogger<CallsIntelligenceController> logger, IConfiguration config)
        {
            this.context = context;
            this.logger = logger;
            this.config = config;
        }
        [HttpGet]
        [Route("AttendedCallsList")]
        public async Task<ActionResult> GetAttendedCallsList(DateTime? fromDate = null, DateTime? toDate = null,  string? labName = null)
        {


            if (fromDate == null)
                fromDate = DateTime.Now.AddDays(Convert.ToDouble(config.GetValue<string>("DefaultReportFromDate")));


            if (toDate == null)
                toDate = DateTime.Now;


            //var ValidCallsList = context.ValidCalls.Where(x => x.EventTime >= fromDate.GetValueOrDefault()
            //                                                    && x.EventTime <= toDate.GetValueOrDefault()
            //                                                    && x.LabName.Equals( labName??x.LabName , StringComparison.InvariantCultureIgnoreCase)
            //                                                )
            //                                        .Select(x => new { x.LabName, x.ValidCallId,x.CallType })
            //                                        .GroupBy(vc => new { vc.LabName, vc.CallType });


            var ValidCallsList = context.ValidCalls.Where(x => x.LabName.Equals(labName ?? x.LabName, StringComparison.InvariantCultureIgnoreCase)
                                                         )
                                                 .Select(x => new { x.LabName, x.ValidCallId, x.CallType })
                                                 .GroupBy(vc => new { vc.LabName, vc.CallType });



            var missedCallsList = context.MissedCalls.Where(x => x.EventTime >= fromDate.GetValueOrDefault()
                                                                && x.EventTime <= toDate.GetValueOrDefault()
                                                                && x.LabName.Equals(labName ?? x.LabName, StringComparison.InvariantCultureIgnoreCase)
                                                           )
                                                            .Select(x=> new { x.LabName, x.Id })
                                                            .GroupBy(vc => new { vc.LabName });
            return null;



        }
    }
}
