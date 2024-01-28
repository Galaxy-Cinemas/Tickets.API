using AutoMapper;
using Galaxi.Tickets.Data.Models;
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
    public class GetTicketByIdHandler
        : IRequestHandler<GetTicketByIdQuery, TicketDto>
    {
        private readonly ITicketRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTicketByIdHandler> _log;
        public GetTicketByIdHandler(ITicketRepository repo, IMapper mapper, ILogger<GetTicketByIdHandler> log)
        {
            _repo = repo;
            _mapper = mapper;
            _log = log;
        }

        public async Task<TicketDto> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Ticket ticketById = await _repo.GetTicketById(request.ticketId);
                var ticketByIdViewModel = _mapper.Map<TicketDto>(ticketById);
                return ticketByIdViewModel;
            }
            catch (Exception ex)
            {
                _log.LogError("An exception has occurred getting the ticket {0}", ex.Message);
                throw;
            }
        }
    }
}
