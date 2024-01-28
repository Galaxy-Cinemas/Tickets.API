using Galaxi.Tickets.Domain.DTOs;
using MediatR;

namespace Galaxi.Tickets.Domain.Infrastructure.Queries
{
    public record GetAllTicketQuery : IRequest<IEnumerable<TicketDto>>;
}
