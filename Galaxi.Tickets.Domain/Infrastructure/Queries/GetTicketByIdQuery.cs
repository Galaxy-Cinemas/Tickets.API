using Galaxi.Tickets.Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxi.Tickets.Domain.Infrastructure.Queries
{
    public record GetTicketByIdQuery(int ticketId) : IRequest<TicketDto>;
}
