namespace Galaxi.Tickets.Data.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int FunctionId { get; set; }
        public decimal AdditionalPrice { get; set; } = 0;
        public string UserName { get; set; }
        public int NumSeats { get; set; } = 1;
    }
}
