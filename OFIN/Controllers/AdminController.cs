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
    public class AdminController : ControllerBase
    {
        private readonly AdminContext _context;
        private readonly ActionResponse _actionContext;
        private readonly LogContext _logContext;
        private readonly BalanceContext _balanceContext;

        public AdminController(AdminContext context, ActionResponse actionContext, LogContext logContext, BalanceContext balanceContext)
        {
            _context = context;
            _actionContext = actionContext;
            _logContext = logContext;
            _balanceContext = balanceContext;
        }

        // GET: api/Admin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmin()
        {
            return await _context.tAdmin.ToListAsync();
        }

        // GET: api/Admin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(string id)
        {
            var admin = await _context.tAdmin.FindAsync(id);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        // PUT: api/Admin/5
        [HttpPut("{id}")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> PutAdmin(string id, [FromForm]Admin admin)
        {
            ActionResponses actionResponses;
            if (id != admin.fUsername)
            {
                return BadRequest();
            }

            using (_context)
            {
                var entity = _context.tAdmin.FirstOrDefault(item => item.fUsername == id);
                if (entity != null)
                {
                    entity.fUsername = admin.fUsername;
                    entity.fPassword = Utilities.Encrypt(admin.fPassword);
                    _context.SaveChanges();
                    InsertLog("Modify Admin Information", "Execution Success", "0");
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                    return CreatedAtAction("GetAdmin", actionResponses);
                }
            }

            return NoContent();
        }

        // POST: api/Admin
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Admin>> PostAdmin([FromForm]Admin admin)
        {
            ActionResponses actionResponses;
            if (AdminExists(admin.fUsername))
            {
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "1");
                InsertLog("Create Admin", "Execution Failed", "1");
                return CreatedAtAction("GetAdmin", actionResponses);
            }

            admin.fPassword = Utilities.Encrypt(admin.fPassword);
            admin.fAccessToken = GenerateToken(admin.fUsername);
            admin.fIsVerified = "False";
            admin.fRegTime = DateTime.Now;
            _context.tAdmin.Add(admin);
            await _context.SaveChangesAsync();
            InsertLog("Create Admin", "Execution Success", "0");
            actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
            return CreatedAtAction("GetAdmin", actionResponses);
        }

        // DELETE: api/Admin/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Admin>> DeleteAdmin(string id)
        {
            var admin = await _context.tAdmin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.tAdmin.Remove(admin);
            await _context.SaveChangesAsync();

            return admin;
        }

        private bool AdminExists(string id)
        {
            return _context.tAdmin.Any(e => e.fUsername == id);
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

        private string GenerateToken(string Username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Authenticator.SecretKey);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, Username));
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));

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
    }
}
