using AutoMapper;
using Galaxi.Tickets.Data.Models;
using Galaxi.Tickets.Domain.DTOs;
using Galaxi.Tickets.Domain.Infrastructure.Commands;

namespace Galaxi.Tickets.Domain.Profiles
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<CreatedTicketCommand, Ticket>();
            CreateMap<CreatedTicketCommand, TicketDto>();
            CreateMap<Ticket, TicketDto>();


        }
    }
}
