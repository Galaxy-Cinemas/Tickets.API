namespace Galaxi.Tickets.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int FunctionId { get; set; }
        public Decimal AdditionalPrice { get; set; }
        public string? UserName { get; set; }
    }
}
