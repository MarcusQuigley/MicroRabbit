using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Infra.Bus
{
    //
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly IList<Type> _eventTypes;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _eventTypes = new List<Type>();
            _handlers = new Dictionary<string, List<Type>>();
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }
        //whoeever is using this sends a generic event
        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = @event.GetType().Name;
                channel.QueueDeclare(eventName, false, false, false, null); //create q for each type
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName, null, body);
            }
        }
        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);
            if (!_eventTypes.Contains(typeof(T)))
                _eventTypes.Add(typeof(T));

            if (!_handlers.ContainsKey(eventName))
                _handlers.Add(eventName, new List<Type>());

            if (_handlers[eventName].Any(t => t.GetType() == handlerType))
                throw new ArgumentException($"Handler Type {handlerType.Name} already is registered for {eventName}", nameof(handlerType));
            _handlers[eventName].Add(handlerType);
            StartBasicConsume<T>(); //listening for msgs in q
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true //async consumer
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;
            channel.QueueDeclare(eventName, false, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_ReceivedAsync;
            channel.BasicConsume(eventName, true, consumer);

        }

        private async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
        {
            var eventName = @event.RoutingKey;
            var message = Encoding.UTF8.GetString(@event.Body.ToArray());
            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception)
            {

            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                       
                        //var handler = Activator.CreateInstance(subscription);
                        var handler = scope.ServiceProvider.GetRequiredService(subscription);
                        if (handler == null) continue;
                        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                        //(IEventHandler<>)handler.Handle(@event);
                    }
                }
            }
        }
    }
}
