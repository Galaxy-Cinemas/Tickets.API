using AutoMapper;
using Galaxi.Tickets.Data.Models;
using Galaxi.Tickets.Domain.Infrastructure.Commands;
using Galaxi.Tickets.Persistence.Repositorys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxi.Tickets.Domain.Handlers
{
    public class CreatedTicketHandler
        : IRequestHandler<CreatedTicketCommand, bool>
    {
        private readonly ITicketRepository _repo;
        private readonly IMapper _mapper;
        public CreatedTicketHandler(ITicketRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CreatedTicketCommand request, CancellationToken cancellationToken)
        {
            var createdMovie = _mapper.Map<Ticket>(request);

            _repo.Add(createdMovie);

            return await _repo.SaveAll();
        }
    }
}
