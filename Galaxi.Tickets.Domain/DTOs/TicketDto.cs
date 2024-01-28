using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxi.Tickets.Domain.DTOs
{
    public class TicketDto
    {
        public int FunctionId { get; set; }
        public decimal AdditionalPrice { get; set; } = 0;
        public string UserName { get; set; }
        public int NumSeats { get; set; } = 1;
    }
}
