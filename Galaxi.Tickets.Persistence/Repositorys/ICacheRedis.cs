
namespace Galaxi.Tickets.Persistence.Repositorys
{
    public interface ICacheRedis
    {
        Task<T> GetCacheAsync<T>(string cacheKey) where T : class;
        Task RemoveCacheAsync(string cacheKeyTicket, string cacheKeyTicketById, string cacheKeyAllTickets, Guid? ticketId = null, Guid? functionId = null);
        Task SetCacheAsync<T>(T entity, string cacheKey);
    }
}