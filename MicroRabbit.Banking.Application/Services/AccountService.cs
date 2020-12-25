
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Application.Services
{
  
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IEventBus _eventBus;
        public AccountService(IAccountRepository repository, IEventBus eventBus) {
            _repository = repository;
            _eventBus = eventBus;
        }
        public async Task<IEnumerable<Account>> GetAccountsAsync() {
            return await _repository.GetAccountsAsync();
        }

        public async Task TransferAsync(AccountTransfer accountTransfer) {
            var createTransferCommand = new CreateTransferCommand(
               accountTransfer.AccountFrom,
               accountTransfer.AccountTo,
               accountTransfer.TransferAmount
               );
           await _eventBus.SendCommand(createTransferCommand);
        }
    }
}
