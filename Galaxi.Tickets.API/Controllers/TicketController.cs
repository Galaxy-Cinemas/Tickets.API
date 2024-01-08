using Galaxi.Tickets.Domain.Infrastructure.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Galaxi.Tickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatedTicketCommand ticketToCreate)
        {
            var created = await _mediator.Send(ticketToCreate);
            if (created)
                return Ok(ticketToCreate);

            return BadRequest();
        }
    }
}
