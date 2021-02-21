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

namespace CRMTransactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissedCallsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger logger;

        public MissedCallsController(AppDbContext context, ILogger<ValidCallsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        // GET: api/MissedCalls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MissedCall>>> GetMissedCalls()
        {

           return await context.MissedCalls.Include("ValidCall").ToListAsync();
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
