using MediatR;

namespace Galaxi.Tickets.Domain.Infrastructure.Commands
{
    public record CreatedTicketCommand(int FunctionId, Decimal AdditionalPrice, string UserName)
        : IRequest<bool>;
}
