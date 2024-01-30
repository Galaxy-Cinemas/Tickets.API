using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxi.Tickets.Domain.DTOs
{
    public class TokenUserInfo
    {
        public string nameid { get; set; }
        public string email { get; set; }
        public string unique_name { get; set; }
        public int exp { get; set; }
        public string family_name { get; set; }
        public string role { get; set; }
    }
}
