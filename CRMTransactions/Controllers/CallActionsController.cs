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
    public class CallActionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CallActionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CallActions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CallAction>>> GetCallAction()
        {
            return await _context.CallAction.ToListAsync();
        }

        // GET: api/CallActions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CallAction>> GetCallAction(int id)
        {
            var callAction = await _context.CallAction.FindAsync(id);

            if (callAction == null)
            {
                return NotFound();
            }

            return callAction;
        }

        // PUT: api/CallActions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCallAction(int id, CallAction callAction)
        {
            if (id != callAction.Id)
            {
                return BadRequest();
            }

            _context.Entry(callAction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CallActionExists(id))
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

        // POST: api/CallActions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CallAction>> PostCallAction(CallAction callAction)
        {
            _context.CallAction.Add(callAction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCallAction", new { id = callAction.Id }, callAction);
        }

        // DELETE: api/CallActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CallAction>> DeleteCallAction(int id)
        {
            var callAction = await _context.CallAction.FindAsync(id);
            if (callAction == null)
            {
                return NotFound();
            }

            _context.CallAction.Remove(callAction);
            await _context.SaveChangesAsync();

            //remove the deleted actions from the table entries

            var validCallswithSelectedAction = _context.ValidCalls.Where(x => x.Action.Equals(callAction.Actions, StringComparison.InvariantCultureIgnoreCase));
            if(validCallswithSelectedAction.Count()>0)
            {
                foreach( ValidCall vc in  validCallswithSelectedAction)
                {
                    vc.Action = "";
                }
                await _context.SaveChangesAsync();
            }

            return callAction;
        }

        private bool CallActionExists(int id)
        {
            return _context.CallAction.Any(e => e.Id == id);
        }
    }
}
