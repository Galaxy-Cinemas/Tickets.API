using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxi.Tickets.Domain.DTOs
{
    public class TicketSummaryDto
    {
        public Guid TicketId { get; set; }
        public Guid FunctionId { get; set; }
        public string UserName { get; set; }
    }
}
