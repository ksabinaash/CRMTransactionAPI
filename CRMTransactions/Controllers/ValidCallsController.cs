using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMTransactions.Models;
using Microsoft.Extensions.Logging;

namespace CRMTransactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidCallsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public ValidCallsController(AppDbContext context, ILogger<ValidCallsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ValidCalls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ValidCall>>> GetValidCalls()
        {
            _logger.LogInformation("Get Valid Call successfull");
            _logger.LogInformation("Get Valid Call successfull");
            _logger.LogInformation("Get Valid Call successfull");
            _logger.LogInformation("Get Valid Call successfull");
            _logger.LogInformation("Get Valid Call successfull");
            _logger.LogInformation("Get Valid Call successfull");
            _logger.LogInformation("Get Valid Call successfull");
            _logger.LogInformation("Get Valid Call successfull");
            return await _context.ValidCalls.ToListAsync();
        }

        // GET: api/ValidCalls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ValidCall>> GetValidCall(int id)
        {
            var validCall = await _context.ValidCalls.FindAsync(id);

            if (validCall == null)
            {
                return NotFound();
            }

            return validCall;
        }

        // PUT: api/ValidCalls/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutValidCall(int id, ValidCall validCall)
        {
            if (id != validCall.ValidCallId)
            {
                return BadRequest();
            }

            _context.Entry(validCall).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

            _context.ValidCalls.Add(validCall);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Post ValidCall successfull");
            // update only if the call type is missed cal ?

            //if (validCall.CallType.Equals("outgoing", StringComparison.InvariantCultureIgnoreCase))
            //{
            var missedcalls = _context.MissedCalls.Where(x =>

                (x.CustomerMobileNumber.Equals(validCall.CustomerMobileNumber)
                && !x.ValidCallId.HasValue)
                ).ToList();


            foreach (var v in missedcalls)
                {
                    v.ValidCallId = validCall.ValidCallId;
                     _context.MissedCalls.Update(v);
                }

            _context.SaveChanges();
            //}


            return Created("", validCall);
        }

        // DELETE: api/ValidCalls/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ValidCall>> DeleteValidCall(int id)
        {
            var validCall = await _context.ValidCalls.FindAsync(id);
            if (validCall == null)
            {
                return NotFound();
            }

            _context.ValidCalls.Remove(validCall);
            await _context.SaveChangesAsync();

            return validCall;
        }

        private bool ValidCallExists(int id)
        {
            return _context.ValidCalls.Any(e => e.ValidCallId == id);
        }
    }
}
