using Galaxi.Tickets.Data.Models;

namespace Galaxi.Tickets.Persistence.Repositorys
{
    public interface ITicketRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<Ticket> GetTicketById(int id);
        Task<IEnumerable<Ticket>> GetTicketsAsync();
        Task<bool> SaveAll();
        void Update<T>(T entity) where T : class;
    }
}