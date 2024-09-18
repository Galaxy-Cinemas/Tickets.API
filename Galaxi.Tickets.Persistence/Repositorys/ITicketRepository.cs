using Galaxi.Tickets.Data.Models;

namespace Galaxi.Tickets.Persistence.Repositorys
{
    public interface ITicketRepository
    {

        Task Add(Ticket ticket);
        Task Delete(Ticket ticket);
        Task Update(Ticket ticket);

        Task<Ticket> GetTicketByIdAsync(Guid id);
        Task<IEnumerable<Ticket>> GetTicketsAsync();
        Task<bool> SaveAll();
        Task MigrateAsync();
    }
}