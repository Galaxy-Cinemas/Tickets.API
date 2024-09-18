using AutoMapper;
using Galaxi.Tickets.Domain.DTOs;
using Galaxi.Tickets.Domain.Infrastructure.Queries;
using Galaxi.Tickets.Persistence.Repositorys;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Galaxi.Tickets.Domain.Handlers
{
    public class GetAllTicketsHandler
         : IRequestHandler<GetAllTicketQuery, IEnumerable<TicketSummaryDto>>
    {
        private readonly ITicketRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTicketsHandler> _log;

        public GetAllTicketsHandler(ITicketRepository repo, IMapper mapper, ILogger<GetAllTicketsHandler> log)
        {
            _repo = repo;
            _mapper = mapper;
            _log = log;
        }
        public async Task<IEnumerable<TicketSummaryDto>> Handle(GetAllTicketQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _repo.GetTicketsAsync();
            if (ticket == null || !ticket.Any())
            {
                throw new KeyNotFoundException();
            }
            var ticketViewModel = _mapper.Map<List<TicketSummaryDto>>(ticket);
            return ticketViewModel;
        }
    }
}
