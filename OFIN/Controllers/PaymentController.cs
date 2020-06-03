using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OFIN.Models;
using OFIN.Models.Account;
using OFIN.Models.eWallet.Topup;
using OFIN.Models.Merchant;
using OFIN.Models.Payment;

namespace OFIN.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentContext _context;
        private readonly ItemContext _itemContext;
        private readonly StockContext _stockContext;
        private readonly UserContext _userContext;
        private readonly ActionResponse _actionContext;
        private readonly LogContext _logContext;
        private readonly UserLogContext _userLogContext;
        private readonly BalanceContext _balanceContext;

        public PaymentController(PaymentContext context, ItemContext itemContext, StockContext stockContext, UserContext userContext, ActionResponse actionContext, LogContext logContext, UserLogContext userLogContext, BalanceContext balanceContext)
        {
            _context = context;
            _itemContext = itemContext;
            _stockContext = stockContext;
            _userContext = userContext;
            _actionContext = actionContext;
            _logContext = logContext;
            _userLogContext = userLogContext;
            _balanceContext = balanceContext;
        }

        // GET: api/Payment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayment()
        {
            return await _context.tPayment.ToListAsync();
        }

        // GET: api/Payment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayment(string id)
        {
            var payment = await _context.tPayment.Where(x => x.fUsername == id).ToListAsync();

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        // PUT: api/Payment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(string id, Payment payment)
        {
            if (id != payment.fPaymentId)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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

        // POST: api/Payment
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Payment>> PostPayment([FromForm]Payment payment)
        {
            int totalPrice = 0;
            ActionResponses actionResponses;
            if (UserExists(payment.fUsername))
            {
                if (ItemExists(payment.fItemCode))
                {
                    if (isStockEnough(payment.fItemCode, payment.fQty))
                    {
                        if (GetBalance(payment.fUsername) < GetTotalPrice(payment.fItemCode, payment.fQty))
                        {
                           
                            actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "6");
                            InsertLog("Create Payment", "Execution Failed", "6");
                            return CreatedAtAction("GetPayment", actionResponses);
                        }
                        else
                        {
                            totalPrice = GetTotalPrice(payment.fItemCode, payment.fQty);
                            payment.fPaymentDate = DateTime.Now;
                            _context.tPayment.Add(payment);
                            await _context.SaveChangesAsync();

                            using (_stockContext)
                            {
                                var entity = _stockContext.tStock.FirstOrDefault(item => item.fItemCode == payment.fItemCode);
                                if (entity != null)
                                {
                                    entity.fItemCode = payment.fItemCode;
                                    entity.fItemStock = entity.fItemStock - payment.fQty;
                                    _stockContext.SaveChanges();

                                    InsertLog("Decrease Stock", "Execution Success", "0");
                                }
                            }

                            using (_balanceContext)
                            {
                                var entity = _balanceContext.tBalance.FirstOrDefault(item => item.fUsername == payment.fUsername);
                                if (entity != null)
                                {
                                    entity.fUsername = payment.fUsername;
                                    entity.fBalance = entity.fBalance - totalPrice;
                                    _balanceContext.SaveChanges();

                                    InsertLog("Decrease Balance", "Execution Success", "0");
                                }
                            }

                            using (_itemContext)
                            {
                                var entity = _itemContext.tItem.FirstOrDefault(item => item.fItemCode == payment.fItemCode);
                                if (entity != null)
                                {
                                    entity.fItemCode = payment.fItemCode;
                                    entity.fItemBought = entity.fItemBought + payment.fQty;
                                    _itemContext.SaveChanges();
                                }
                            }

                            actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "0");
                            InsertLog("Create Payment", "Execution Success", "0");
                            InsertUserLog(payment.fUsername, "Payment", "Payment successfully executed. Total Price is: " + totalPrice + "");
                            return CreatedAtAction("GetPayment", actionResponses);
                        }
                    }
                    else
                    {

                        actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "5");
                        InsertLog("Create Payment", "Execution Failed", "5");
                        return CreatedAtAction("GetPayment", actionResponses);

                    }


                }
                else
                {
                    actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "7");
                    InsertLog("Create Payment", "Execution Failed", "7");
                    return CreatedAtAction("GetPayment", actionResponses);
                }
            }
            else
            {
                actionResponses = _actionContext.tActionResponses.FirstOrDefault(x => x.fErrorCode == "8");
                InsertLog("Create Payment", "Execution Failed", "8");
                return CreatedAtAction("GetPayment", actionResponses);
            }



        }

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Payment>> DeletePayment(string id)
        {
            var payment = await _context.tPayment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.tPayment.Remove(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        private bool PaymentExists(string id)
        {
            return _context.tPayment.Any(e => e.fPaymentId == id);
        }

        private bool ItemExists(string id)
        {
            return _itemContext.tItem.Any(e => e.fItemCode == id);
        }

        private bool isStockEnough(string itemCode, int stockQty)
        {
            var stock = _stockContext.tStock.FirstOrDefault(x => x.fItemCode == itemCode);
            if (stock.fItemStock >= stockQty)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        private bool UserExists(string Username)
        {
            return _userContext.tUser.Any(e => e.fUsername == Username);
        }

        private int GetBalance(string Username)
        {
            try
            {
                var balance = _balanceContext.tBalance.FirstOrDefault(x => x.fUsername == Username);
                return balance.fBalance;
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        private int GetTotalPrice(string ItemCode, int Qty)
        {
            var item = _itemContext.tItem.FirstOrDefault(x => x.fItemCode == ItemCode);
            return Convert.ToInt32(item.fItemPrice)* Qty;
        }

    }
}
