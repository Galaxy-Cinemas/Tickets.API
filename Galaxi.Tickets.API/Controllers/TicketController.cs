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
using Galaxi.Tickets.Domain.Services;
using System.Net;

namespace Galaxi.Tickets.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("v1/[controller]/[action]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITicketServices _serviceTicket;
        private readonly ILogger<TicketController> _log;

        public TicketController(ILogger<TicketController> log,  IMediator mediator, ITicketServices serviceTicket)
        {
            _mediator = mediator;
            _serviceTicket = serviceTicket;
            _log = log;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                string Authorization = HttpContext.Request.Headers["Authorization"];
                TokenUserInfo jwtPayload = _serviceTicket.DeserealizeToken(Authorization);
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

            TokenUserInfo jwtPayload = _serviceTicket.DeserealizeToken(Authorization);

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
