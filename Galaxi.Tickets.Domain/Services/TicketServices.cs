using Galaxi.Tickets.Domain.DTOs;
using Galaxi.Tickets.Domain.Infrastructure.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Galaxi.Tickets.Domain.Services
{
    public class TicketServices : ITicketServices
    {
        public TokenUserInfo DeserealizeToken(string Authorization)
        {
            try
            {
                var Token = Authorization.Remove(0, 7);

                var tokenHandler = new JwtSecurityTokenHandler();

                // Lee y valida el token JWT
                var token = tokenHandler.ReadJwtToken(Token);

                // Accede a la carga útil (payload)
                var payload = token.Payload;

                // Serializa la carga útil a una instancia de JwtPayload
                var jsonPayload = JsonConvert.SerializeObject(payload);
                return JsonConvert.DeserializeObject<TokenUserInfo>(jsonPayload);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
