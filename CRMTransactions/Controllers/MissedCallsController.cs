using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMTransactions.Models;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace CRMTransactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissedCallsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger logger;
        private readonly IConfiguration config;

        public MissedCallsController(AppDbContext context, ILogger<ValidCallsController> logger, IConfiguration config)
        {
            this.context = context;
            this.logger = logger;
            this.config = config;
        }

        // GET: api/MissedCalls/
        [HttpGet]
        [Route("GetMissedCalls")]
        public async Task<ActionResult<IEnumerable<MissedCall>>> GetMissedCalls()
        {
            return await context.MissedCalls.Include("ValidCall").ToListAsync();
        }

        // GET: api/MissedCalls/GetMissedCallsForGrid
        [HttpGet]
        [Route("GetMissedCallsForGrid")]
        public async Task<ActionResult<IEnumerable<MissedCallGrid>>> GetMissedCallsForGrid(DateTime? dateTime = null)
        {
            var missedCalls = await context.MissedCalls.Include("ValidCall").ToListAsync();

            if (dateTime == null)
                dateTime = DateTime.Now.AddDays(Convert.ToDouble(config.GetValue<string>("DaysFilter")));

            List<MissedCallGrid> result = new List<MissedCallGrid>();

            foreach (var call in missedCalls.Where(m => m.EventTime >= dateTime))
            {
                if (call.ValidCallId == null)
                {
                    MissedCallGrid item = new MissedCallGrid
                    {
                        CustomerMobileNumber = call.CustomerMobileNumber,
                        EventTime = call.EventTime,
                        Id = call.Id,
                        LabName = call.LabName,
                        LabPhoneNumber = call.LabPhoneNumber,
                        IsWhiteListedCall = call.IsWhiteListed,
                        CustomerName = call.CustomerName,
                        CallBackStatus = config.GetValue<string>("NotCalledBackMsg")
                    };
                    result.Add(item);
                }
                else
                {
                    TimeSpan ts = call.ValidCall.EventTime - call.EventTime;

                    MissedCallGrid item = new MissedCallGrid
                    {
                        CustomerMobileNumber = call.CustomerMobileNumber,
                        EventTime = call.EventTime,
                        Id = call.Id,
                        LabName = call.LabName,
                        LabPhoneNumber = call.LabPhoneNumber,
                        CallBackStatus = config.GetValue<string>("CalledBackMsg"),
                        RespondedCallType = call.ValidCall.CallType,
                        RespondedCallDuration = call.ValidCall.CallDuration,
                        RespondedCustomerMobileNumber = call.ValidCall.CustomerMobileNumber,
                        RespondedEventTime = call.ValidCall.EventTime,
                        RespondedLabName = call.ValidCall.LabName,
                        RespondedLabPhoneNumber = call.ValidCall.LabPhoneNumber,
                        RespondedTime = call.RespondedTime,
                        Action = call.ValidCall.Action,
                        CallPurpose = call.ValidCall.CallPurpose,
                        Comment = call.ValidCall.Comment,
                        IsWhiteListedCall = call.IsWhiteListed,
                        CustomerName = call.CustomerName
                    };

                    result.Add(item);
                }
            }

            return result;
        }

        // GET: api/MissedCalls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MissedCall>> GetMissedCall(int id)
        {
            var missedCall = await context.MissedCalls.FindAsync(id);

            if (missedCall == null)
            {
                return NotFound();
            }

            return missedCall;
        }

        // PUT: api/MissedCalls/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMissedCall(int id, MissedCall missedCall)
        {


            if (id != missedCall.Id)
            {
                return BadRequest();
            }

            context.Entry(missedCall).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MissedCallExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MissedCalls
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MissedCall>> PostMissedCall(MissedCall missedCall)
        {
            try
            {
                // to check if the missed call is from the whitelisted numbers
                var whiteList = context.WhiteList.Where(x => x.MobileNumber.Equals(missedCall.CustomerMobileNumber)).FirstOrDefault();
                
                if (whiteList?.MobileNumber!=null)
                {
                    missedCall.IsWhiteListed = true;

                    missedCall.CustomerName = whiteList.Name;
                }

                context.MissedCalls.Add(missedCall);

                await context.SaveChangesAsync();

                logger.LogInformation("Post Missed Call successfull");

                return CreatedAtAction("GetMissedCall", new { id = missedCall.Id }, missedCall);
            }
            catch (Exception ex)
            {

                logger.LogError("Exception in PostMissedCall" + ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError, "Error in Posting Missed Call Obnect");
            }
        }

        // DELETE: api/MissedCalls/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MissedCall>> DeleteMissedCall(int id)
        {
            var missedCall = await context.MissedCalls.FindAsync(id);

            if (missedCall == null)
            {
                return NotFound();
            }

            context.MissedCalls.Remove(missedCall);

            await context.SaveChangesAsync();

            return missedCall;
        }

        private bool MissedCallExists(int id)
        {
            return context.MissedCalls.Any(e => e.Id == id);
        }
    }
}
