using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankingDbContext _context;

        public AccountRepository(BankingDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync() {
            return await _context.Accounts.ToListAsync();
        }
    }
}
