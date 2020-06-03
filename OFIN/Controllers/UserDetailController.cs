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
    public class UserDetailController : ControllerBase
    {
        private readonly UserDetailContext _context;
        private readonly ActionResponse _actionContext;
        private readonly UserContext _userContext;
        private readonly LogContext _logContext;
        public UserDetailController(UserDetailContext context, ActionResponse actionContext, UserContext userContext, LogContext logContext)
        {
            _context = context;
            _actionContext = actionContext;
            _userContext = userContext;
            _logContext = logContext;
        }

        // GET: api/UserDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetail>>> GetUserDetail()
        {
            return await _context.tUserDetail.ToListAsync();
        }

        // GET: api/UserDetail/5
        [HttpGet("{id}")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<UserDetail>> GetUserDetail(string id)
        {
            var userDetail = await _context.tUserDetail.FindAsync(id);

            if (userDetail == null)
            {
                return NotFound();
            }

            return userDetail;
        }

        // PUT: api/UserDetail/5
        [HttpPut("{id}")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> PutUserDetail(string id, [FromForm]UserDetail userDetail)
        {
            ActionResponses actionResponses;
            if (id != userDetail.fUsername)
            {
                return BadRequest();
            }

            using (_context)
            {
                var entity = _context.tUserDetail.FirstOrDefault(item => item.fUsername == id);
                if (entity != null)
                {
                    entity.fUsername = userDetail.fUsername;
                    entity.fFirstName = userDetail.fFirstName;
                    entity.fLastName = userDetail.fLastName;
                    entity.fGender = userDetail.fGender;
                    _context.SaveChanges();

                    InsertLog("User Detail Changes", "Execution Success", "0");
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                    return CreatedAtAction("GetUserDetail", actionResponses);
                }
            }

            return NoContent();
        }

        // POST: api/UserDetail
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<UserDetail>> PostUserDetail([FromForm]UserDetail userDetail)
        {
            ActionResponses actionResponses;
            if (UserExists(userDetail.fUsername))
            {
                if (UserDetailExists(userDetail.fUsername))
                {
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "1");
                    InsertLog("Create User Detail", "Execution Failed", "1");
                    return CreatedAtAction("GetUserDetail", actionResponses);
                }

                _context.tUserDetail.Add(userDetail);
                await _context.SaveChangesAsync();
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                InsertLog("Create User Detail", "Execution Success", "0");
                return CreatedAtAction("GetUserDetail", actionResponses);
            }
            else
            {
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "3");
                InsertLog("Create User Detail", "Execution Failed", "3");
                return CreatedAtAction("GetUserDetail", actionResponses);
            }


        }

        // DELETE: api/UserDetail/5
        [HttpDelete("{id}")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<UserDetail>> DeleteUserDetail(string id)
        {
            var userDetail = await _context.tUserDetail.FindAsync(id);
            if (userDetail == null)
            {
                return NotFound();
            }

            _context.tUserDetail.Remove(userDetail);
            await _context.SaveChangesAsync();

            return userDetail;
        }

        private bool UserDetailExists(string id)
        {
            return _context.tUserDetail.Any(e => e.fUsername == id);
        }

        private bool UserExists(string id)
        {
            return _userContext.tUser.Any(e => e.fUsername == id);
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
