using Galaxi.Tickets.Domain.DTOs;
using MediatR;

namespace Galaxi.Tickets.Domain.Infrastructure.Queries
{
    public record GetAllTicketsByUserEmailQuery(string email) : IRequest<IEnumerable<TicketSummaryDto>>;
}
