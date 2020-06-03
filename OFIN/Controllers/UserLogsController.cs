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
    public class UserLogsController : ControllerBase
    {
        private readonly UserLogContext _context;

        public UserLogsController(UserLogContext context)
        {
            _context = context;
        }

        // GET: api/UserLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLog>>> GettUserLogs()
        {
            return await _context.tUserLogs.ToListAsync();
        }

        // GET: api/UserLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserLog>> GetUserLog(string id)
        {
            var userLog = await _context.tUserLogs.FindAsync(id);

            if (userLog == null)
            {
                return NotFound();
            }

            return userLog;
        }

        // PUT: api/UserLogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserLog(string id, UserLog userLog)
        {
            if (id != userLog.fLogId)
            {
                return BadRequest();
            }

            _context.Entry(userLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserLogExists(id))
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

        // POST: api/UserLogs
        [HttpPost]
        public async Task<ActionResult<UserLog>> PostUserLog(UserLog userLog)
        {
            _context.tUserLogs.Add(userLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserLog", new { id = userLog.fLogId }, userLog);
        }

        // DELETE: api/UserLogs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserLog>> DeleteUserLog(string id)
        {
            var userLog = await _context.tUserLogs.FindAsync(id);
            if (userLog == null)
            {
                return NotFound();
            }

            _context.tUserLogs.Remove(userLog);
            await _context.SaveChangesAsync();

            return userLog;
        }

        private bool UserLogExists(string id)
        {
            return _context.tUserLogs.Any(e => e.fLogId == id);
        }
    }
}
