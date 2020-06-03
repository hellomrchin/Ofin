using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OFIN.Models;
using OFIN.Models.Merchant;

namespace OFIN.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]

    public class StockController : ControllerBase
    {
        private readonly StockContext _context;
        private readonly ActionResponse _actionContext;
        private readonly LogContext _logContext;
        private readonly ItemContext _itemContext;
        public StockController(StockContext context, ItemContext itemContext, ActionResponse actionContext, LogContext logContext)
        {
            _context = context;
            _actionContext = actionContext;
            _itemContext = itemContext;
            _logContext = logContext;
        }

        // GET: api/Stock
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stocks>>> GetStock()
        {
            return await _context.tStock.ToListAsync();
        }

        // GET: api/Stock/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Stocks>> GetStocks(string id)
        {
            var stocks = await _context.tStock.FindAsync(id);

            if (stocks == null)
            {
                return NotFound();
            }

            return stocks;
        }

        // PUT: api/Stock/5
        [HttpPut("{id}")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> PutStocks(string id, [FromForm]Stocks stocks)
        {
            ActionResponses actionResponses;
            if (User.IsInRole("Admin"))
            {

                if (id != stocks.fItemCode)
                {
                    return BadRequest();
                }

                using (_context)
                {
                    var entity = _context.tStock.FirstOrDefault(item => item.fItemCode == id);
                    if (entity != null)
                    {
                        entity.fItemCode = stocks.fItemCode;
                        entity.fItemStock = stocks.fItemStock + entity.fItemStock;
                        entity.fLastUpdate = DateTime.Now;
                        _context.SaveChanges();

                        InsertLog("Update Stock", "Execution Success", "0");
                        actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                        return CreatedAtAction("GetStock", actionResponses);
                    }
                }

                return NoContent();
            }
            else
            {
                InsertLog("Update Stock", "Execution Failed", "4");
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "4");
                return CreatedAtAction("GetStock", actionResponses);
            }


        }

        // POST: api/Stock
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Stocks>> PostStocks([FromForm]Stocks stocks)
        {

            ActionResponses actionResponses;
            if (User.IsInRole("Admin"))
            {
                if (ItemExists(stocks.fItemCode))
                {
                    if (StocksExists(stocks.fItemCode))
                    {
                        actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "1");
                        InsertLog("Create Stock", "Execution Failed", "1");
                        return CreatedAtAction("GetStock", actionResponses);
                    }

                    _context.tStock.Add(stocks);
                    await _context.SaveChangesAsync();
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                    InsertLog("Create Stock", "Execution Success", "0");
                    return CreatedAtAction("GetStock", actionResponses);
                }
                else
                {
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "3");
                    InsertLog("Create Stock", "Execution Failed", "3");
                    return CreatedAtAction("GetStock", actionResponses);
                }
            }
            else
            {
                InsertLog("Insert Stock", "Execution Failed", "4");
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "4");
                return CreatedAtAction("GetStock", actionResponses);
            }


        }

        // DELETE: api/Stock/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Stocks>> DeleteStocks(string id)
        {
            var stocks = await _context.tStock.FindAsync(id);
            if (stocks == null)
            {
                return NotFound();
            }

            _context.tStock.Remove(stocks);
            await _context.SaveChangesAsync();

            return stocks;
        }

        private bool StocksExists(string id)
        {
            return _context.tStock.Any(e => e.fItemCode == id);
        }


        private bool ItemExists(string id)
        {
            return _itemContext.tItem.Any(e => e.fItemCode == id);
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
