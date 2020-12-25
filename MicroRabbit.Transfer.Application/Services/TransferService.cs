 
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _repository;
        private readonly IEventBus _eventBus;
        public TransferService(ITransferRepository repository, IEventBus eventBus) {
            _repository = repository;
            _eventBus = eventBus;
        }
        public async Task<IEnumerable<TransferLog>> GetTransfersAsync() {
            return await _repository.GetTransfersAsync();
        }
    }
}
