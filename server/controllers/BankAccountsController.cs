using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.database;
using server.models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public BankAccountsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/BankAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccountDTO>>> GetBankAccounts(int userID)
        {
            var BankAccounts = await _context.BankAccounts.Where(a => a.AccountId == userID).ToListAsync();
            return BankAccounts.Select(ItemToDTO).ToList();
        }

        // GET: api/BankAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccountDTO>> GetBankAccount(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid account ID." });
            }

            var BankAccount = await _context.BankAccounts.FindAsync(id);

            if (BankAccount == null)
            {
                return NotFound();
            }

            return ItemToDTO(BankAccount);
        }

        // PUT: api/BankAccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankAccount(int id, BankAccountDTO BankaccountDTO)
        {
            if (id != BankaccountDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(BankaccountDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankAccountExists(id))
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

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BankAccountDTO>> PostBankAccount(BankAccountDTO BankaccountDTO)
        {
            var BankAccount = new BankAccount
            {
                AccountType = BankaccountDTO.AccountType,
                Balance = BankaccountDTO.Balance
            };
            _context.BankAccounts.Add(BankAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBankAccount", new { id = BankAccount.Id }, ItemToDTO(BankAccount));
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBankAccount(int id)
        {
            var BankAccount = await _context.BankAccounts.FindAsync(id);
            if (BankAccount == null)
            {
                return NotFound();
            }

            _context.BankAccounts.Remove(BankAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BankAccountExists(int id)
        {
            return _context.BankAccounts.Any(e => e.Id == id);
        }

        private static BankAccountDTO ItemToDTO(BankAccount BankAccount) =>
            new BankAccountDTO
            {
                Id = BankAccount.Id,
                AccountType = BankAccount.AccountType,
                Balance = BankAccount.Balance
            };
    }
}
