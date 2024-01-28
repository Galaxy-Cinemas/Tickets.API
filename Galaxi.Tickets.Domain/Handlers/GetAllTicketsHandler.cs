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

namespace Galaxi.Tickets.Domain.Handlers
{
    public class GetAllTicketsHandler
         : IRequestHandler<GetAllTicketQuery, IEnumerable<TicketDto>>
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
        public async Task<IEnumerable<TicketDto>> Handle(GetAllTicketQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var ticket = await _repo.GetTicketsAsync();
                var ticketViewModel = _mapper.Map<List<TicketDto>>(ticket);
                return ticketViewModel;
            }
            catch (Exception ex)
            {
                _log.LogError("An exception has occurred getting all tickets {0}", ex.Message);
                throw;
            }
        }
    }
}
