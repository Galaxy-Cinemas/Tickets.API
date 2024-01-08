using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
