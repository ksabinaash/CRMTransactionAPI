using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMTransactions.Models;

namespace CRMTransactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallPurposesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CallPurposesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CallPurposes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CallPurpose>>> GetCallPurpose()
        {
            return await _context.CallPurpose.ToListAsync();
        }

        // GET: api/CallPurposes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CallPurpose>> GetCallPurpose(int id)
        {
            var callPurpose = await _context.CallPurpose.FindAsync(id);

            if (callPurpose == null)
            {
                return NotFound();
            }

            return callPurpose;
        }

        // PUT: api/CallPurposes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCallPurpose(int id, CallPurpose callPurpose)
        {
            if (id != callPurpose.Id)
            {
                return BadRequest();
            }

            _context.Entry(callPurpose).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CallPurposeExists(id))
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

        // POST: api/CallPurposes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CallPurpose>> PostCallPurpose(CallPurpose callPurpose)
        {
            _context.CallPurpose.Add(callPurpose);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCallPurpose", new { id = callPurpose.Id }, callPurpose);
        }

        // DELETE: api/CallPurposes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CallPurpose>> DeleteCallPurpose(int id)
        {
            var callPurpose = await _context.CallPurpose.FindAsync(id);
            if (callPurpose == null)
            {
                return NotFound();
            }

            _context.CallPurpose.Remove(callPurpose);

            await _context.SaveChangesAsync();

            //remove the deleted purpose from the table entries

            var validCallswithSelectedAction = _context.ValidCalls.Where(x => x.CallPurpose.Equals(callPurpose.PurposeoftheCall, StringComparison.InvariantCultureIgnoreCase));
            if (validCallswithSelectedAction.Count() > 0)
            {
                foreach (ValidCall vc in validCallswithSelectedAction)
                {
                    vc.CallPurpose = "";
                }
                await _context.SaveChangesAsync();
            }


            return callPurpose;
        }

        private bool CallPurposeExists(int id)
        {
            return _context.CallPurpose.Any(e => e.Id == id);
        }
    }
}
