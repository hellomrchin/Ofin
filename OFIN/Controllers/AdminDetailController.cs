using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OFIN.Models;
using OFIN.Models.Account;

namespace OFIN.Controllers
{
    [Route("v1/Account/[controller]")]
    [ApiController]
    public class AdminDetailController : ControllerBase
    {
        private readonly AdminDetailContext _context;
        private readonly ActionResponse _actionContext;
        private readonly AdminContext _userContext;
        private readonly LogContext _logContext;

        public AdminDetailController(AdminDetailContext context, ActionResponse actionContext, AdminContext userContext, LogContext logContext)
        {
            _context = context;
            _actionContext = actionContext;
            _userContext = userContext;
            _logContext = logContext;
        }

        // GET: api/AdminDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminDetail>>> GetAdminDetail()
        {
            return await _context.tAdminDetail.ToListAsync();
        }

        // GET: api/AdminDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminDetail>> GetAdminDetail(string id)
        {
            var adminDetail = await _context.tAdminDetail.FindAsync(id);

            if (adminDetail == null)
            {
                return NotFound();
            }

            return adminDetail;
        }

        // PUT: api/AdminDetail/5
        [HttpPut("{id}")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> PutAdminDetail(string id, [FromForm]AdminDetail adminDetail)
        {
            ActionResponses actionResponses;
            if (id != adminDetail.fUsername)
            {
                return BadRequest();
            }

            using (_context)
            {
                var entity = _context.tAdminDetail.FirstOrDefault(item => item.fUsername == id);
                if (entity != null)
                {
                    entity.fUsername = adminDetail.fUsername;
                    entity.fFirstName = adminDetail.fFirstName;
                    entity.fLastName = adminDetail.fLastName;
                    entity.fGender = adminDetail.fGender;
                    _context.SaveChanges();

                    InsertLog("Modify Admin Extended Information", "Execution Success", "0");
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                    return CreatedAtAction("GetAdminDetail", actionResponses);
                }
            }

            return NoContent();
        }

        // POST: api/AdminDetail
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<AdminDetail>> PostAdminDetail([FromForm]AdminDetail adminDetail)
        {
            ActionResponses actionResponses;
            if (UserExists(adminDetail.fUsername))
            {
                if (AdminDetailExists(adminDetail.fUsername))
                {
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "1");
                    InsertLog("Create User Detail", "Execution Failed", "1");
                    return CreatedAtAction("GetAdminDetail", actionResponses);
                }

                _context.tAdminDetail.Add(adminDetail);
                await _context.SaveChangesAsync();
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                InsertLog("Create User Detail", "Execution Success", "0");
                return CreatedAtAction("GetAdminDetail", actionResponses);
            }
            else
            {
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "3");
                InsertLog("Create User Detail", "Execution Failed", "3");
                return CreatedAtAction("GetAdminDetail", actionResponses);
            }
        }

        // DELETE: api/AdminDetail/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AdminDetail>> DeleteAdminDetail(string id)
        {
            var adminDetail = await _context.tAdminDetail.FindAsync(id);
            if (adminDetail == null)
            {
                return NotFound();
            }

            _context.tAdminDetail.Remove(adminDetail);
            await _context.SaveChangesAsync();

            return adminDetail;
        }

        private bool AdminDetailExists(string id)
        {
            return _context.tAdminDetail.Any(e => e.fUsername == id);
        }

        private bool UserExists(string id)
        {
            return _userContext.tAdmin.Any(e => e.fUsername == id);
        }

        private void InsertLog(string LogGroup, string LogMsg, string ErrorCode)
        {
            Logs log = new Logs();
            log.LogGroup = LogGroup;
            log.LogMsg = LogMsg;
            log.ErrorCode = ErrorCode;
            log.LogDate = DateTime.Now;
            log.IpAddress = Utilities.GetIp();
            _logContext.tLogs.Add(log);
            _logContext.SaveChanges();
        }
    }
}
