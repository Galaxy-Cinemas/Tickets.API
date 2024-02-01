using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Galaxi.Bus.Message
{
    public record TickedCreated
    {
        public int FunctionId { get; init; }
        public int NumSeat { get; init; }
        public string Email { get; init; }
    }

    public record CheckFunctionSeats
    {
        public int FunctionId { get; init; }
    }

    public record FunctionStatusSeats
    {
        public bool Exist { get; init; }
        public int NumSeatAvailable { get; init; }
    }



    
}
