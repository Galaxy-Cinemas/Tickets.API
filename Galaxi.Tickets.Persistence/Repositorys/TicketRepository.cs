using Galaxi.Tickets.Data.Models;
using Galaxi.Tickets.Persistence.Persistence;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Galaxi.Tickets.Persistence.Repositorys
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketContextDb _context;
        private readonly ICacheRedis _cache;
        private const string _cacheKeyAllTicketsByUser = "all_tickets_";
        private const string _cacheKeyTicket = "ticket_";
        private const string _cacheKeyFunctionById = "ticketByFunctionId_";

        public TicketRepository(TicketContextDb context, ICacheRedis cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task Add(Ticket ticket)
        {
            _context.Add(ticket);
            await _cache.RemoveCacheAsync(_cacheKeyTicket, _cacheKeyFunctionById, _cacheKeyAllTicketsByUser, emailUser: ticket.UserName, ticketId: ticket.TicketId);
        }
        public async Task Delete(Ticket ticket)
        {
            _context.Remove(ticket);
            await _cache.RemoveCacheAsync(_cacheKeyTicket, _cacheKeyFunctionById, _cacheKeyAllTicketsByUser, ticket.TicketId, ticket.FunctionId);
        }
        public async Task Update(Ticket ticket)
        {
            _context.Update(ticket);
            await _cache.RemoveCacheAsync(_cacheKeyTicket, _cacheKeyFunctionById, _cacheKeyAllTicketsByUser, ticket.TicketId, ticket.FunctionId);
        }
        public async Task<Ticket> GetTicketByIdAsync(Guid ticketId)
        {
            var cacheKey = $"{_cacheKeyTicket}{ticketId}";

            Ticket cacheTicket = null;

            cacheTicket = await _cache.GetCacheAsync<Ticket>(cacheKey);

            if (cacheTicket != null)
            {
                return cacheTicket;
            }

            var ticket = await _context.Ticket.FirstOrDefaultAsync(u => u.TicketId == ticketId);
            if (ticket != null)
            {
                _ = _cache.SetCacheAsync(ticket, cacheKey);
            }
            return ticket;
        }
        public async Task<IEnumerable<Ticket>> GetTicketsByUserAsync(string emailUser)
        {
            var cacheKey = $"{_cacheKeyAllTicketsByUser}{emailUser}";

            var cacheTickets = await _cache.GetCacheAsync<IEnumerable<Ticket>>(cacheKey);

            if (cacheTickets != null)
            {
                return cacheTickets;
            }
            var tickets = _context.Ticket.Where(u => u.UserName == emailUser);
            if (tickets != null && tickets.Any())
            {
                _ = _cache.SetCacheAsync(tickets, cacheKey);
            }
            return tickets;
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task MigrateAsync()
        {
            await _context.Database.MigrateAsync();
        }
    }
}
