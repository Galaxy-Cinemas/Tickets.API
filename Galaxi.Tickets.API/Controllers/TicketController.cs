using Galaxi.Tickets.Domain.Infrastructure.Commands;
using Galaxi.Tickets.Domain.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var Email = HttpContext.Request.Headers["email"];
            var created = await _mediator.Send(ticketToCreate);
            if (created)
                return Ok(ticketToCreate);

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
