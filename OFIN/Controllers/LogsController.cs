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
    [Route("v1/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LogContext _context;

        public LogsController(LogContext context)
        {
            _context = context;
        }

        // GET: api/Logs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Logs>>> GettLogs()
        {
            return await _context.tLogs.ToListAsync();
        }

        // GET: api/Logs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Logs>> GetLogs(string id)
        {
            var logs = await _context.tLogs.FindAsync(id);

            if (logs == null)
            {
                return NotFound();
            }

            return logs;
        }

        // PUT: api/Logs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogs(string id, Logs logs)
        {
            if (id != logs.LogId)
            {
                return BadRequest();
            }

            _context.Entry(logs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogsExists(id))
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

        // POST: api/Logs
        [HttpPost]
        public async Task<ActionResult<Logs>> PostLogs(Logs logs)
        {
            _context.tLogs.Add(logs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogs", new { id = logs.LogId }, logs);
        }

        // DELETE: api/Logs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Logs>> DeleteLogs(string id)
        {
            var logs = await _context.tLogs.FindAsync(id);
            if (logs == null)
            {
                return NotFound();
            }

            _context.tLogs.Remove(logs);
            await _context.SaveChangesAsync();

            return logs;
        }

        private bool LogsExists(string id)
        {
            return _context.tLogs.Any(e => e.LogId == id);
        }
    }
}
