using Galaxi.Tickets.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Galaxi.Tickets.Persistence.Persistence
{
    public interface ITicketContextDb
    {
        DbSet<Ticket> Ticket { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}