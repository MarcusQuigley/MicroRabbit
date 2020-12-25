using MicroRabbit.Banking.Application;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Api.Controllers
{
    [Route("api/banking")]
    [ApiController]
    public class BankingController : ControllerBase
    {

        private readonly IAccountService _accountService;

        public BankingController(IAccountService accountService) {
            _accountService = accountService;
        } 

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts() {
            var accounts = await _accountService.GetAccountsAsync();
            return Ok(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransfer([FromBody] AccountTransfer accountTransfer) {
            await _accountService.TransferAsync(accountTransfer);
            return Ok();
        }
    }
}
