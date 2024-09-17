using AutoMapper;
using FluentValidation;
using Galaxi.Bus.Message;
using Galaxi.Tickets.Data.Models;
using Galaxi.Tickets.Domain.DTOs;
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
        private readonly IValidator<TicketDetailsDto> _validatorAvailableSeats;

        public CreatedTicketHandler(
            ITicketRepository repo,
            IMapper mapper,
            IBus bus,
            IValidator<TicketDetailsDto> validatorAvailableSeats
            )
        {
            _repo = repo;
            _mapper = mapper;
            _bus = bus;
            _validatorAvailableSeats = validatorAvailableSeats;
        }
        public async Task<bool> Handle(CreatedTicketCommand request, CancellationToken cancellationToken)
        {
            TicketDetailsDto requestTicket = _mapper.Map<TicketDetailsDto>(request);

            var result = await _validatorAvailableSeats.ValidateAsync(requestTicket);

            if (!result.IsValid) return false;

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
