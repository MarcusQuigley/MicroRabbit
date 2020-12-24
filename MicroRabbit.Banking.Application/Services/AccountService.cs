
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Application.Services
{
  
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        public AccountService(IAccountRepository repository) {
            _repository = repository;
        }
        public async Task<IEnumerable<Account>> GetAccountsAsync() {
            return await _repository.GetAccountsAsync();
        }
    }
}
