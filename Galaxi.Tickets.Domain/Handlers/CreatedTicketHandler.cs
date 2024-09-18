using AutoMapper;
using FluentValidation;
using Galaxi.Bus.Message;
using Galaxi.Tickets.Data.Models;
using Galaxi.Tickets.Domain.DTOs;
using Galaxi.Tickets.Domain.Infrastructure.Commands;
using Galaxi.Tickets.Persistence.Repositorys;
using MassTransit;
using MediatR;

namespace Galaxi.Tickets.Domain.Handlers
{
    public class CreatedTicketHandler
        : IRequestHandler<CreatedTicketCommand, TicketDetailsDto>
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
        public async Task<TicketDetailsDto> Handle(CreatedTicketCommand request, CancellationToken cancellationToken)
        {
            TicketDetailsDto requestTicket = _mapper.Map<TicketDetailsDto>(request);

            var result = await _validatorAvailableSeats.ValidateAsync(requestTicket);

            if (!result.IsValid) throw new KeyNotFoundException();

            Ticket createdMovie = _mapper.Map<Ticket>(request);

            _repo.Add(createdMovie);

            var sucess = await _repo.SaveAll();

            if (!sucess) throw new InvalidOperationException();
            
            await _bus.Publish(new TickedCreated
            {
                FunctionId = createdMovie.FunctionId,
                NumSeat = createdMovie.NumSeats,
            });

            var ticketBought = _mapper.Map<TicketDetailsDto>(requestTicket);

            return ticketBought;
        }
    }
}
