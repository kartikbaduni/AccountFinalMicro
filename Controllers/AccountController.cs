using AccountMicroservice.DTO;
using AccountMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AccountMicroservice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountDbContext accountDb;

        public AccountController(AccountDbContext accountDb)
        {
            this.accountDb = accountDb;
        }

        [HttpPost]
        public List<AccountCreationStatus> CreateAccount([Required] int customerId)
        {
            var savingAc = new Account
            {
                AccountCreationDate = DateTime.Now,
                CustomerId = customerId,
                AccountType = "Savings",
                CurrentBalance = 0
            };

            var currentAc = new Account
            {
                AccountCreationDate = DateTime.Now,
                CustomerId = customerId,
                AccountType = "Current",
                CurrentBalance = 0
            };

            accountDb.Accounts.Add(savingAc);
            accountDb.Accounts.Add(currentAc);
            accountDb.SaveChanges();

            Deposit(savingAc.AccountId, 500);
            Deposit(currentAc.AccountId, 1000);

            return new List<AccountCreationStatus>()
            {
                new AccountCreationStatus
                {
                    AccountId=savingAc.AccountId,
                    Message="Saving account created"
                },
                new AccountCreationStatus
                {
                    AccountId=currentAc.AccountId,
                    Message="Current account created"
                }
            };
        }

        [HttpGet]
        public ActionResult<List<Account>> GetCustomerAccounts(int customerid)
        {
            var accounts = accountDb.Accounts.Where(x => x.CustomerId == customerid).ToList();
            if (accounts.Any())
                return Ok(accounts);
            else
                return NotFound($"No accounts found with customer Id : {customerid}");
        }

        [HttpGet]
        public ActionResult<int> GetAccount(int accountId)
        {
            var account = accountDb.Accounts.FirstOrDefault(x => x.AccountId == accountId);
            if (account == null)
                return NotFound();
            else
                return Ok(account.CurrentBalance);
        }

        [HttpGet]
        public ActionResult<List<Statement>> GetAccountStatement(int accountId, DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                var statements = accountDb.Statements.Where(x => x.AccountId == accountId && x.TransactionDate > start.Value && x.TransactionDate < end.Value).ToList();
                if (statements.Count > 0)
                    return Ok(statements.OrderBy(x => x.TransactionDate));
                else
                    return NoContent();
            }
            else
            {
                var statements = accountDb.Statements.Where(x => x.AccountId == accountId && x.TransactionDate > DateTime.Now.AddMonths(-1)).ToList();
                if (statements.Count > 0)
                    return Ok(statements.OrderBy(x => x.TransactionDate));
                else
                    return NoContent();
            }
        }

        [HttpPost]
        public ActionResult<TransactionStatus> Deposit(int accountId, double amount)
        {
            var account = accountDb.Accounts.SingleOrDefault(x => x.AccountId == accountId);
            if (account == null)
                return BadRequest("Account not found");
            account.CurrentBalance += amount;

            var st = new Statement()
            {
                AccountId = accountId,
                TransactionDate = DateTime.Now,
                ValueDate = DateTime.Now.AddSeconds(5),
                Withdrawal = 0,
                Deposit = amount,
                ClosingBalance = account.CurrentBalance
            };

            accountDb.Statements.Add(st);
            accountDb.SaveChanges();

            return Ok(new TransactionStatus { Message = "Success", TransactionID = st.TransactionID, Updated_Balance = st.ClosingBalance });
        }

        [HttpPost]
        public ActionResult<TransactionStatus> Withdraw(int accountId, double amount)
        {
            var account = accountDb.Accounts.SingleOrDefault(x => x.AccountId == accountId);
            if (account == null)
                return BadRequest("Account not found");
            account.CurrentBalance -= amount;

            var st = new Statement()
            {
                AccountId = accountId,
                TransactionDate = DateTime.Now,
                ValueDate = DateTime.Now.AddSeconds(5),
                Withdrawal = amount,
                Deposit = 0,
                ClosingBalance = account.CurrentBalance
            };

            accountDb.Statements.Add(st);
            accountDb.SaveChanges();

            return Ok(new TransactionStatus { Message = "Success", TransactionID = st.TransactionID, Updated_Balance = st.ClosingBalance });
        }

        [HttpGet]
        public List<Account> GetAllAccounts()
        {
            return accountDb.Accounts.ToList();
        }

        [HttpGet]
        public Account GetAccountDetail(int accountId)
        {
            return accountDb.Accounts.SingleOrDefault(x => x.AccountId == accountId);
        }
    }
}
