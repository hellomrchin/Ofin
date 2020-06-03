using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OFIN.Models;
using OFIN.Models.Merchant;

namespace OFIN.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierContext _context;
        private readonly ActionResponse _actionContext;
        private readonly LogContext _logContext;

        public SupplierController(SupplierContext context, ActionResponse actionContext, LogContext logContext)
        {
            _context = context;
            _actionContext = actionContext;
            _logContext = logContext;
        }

        // GET: api/Supplier
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Suppliers>>> GetSupplier()
        {
            return await _context.tSupplier.ToListAsync();
        }

        // GET: api/Supplier/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Suppliers>> GetSuppliers(string id)
        {
            var suppliers = await _context.tSupplier.FindAsync(id);

            if (suppliers == null)
            {
                return NotFound();
            }

            return suppliers;
        }

        // PUT: api/Supplier/5
        [HttpPut("{id}")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> PutSuppliers(string id, [FromForm]Suppliers suppliers)
        {
            ActionResponses actionResponses;
            if (id != suppliers.fSupplierCode)
            {
                return BadRequest();
            }

            using (_context)
            {
                var entity = _context.tSupplier.FirstOrDefault(i => i.fSupplierCode == id);
                if (entity != null)
                {
                    entity.fSupplierCode = suppliers.fSupplierCode;
                    entity.fSupplierName = suppliers.fSupplierName;
                    entity.fSupplierEmail = suppliers.fSupplierEmail;
                    entity.fSupplierPhone = suppliers.fSupplierPhone;
                    entity.fRemark = suppliers.fRemark;
                    entity.fModifiedAt = Utilities.GetIp();
                    entity.fModifiedBy = "Admin";
                    entity.fModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                    InsertLog("Modify Supplier Information", "Execution Success", "0");
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                    return CreatedAtAction("GetSupplier", actionResponses);
                }
            }

            return NoContent();
        }

        // POST: api/Supplier
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Suppliers>> PostSuppliers([FromForm]Suppliers suppliers)
        {
            ActionResponses actionResponses;
            if (User.IsInRole("Admin"))
            {
                if (SuppliersExists(suppliers.fSupplierCode))
                {
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "1");
                    InsertLog("Create Item", "Execution Failed", "1");
                    return CreatedAtAction("GetSupplier", actionResponses);
                }
                else
                {
                    suppliers.fCreatedAt = Utilities.GetIp();
                    suppliers.fCreatedBy = "Admin";
                    suppliers.fCreatedDate = DateTime.Now;

                    suppliers.fModifiedBy = "Admin";
                    suppliers.fModifiedAt = Utilities.GetIp();
                    suppliers.fModifiedDate = DateTime.Now;
                    _context.tSupplier.Add(suppliers);
                    await _context.SaveChangesAsync();
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                    InsertLog("Create Item", "Execution Success", "0");
                    return CreatedAtAction("GetSupplier", actionResponses);
                }
            }
            else
            {
                InsertLog("Create Item", "Execution Failed", "4");
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "4");
                return CreatedAtAction("GetSupplier", actionResponses);
            }
        }

        // DELETE: api/Supplier/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Suppliers>> DeleteSuppliers(string id)
        {
            var suppliers = await _context.tSupplier.FindAsync(id);
            if (suppliers == null)
            {
                return NotFound();
            }

            _context.tSupplier.Remove(suppliers);
            await _context.SaveChangesAsync();

            return suppliers;
        }

        private bool SuppliersExists(string id)
        {
            return _context.tSupplier.Any(e => e.fSupplierCode == id);
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
