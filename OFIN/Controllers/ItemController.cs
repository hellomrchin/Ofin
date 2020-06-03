using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OFIN.Models;
using OFIN.Models.Account;
using OFIN.Models.Merchant;

namespace OFIN.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ItemContext _context;
        private readonly StockContext _stockContext;
        private readonly ActionResponse _actionContext;
        private readonly UserContext _userContext;
        private readonly LogContext _logContext;
        private readonly SupplierContext _supplierContext;
        public ItemController(ItemContext context, StockContext stockContext, ActionResponse actionContext, LogContext logContext, SupplierContext supplierContext)
        {
            _context = context;
            _stockContext = stockContext;
            _actionContext = actionContext;
            _logContext = logContext;
            _supplierContext = supplierContext;
        }

        // GET: api/Item
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItem()
        {
            ActionResponses actionResponses;
            if (User.IsInRole("Admin"))
            {
                return await _context.tItem.ToListAsync();
            }
            else
            {
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "4");
                return CreatedAtAction("GetItem", actionResponses);
            }

        }

        // GET: api/Item/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(string id)
        {
            ActionResponses actionResponses;
            if (User.IsInRole("Admin"))
            {
                var item = await _context.tItem.FindAsync(id);

                if (item == null)
                {
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "3");
                    return CreatedAtAction("GetItem", actionResponses);
                }

                return item;
            }
            else
            {
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "4");
                return CreatedAtAction("GetItem", actionResponses);
            }
        }

        // PUT: api/Item/5
        [HttpPut("{id}")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> PutItem(string id, [FromForm]Item item)
        {
            ActionResponses actionResponses;
            if (id != item.fItemCode)
            {
                return BadRequest();
            }

            using (_context)
            {
                var entity = _context.tItem.FirstOrDefault(i => i.fItemCode == id);
                if (entity != null)
                {
                    entity.fItemCode = item.fItemCode;
                    entity.fItemPrice = item.fItemPrice;
                    entity.fItemDesc = item.fItemDesc;
                    entity.fItemTitle = item.fItemTitle;
                    entity.fModifiedAt = Utilities.GetIp();
                    entity.fModifiedBy = "Admin";
                    entity.fModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                    InsertLog("Modify Item Information", "Execution Success", "0");
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                    return CreatedAtAction("GetItem", actionResponses);
                }
            }

            return NoContent();
        }

        // POST: api/Item
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Item>> PostItem([FromForm]Item item)
        {
            ActionResponses actionResponses;
            if (User.IsInRole("Admin"))
            {
                if (ItemExists(item.fItemCode))
                {
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "1");
                    InsertLog("Create Item", "Execution Failed", "1");
                    return CreatedAtAction("GetItem", actionResponses);
                }
                else
                {
                    var supplier = _supplierContext.tSupplier.FirstOrDefault(x => x.fSupplierCode == item.fSupplier);
                    if (supplier == null)
                    {
                        actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "9");
                        InsertLog("Create Item", "Execution Failed", "9");
                        return CreatedAtAction("GetItem", actionResponses);
                    }

                    item.fItemBought = 0;
                    item.fCreatedAt = Utilities.GetIp();
                    item.fCreatedBy = "Admin";
                    item.fCreatedDate = DateTime.Now;

                    item.fModifiedAt = Utilities.GetIp();
                    item.fModifiedBy = "Admin";
                    item.fModifiedDate = DateTime.Now;
                    _context.tItem.Add(item);
                    await _context.SaveChangesAsync();
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                    InsertLog("Create Item", "Execution Success", "0");
                    return CreatedAtAction("GetItem", actionResponses);
                }
            }
            else
            {
                InsertLog("Create Item", "Execution Failed", "4");
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "4");
                return CreatedAtAction("GetItem", actionResponses);
            }


        }

        // DELETE: api/Item/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(string id)
        {
            var item = await _context.tItem.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.tItem.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        private bool ItemExists(string id)
        {
            return _context.tItem.Any(e => e.fItemCode == id);
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
