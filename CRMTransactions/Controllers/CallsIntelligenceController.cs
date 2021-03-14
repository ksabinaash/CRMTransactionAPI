using CRMTransactions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<CallVolumeChart>> GetAttendedCallsList(DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (fromDate == null)
                fromDate = DateTime.Now.AddDays(Convert.ToDouble(config.GetValue<string>("DefaultReportFromDate")));

            if (toDate == null)
                toDate = DateTime.Now;

            var labs = GetLabs().Result.Value;

            var MissedCallCharts = new List<ChartMetrics>();

            foreach (var item in labs)
            {
                ChartMetrics chartV2 = new ChartMetrics();
                chartV2.labName = item;
                MissedCallCharts.Add(chartV2);
            }

            var IncomingCallCharts = new List<ChartMetrics>();

            foreach (var item in labs)
            {
                ChartMetrics chartV2 = new ChartMetrics();
                chartV2.labName = item;
                IncomingCallCharts.Add(chartV2);
            }

            var OutGoingCallCharts = new List<ChartMetrics>();

            foreach (var item in labs)
            {
                ChartMetrics chartV2 = new ChartMetrics();
                chartV2.labName = item;
                OutGoingCallCharts.Add(chartV2);
            }

            var validCalls = await context.ValidCalls
                                              .Where(s => s.EventTime >= fromDate && s.EventTime <= toDate)
                                              .Select(x => new { x.LabName, x.ValidCallId, x.CallType })
                                              .ToListAsync();

            var missedCalls = await context.MissedCalls
                                           .Where(s => s.EventTime >= fromDate && s.EventTime <= toDate)
                                           .Select(x => new { x.LabName, x.Id, CallType = "Missed" })
                                           .ToListAsync();

            var groupedValidCalls = validCalls.GroupBy(vc => new { vc.LabName, vc.CallType })
                                             .Select(group => new
                                             {
                                                 Metric = group.Key,
                                                 Count = group.Count()
                                             }).ToList();

            var groupedMissedCalls = missedCalls.GroupBy(vc => new { vc.LabName, vc.CallType })
                                            .Select(group => new
                                            {
                                                Metric = group.Key,
                                                Count = group.Count()
                                            }).ToList();


            foreach (var item in groupedMissedCalls)
            {
                MissedCallCharts.Where(m => m.labName == item.Metric.LabName).ToList().ForEach(s => s.count = item.Count);
            }

            foreach (var item in groupedValidCalls)
            {
                IncomingCallCharts.Where(m => m.labName == item.Metric.LabName && item.Metric.CallType == "Incoming").ToList().ForEach(s => s.count = item.Count);

                OutGoingCallCharts.Where(m => m.labName == item.Metric.LabName && item.Metric.CallType == "Outgoing").ToList().ForEach(s => s.count = item.Count);
            }

            CallVolumeChart response = new CallVolumeChart();
            response.MissedCalls = MissedCallCharts;
            response.IncomingCalls = IncomingCallCharts;
            response.OutgoingCalls = OutGoingCallCharts;
            response.Labs = labs;
            response.MissedCallsCount = MissedCallCharts.Select(m => m.count.ToString()).ToList();
            response.IncomingCallsCount = IncomingCallCharts.Select(m => m.count.ToString()).ToList();
            response.OutgoingCallsCount = OutGoingCallCharts.Select(m => m.count.ToString()).ToList();

            return response;
        }

        [HttpGet]
        [Route("GetCallPurposeChartData")]
        public async Task<ActionResult<CallPurposeChart>> GetCallPurposeChartData(DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (fromDate == null)
                fromDate = DateTime.Now.AddDays(Convert.ToDouble(config.GetValue<string>("DefaultReportFromDate")));

            if (toDate == null)
                toDate = DateTime.Now;

            var labs = GetLabs().Result.Value;

            var callPurpose = await context.CallPurpose.ToListAsync();

            var purposes = callPurpose.Select(m => m.PurposeoftheCall).ToList();

            Dictionary<string, List<ChartMetrics>> purposeDictionary = new Dictionary<string, List<ChartMetrics>>();

            foreach (var item in purposes)
            {
                var purpose = new List<ChartMetrics>();

                foreach (var lab in labs)
                {
                    ChartMetrics chartV2 = new ChartMetrics();
                    chartV2.labName = lab;
                    purpose.Add(chartV2);
                }

                purposeDictionary.Add(item, purpose);
            }

            var validCalls = await context.ValidCalls
                                              .Where(s => s.EventTime >= fromDate && s.EventTime <= toDate)
                                              .Select(x => new { x.LabName, x.ValidCallId, x.CallPurpose })
                                              .ToListAsync();

            var groupedValidCalls = validCalls.GroupBy(vc => new { vc.LabName, vc.CallPurpose })
                                             .Select(group => new
                                             {
                                                 Metric = group.Key,
                                                 Count = group.Count()
                                             }).ToList();

            foreach (var item in groupedValidCalls)
            {
                var purpose = purposeDictionary[item.Metric.CallPurpose] as List<ChartMetrics>;

                purpose.Where(m => m.labName == item.Metric.LabName && item.Metric.CallPurpose == item.Metric.CallPurpose).ToList().ForEach(s => s.count = item.Count);
            }

            CallPurposeChart response = new CallPurposeChart();
            response.labs = labs;
            response.purposes = purposeDictionary.Keys.ToList();
            response.purposeData = purposeDictionary;
            response.countData = new List<List<string>>();
            foreach (var item in purposeDictionary.Values)
            {
                response.countData.Add(item.Select(m=> m.count.ToString()).ToList());
            }

            return response;
        }

        private async Task<ActionResult<List<String>>> GetLabs()
        {
            var ValidCallsLabs = await context.ValidCalls.Select(m => m.LabName).Distinct().ToListAsync();

            var missedCallsLabs = await context.MissedCalls.Select(m => m.LabName).Distinct().ToListAsync();

            List<string> labs = new List<string>();

            labs.AddRange(ValidCallsLabs);

            labs.AddRange(missedCallsLabs);

            labs = labs.Distinct().OrderBy(x => x).ToList();

            return labs;
        }
    }
}
