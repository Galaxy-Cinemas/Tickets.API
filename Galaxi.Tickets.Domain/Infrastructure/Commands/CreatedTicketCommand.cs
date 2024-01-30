using MediatR;

namespace Galaxi.Tickets.Domain.Infrastructure.Commands
{
    public record CreatedTicketCommand(int FunctionId, decimal? AdditionalPrice, string UserName, int NumSeats = 1)
        : IRequest<bool>;
}
