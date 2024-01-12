using Galaxi.Tickets.Data.Models;
using Galaxi.Tickets.Persistence.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Galaxi.Tickets.Persistence.Repositorys
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketContextDb _context;

        public TicketRepository(TicketContextDb context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public async Task<Ticket> GetTicketById(int id)
        {
            var ticket = await _context.Ticket.FirstOrDefaultAsync(u => u.Id == id);
            return ticket;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync()
        {
            var movie = await _context.Ticket.ToListAsync();
            return movie;
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
