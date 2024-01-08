using MediatR;

namespace Galaxi.Tickets.Domain.Infrastructure.Commands
{
    public record CreatedTicketCommand
        : IRequest<bool>;

}
