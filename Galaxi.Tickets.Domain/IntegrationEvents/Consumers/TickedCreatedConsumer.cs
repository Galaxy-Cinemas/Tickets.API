using Galaxi.Bus.Message;
using Galaxi.Tickets.Persistence.Repositorys;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Galaxi.Tickets.Domain.IntegrationEvents.Consumers
{
    public class TickedCreatedConsumer : IConsumer<TickedCreated>
    {
        private readonly ILogger<TickedCreatedConsumer> _log;
        private readonly ITicketRepository _repo;
        public TickedCreatedConsumer(ILogger<TickedCreatedConsumer> log, ITicketRepository repo)
        {
            _log = log;
            _repo = repo;
        }

        public Task Consume(ConsumeContext<TickedCreated> context)
        {
            _log.LogInformation("Nuevo evento: Se crea un Ticket}.", context.Message.MovietId);
            _log.LogWarning("Movie Id: {0}", context.Message.MovietId);

            

            throw new NotImplementedException();
        }
    }
}
