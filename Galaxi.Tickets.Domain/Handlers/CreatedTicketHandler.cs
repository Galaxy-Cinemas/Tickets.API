using AutoMapper;
using Galaxi.Bus.Message;
using Galaxi.Tickets.Data.Models;
using Galaxi.Tickets.Domain.Infrastructure.Commands;
using Galaxi.Tickets.Persistence.Repositorys;
using MassTransit;
using MediatR;
using System.Net.Http;

namespace Galaxi.Tickets.Domain.Handlers
{
    public class CreatedTicketHandler
        : IRequestHandler<CreatedTicketCommand, bool>
    {
        private readonly ITicketRepository _repo;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public CreatedTicketHandler(ITicketRepository repo, IMapper mapper, IBus bus)
        {
            _repo = repo;
            _mapper = mapper;
            _bus = bus;
        }
        public async Task<bool> Handle(CreatedTicketCommand request, CancellationToken cancellationToken)
        {
            Ticket createdMovie = _mapper.Map<Ticket>(request);

            _repo.Add(createdMovie);

            var created = await _repo.SaveAll();

            await _bus.Publish(new TickedCreated
            {
                FunctionId = createdMovie.FunctionId,
                NumSeat = createdMovie.NumSeats,
                //Email = HttpContext.Request.Headers["Email"]
            });

            return created;
        }
    }
}
