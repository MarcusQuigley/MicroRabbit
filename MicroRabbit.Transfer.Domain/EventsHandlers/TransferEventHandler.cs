using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Domain.EventsHandlers
{
    public class TransferEventHandler : IEventHandler<TransferCreatedEvent>
    {
        private readonly ITransferRepository _transferRepository;
        public TransferEventHandler(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }
        public Task<bool> Handle(TransferCreatedEvent @event)
        {
            _transferRepository.AddAsync(new Models.TransferLog()
            {
                AccountFrom = @event.From,
                AccountTo = @event.To,
                TransferAmount = @event.Amount,
            });
            return _transferRepository.SaveChangesAsync();
            //return Task.CompletedTask;
        }
    }
}
