using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMTransactions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace CRMTransactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidCallsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger logger;
        private readonly IConfiguration configuration;

        public ValidCallsController(AppDbContext context, ILogger<ValidCallsController> logger , IConfiguration configuration)
        {
            this.context = context;
            this.logger = logger;
            this.configuration = configuration;
        }

        // GET: api/ValidCalls
        [HttpGet]
        [Route("GetValidCalls")]
        public async Task<ActionResult<IEnumerable<ValidCall>>> GetValidCalls()
        {
           return await context.ValidCalls.OrderByDescending(x=>x.ValidCallId).ToListAsync();
        }

        // GET: api/ValidCalls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ValidCall>> GetValidCall(int id)
        {
            var validCall = await context.ValidCalls.FindAsync(id);

            if (validCall == null)
            {
                return NotFound();
            }

            return validCall;
        }

        // PUT: api/ValidCalls/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut()]
        public async Task<IActionResult> PutValidCall( ValidCall validCall)
        {
            int id = validCall.ValidCallId;

            if (id != validCall.ValidCallId)
            {
                return BadRequest();
            }

            context.Entry(validCall).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValidCallExists(id))
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

        // POST: api/ValidCalls
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ValidCall>> PostValidCall(ValidCall validCall)
        {

            context.ValidCalls.Add(validCall);

            await context.SaveChangesAsync();

            logger.LogInformation("Post ValidCall successfull");

            //TODO: update only if the call type is missed cal ? Date filter & call duration

            if (validCall.CallDuration > Convert.ToInt32(configuration.GetValue<string>("CallDurationInSeconds")))
            {
                int hours = Convert.ToInt32(configuration.GetValue<string>("HourFilterRange"));

                var missedcalls = context.MissedCalls.Where(x =>

                    (x.CustomerMobileNumber.Equals(validCall.CustomerMobileNumber)
                    && !x.ValidCallId.HasValue && x.EventTime > DateTime.Now.AddHours(-hours)
                    )
                    ).ToList();

                foreach (var v in missedcalls)
                {
                    TimeSpan ts = validCall.EventTime - v.EventTime;
                    v.ValidCallId = validCall.ValidCallId;
                    v.RespondedTime = Math.Round(ts.TotalHours, 2).ToString() + "Hrs";
                    context.MissedCalls.Update(v);
                }

                context.SaveChanges();
            }
            return Created("", validCall);
        }

        // DELETE: api/ValidCalls/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ValidCall>> DeleteValidCall(int id)
        {
            var validCall = await context.ValidCalls.FindAsync(id);
            if (validCall == null)
            {
                return NotFound();
            }

            context.MissedCalls.RemoveRange(context.MissedCalls.Where(x => x.ValidCallId.Equals(validCall.ValidCallId)).ToList());

            context.ValidCalls.Remove(validCall);

            await context.SaveChangesAsync();

            return validCall;
        }

        private bool ValidCallExists(int id)
        {
            return context.ValidCalls.Any(e => e.ValidCallId == id);
        }
    }
}
