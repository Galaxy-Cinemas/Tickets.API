using Galaxi.Tickets.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Galaxi.Tickets.Persistence.Configurations
{
    public class TicketConfig : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder
                .ToTable("Ticket", "DBO")
                .HasKey(x => new { x.Id });
        }
    }
}
