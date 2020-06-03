using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OFIN.Models;
using OFIN.Models.eWallet.Topup;

namespace OFIN.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class TopupController : ControllerBase
    {
        private readonly BalanceContext _context;
        private readonly LogContext _logContext;
        private readonly UserLogContext _userLogContext;
        private readonly ActionResponse _actionResponse;

        public TopupController(BalanceContext context, LogContext logContext, UserLogContext userLogContext, ActionResponse actionResponse)
        {
            _context = context;
            _logContext = logContext;
            _userLogContext = userLogContext;
            _actionResponse = actionResponse;
        }

        // GET: api/Topup
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Balances>>> GetBalance()
        {
            return await _context.tBalance.ToListAsync();
        }

        // GET: api/Topup/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Balances>> GetBalances(string id)
        {
            var balances = await _context.tBalance.FindAsync(id);

            if (balances == null)
            {
                return NotFound();
            }

            return balances;
        }

        // PUT: api/Topup/5
        [HttpPut("{id}")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> PutBalances(string id, [FromForm]Balances balances)
        {
            ActionResponses actionResponses;
            if (id != balances.fUsername)
            {
                return BadRequest();
            }

            using (_context)
            {
                var entity = _context.tBalance.FirstOrDefault(item => item.fUsername == id);
                if (entity != null)
                {
                    entity.fUsername = balances.fUsername;
                    entity.fBalance = balances.fBalance + entity.fBalance;
                    _context.SaveChanges();

                    InsertLog("Topup", "Execution Success", "0");
                    InsertUserLog(id, "Topup", "Top up success! Topup value: " + balances.fBalance);
                    actionResponses = _actionResponse.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                    return CreatedAtAction("GetBalance", actionResponses);
                }
            }

            return NoContent();
        }

        // POST: api/Topup
        [HttpPost]
        public async Task<ActionResult<Balances>> PostBalances([FromForm]Balances balances)
        {
            ActionResponses actionResponses;
            if (User.IsInRole("Admin"))
            {
                _context.tBalance.Add(balances);
                await _context.SaveChangesAsync();

                InsertLog("Topup", "Execution Success", "0");
                InsertUserLog(balances.fUsername, "Topup", "eWallet Created");
                actionResponses = _actionResponse.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                return CreatedAtAction("GetBalance", actionResponses);
            }
            else
            {
                InsertLog("Create eWallet", "Execution Failed", "4");
                actionResponses = _actionResponse.tActionResponses.FirstOrDefault(x => x.fErrorCode == "4");
                return CreatedAtAction("GetBalance", actionResponses);
            }


        }

        // DELETE: api/Topup/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Balances>> DeleteBalances(string id)
        {
            var balances = await _context.tBalance.FindAsync(id);
            if (balances == null)
            {
                return NotFound();
            }

            _context.tBalance.Remove(balances);
            await _context.SaveChangesAsync();

            return balances;
        }

        private bool BalancesExists(string id)
        {
            return _context.tBalance.Any(e => e.fUsername == id);
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

        private void InsertUserLog(string Username, string Action, string ActionDesc)
        {
            UserLog log = new UserLog();
            log.fUsername = Username;
            log.fAction = Action;
            log.fActionDesc = ActionDesc;
            log.fDate = DateTime.Now;
            _userLogContext.tUserLogs.Add(log);
            _userLogContext.SaveChanges();
        }
    }
}
