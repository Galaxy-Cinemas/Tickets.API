using Galaxi.Tickets.Domain.DTOs;
using MediatR;

namespace Galaxi.Tickets.Domain.Infrastructure.Commands
{
    public record BuyTicketCommand(Guid FunctionId, decimal? AdditionalPrice, string UserName, string UserEmail, int NumSeats = 1)
        : IRequest<TicketDetailsDto>;
}
