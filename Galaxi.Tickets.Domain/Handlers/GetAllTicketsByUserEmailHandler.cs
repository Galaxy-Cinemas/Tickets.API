using AutoMapper;
using Galaxi.Tickets.Domain.DTOs;
using Galaxi.Tickets.Domain.Infrastructure.Queries;
using Galaxi.Tickets.Persistence.Repositorys;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Galaxi.Tickets.Domain.Handlers
{
    public class GetAllTicketsByUserEmailHandler
         : IRequestHandler<GetAllTicketsByUserEmailQuery, IEnumerable<TicketSummaryDto>>
    {
        private readonly ITicketRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTicketsByUserEmailHandler> _log;

        public GetAllTicketsByUserEmailHandler(ITicketRepository repo, IMapper mapper, ILogger<GetAllTicketsByUserEmailHandler> log)
        {
            _repo = repo;
            _mapper = mapper;
            _log = log;
        }
        public async Task<IEnumerable<TicketSummaryDto>> Handle(GetAllTicketsByUserEmailQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _repo.GetTicketsByUserAsync(request.email);
            if (ticket == null || !ticket.Any())
            {
                throw new KeyNotFoundException();
            }
            var ticketViewModel = _mapper.Map<List<TicketSummaryDto>>(ticket);
            return ticketViewModel;
        }
    }
}
