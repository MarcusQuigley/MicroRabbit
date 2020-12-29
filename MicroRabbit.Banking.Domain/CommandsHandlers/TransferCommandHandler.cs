using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Domain.CommandsHandlers
{
    public class TransferCommandHandler : IRequestHandler<CreateTransferCommand, bool>
    {
        private readonly IEventBus _eventBus;

        public TransferCommandHandler(IEventBus eventBus) {
            _eventBus = eventBus;
        }

        //iniitate transfer . its a command sent thru bus from our service and is handled in our command handler
        // the transfer command handler will publish an event to rabbit mq
        public   Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken) {
            //publish event to RabbitMQ
            //Automapper.ProjectTo..
             _eventBus.Publish(new TransferCreatedEvent(request.From,request.To,request.Amount));
            return Task.FromResult(true);
        }
    }
}
