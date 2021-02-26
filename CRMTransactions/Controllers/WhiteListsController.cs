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
    public class WhiteListsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WhiteListsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/WhiteLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WhiteList>>> GetWhiteList()
        {
            return await _context.WhiteList.ToListAsync();
        }

        // GET: api/WhiteLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WhiteList>> GetWhiteList(int id)
        {
            var whiteList = await _context.WhiteList.FindAsync(id);

            if (whiteList == null)
            {
                return NotFound();
            }

            return whiteList;
        }

        // PUT: api/WhiteLists/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWhiteList(int id, WhiteList whiteList)
        {
            if (id != whiteList.Id)
            {
                return BadRequest();
            }

            _context.Entry(whiteList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WhiteListExists(id))
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

        // POST: api/WhiteLists
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<WhiteList>> PostWhiteList(WhiteList whiteList)
        {
            _context.WhiteList.Add(whiteList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWhiteList", new { id = whiteList.Id }, whiteList);
        }

        // DELETE: api/WhiteLists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WhiteList>> DeleteWhiteList(int id)
        {
            var whiteList = await _context.WhiteList.FindAsync(id);
            if (whiteList == null)
            {
                return NotFound();
            }

            _context.WhiteList.Remove(whiteList);
            await _context.SaveChangesAsync();

            return whiteList;
        }

        private bool WhiteListExists(int id)
        {
            return _context.WhiteList.Any(e => e.Id == id);
        }
    }
}
