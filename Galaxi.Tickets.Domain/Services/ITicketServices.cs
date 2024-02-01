using Galaxi.Tickets.Domain.DTOs;

namespace Galaxi.Tickets.Domain.Services
{
    public interface ITicketServices
    {
        TokenUserInfo DeserealizeToken(string Authorization);
    }
}