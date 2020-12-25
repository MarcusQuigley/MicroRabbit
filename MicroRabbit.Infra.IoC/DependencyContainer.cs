using System;
using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.CommandsHandlers;
using MicroRabbit.Banking.Domain.Interfaces;
 
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace MicroRabbit.Infra.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services) {
            //bus
            services.AddSingleton<IEventBus, RabbitMQBus>(serviceProvider => {
                var mediator = serviceProvider.GetService<IMediator>();
                //var serviceScopeFactory = serviceProvider.GetServices<IServiceScopeFactory>();
                return new RabbitMQBus(mediator);
            });
            //domain banking
            services.AddTransient<IRequestHandler<CreateTransferCommand,bool>, TransferCommandHandler>();

            //service
            services.AddTransient<IAccountService, AccountService>();

            //data
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ITransferRepository, TransferRepository>();
            services.AddTransient<BankingDbContext>();
            services.AddTransient<TransferDbContext>();

            //services.AddTransient<IMediator>();
        }
    }
}
