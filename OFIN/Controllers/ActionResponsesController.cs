using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OFIN.Models;

namespace OFIN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionResponsesController : ControllerBase
    {
        private readonly ActionResponse _context;

        public ActionResponsesController(ActionResponse context)
        {
            _context = context;
        }

        // GET: api/ActionResponses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActionResponses>>> GettActionResponses()
        {
            return await _context.tActionResponses.ToListAsync();
        }

        // GET: api/ActionResponses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActionResponses>> GetActionResponses(string id)
        {
            var actionResponses = await _context.tActionResponses.FindAsync(id);

            if (actionResponses == null)
            {
                return NotFound();
            }

            return actionResponses;
        }

        // PUT: api/ActionResponses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionResponses(string id, ActionResponses actionResponses)
        {
            if (id != actionResponses.fErrorCode)
            {
                return BadRequest();
            }

            _context.Entry(actionResponses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActionResponsesExists(id))
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

        // POST: api/ActionResponses
        [HttpPost]
        public async Task<ActionResult<ActionResponses>> PostActionResponses(ActionResponses actionResponses)
        {
            _context.tActionResponses.Add(actionResponses);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActionResponses", new { id = actionResponses.fErrorCode }, actionResponses);
        }

        // DELETE: api/ActionResponses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ActionResponses>> DeleteActionResponses(string id)
        {
            var actionResponses = await _context.tActionResponses.FindAsync(id);
            if (actionResponses == null)
            {
                return NotFound();
            }

            _context.tActionResponses.Remove(actionResponses);
            await _context.SaveChangesAsync();

            return actionResponses;
        }

        private bool ActionResponsesExists(string id)
        {
            return _context.tActionResponses.Any(e => e.fErrorCode == id);
        }
    }
}
