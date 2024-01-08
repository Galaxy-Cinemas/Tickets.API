using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxi.Bus.Message
{
    public class TickedCreated
    {
        public TickedCreated(int movietId)
        {
            MovietId = movietId;
        }

        public int MovietId { get; }
    }
}
