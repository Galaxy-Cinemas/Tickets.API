using Galaxi.Tickets.Data.Models;
using Galaxi.Tickets.Domain.DTOs;
using Galaxi.Tickets.Domain.Infrastructure.Commands;
using Galaxi.Tickets.Domain.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Galaxi.Tickets.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("v1/[controller]/[action]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TicketController> _log;

        public TicketController(ILogger<TicketController> log,  IMediator mediator)
        {
            _mediator = mediator;
            _log = log;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _log.LogInformation("Get all tickets");
                var tickets = await _mediator.Send(new GetAllTicketQuery());
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreatedTicketCommand ticketToCreate)
        {
            string Authorization = HttpContext.Request.Headers["Authorization"];
            var Token = Authorization.Remove(0, 7);

            var tokenHandler = new JwtSecurityTokenHandler();

            // Lee y valida el token JWT
            var token = tokenHandler.ReadJwtToken(Token);

            // Accede a la carga útil (payload)
            var payload = token.Payload;

            // Serializa la carga útil a una instancia de JwtPayload
            var jsonPayload = JsonConvert.SerializeObject(payload);
            TokenUserInfo jwtPayload = JsonConvert.DeserializeObject<TokenUserInfo>(jsonPayload);

            CreatedTicketCommand newCreateTicket = new CreatedTicketCommand
                (
                FunctionId: ticketToCreate.FunctionId,
                AdditionalPrice: ticketToCreate.AdditionalPrice,
                UserName: jwtPayload.email,
                NumSeats: ticketToCreate.NumSeats
                );

        var created = await _mediator.Send(newCreateTicket);
            if (created)
                return Ok(newCreateTicket);

            return BadRequest();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                GetTicketByIdQuery ticketById = new GetTicketByIdQuery(ticketId:id);
                    
                _log.LogInformation("Get ticket {0}", id);
                var ticket = await _mediator.Send(ticketById);
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
