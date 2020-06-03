using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OFIN.Models;
using OFIN.Models.Account;
using OFIN.Models.eWallet.Topup;

namespace OFIN.Controllers
{
    [Route("v1/Account/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly ActionResponse _actionContext;
        private readonly LogContext _logContext;
        private readonly BalanceContext _balanceContext;

        public UserController(UserContext context, ActionResponse actionContext, LogContext logContext, BalanceContext balanceContext)
        {
            _context = context;
            _actionContext = actionContext;
            _logContext = logContext;
            _balanceContext = balanceContext;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.tUser.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.tUser.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> PutUser(string id, [FromForm]User user)
        {
            ActionResponses actionResponses;
            if (id != user.fUsername)
            {
                return BadRequest();
            }

            using (_context)
            {
                var entity = _context.tUser.FirstOrDefault(item => item.fUsername == id);
                if (entity != null)
                {
                    entity.fUsername = user.fUsername;
                    entity.fPassword = Utilities.Encrypt(user.fPassword);
                    _context.SaveChanges();

                    InsertLog("User Change Password", "Execution Success", "0");
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                    return CreatedAtAction("GetUser", actionResponses);
                }
            }

            return NoContent();
        }

        // POST: api/User
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<User>> PostUser([FromForm]User user)
        {
            ActionResponses actionResponses;
            if (UserExists(user.fUsername))
            {
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "1");
                InsertLog("Create User", "Execution Failed", "1");
                return CreatedAtAction("GetUser", actionResponses);
            }

            user.fPassword = Utilities.Encrypt(user.fPassword);
            user.fAccessToken = GenerateToken(user.fUsername);
            user.fIsVerified = "False";
            user.fRegTime = DateTime.Now;
            _context.tUser.Add(user);
            await _context.SaveChangesAsync();
            CreateWallet(user.fUsername);
            InsertLog("Create User", "Execution Success", "0");
            actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
            return CreatedAtAction("GetUser", actionResponses);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(string id)
        {
            ActionResponses actionResponses;
            var user = await _context.tUser.FindAsync(id);
            if (user == null)
            {
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "2");
                InsertLog("Delete User", "Execution Failed", "2");
                return CreatedAtAction("GetUser", actionResponses);
            }

            _context.tUser.Remove(user);
            await _context.SaveChangesAsync();
            actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "2");
            InsertLog("Delete User", "Execution Success", "0");
            return CreatedAtAction("GetUser", actionResponses);
        }

        private bool UserExists(string id)
        {
            return _context.tUser.Any(e => e.fUsername == id);
        }

        private string GenerateToken(string Username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Authenticator.SecretKey);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, Username));
            claims.Add(new Claim(ClaimTypes.Role, "User"));

            var claimsID = new ClaimsIdentity(claims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsID,
                Expires = DateTime.UtcNow.AddHours(48),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private void CreateWallet(string Username)
        {
            Balances balance = new Balances();
            balance.fUsername = Username;
            balance.fBalance = 0;
            balance.fLastTopUp = DateTime.Now;
            _balanceContext.tBalance.Add(balance);
            _balanceContext.SaveChanges();
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
