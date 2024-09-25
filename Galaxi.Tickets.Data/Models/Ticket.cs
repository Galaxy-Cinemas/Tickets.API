namespace Galaxi.Tickets.Data.Models
{
    public class Ticket
    {
        public Guid TicketId { get; set; }
        public Guid FunctionId { get; set; }
        public decimal AdditionalPrice { get; set; } = 0;
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int NumSeats { get; set; } = 1;
    }
}
