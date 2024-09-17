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
using Galaxi.Tickets.Persistence.Repositorys;
using Galaxi.Tickets.Domain.Response;

namespace Galaxi.Tickets.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[action]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITicketServices _serviceTicket;
        private readonly ITicketRepository _repo;
        private readonly ILogger<TicketController> _log;

        public TicketController(ITicketRepository repo,ILogger<TicketController> log,  IMediator mediator, ITicketServices serviceTicket)
        {
            _mediator = mediator;
            _serviceTicket = serviceTicket;
            _repo = repo;
            _log = log;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> migrate()
        {
            await _repo.MigrateAsync();
            var successResponse = ResponseHandler<string>.CreateSuccessResponse("DB has been migrated successfully", null);
            return StatusCode(successResponse.StatusCode.Value, successResponse);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _log.LogInformation("Get all tickets");
                var tickets = await _mediator.Send(new GetAllTicketQuery());
                var successResponse = ResponseHandler<IEnumerable<TicketSummaryDto>>.CreateSuccessResponse("Tickets retrieved successfully", tickets);
                return StatusCode(successResponse.StatusCode.Value, successResponse);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> buyTicked(CreatedTicketCommand ticketToCreate)
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

            var TickedBought = await _mediator.Send(newCreateTicket);
            if (TickedBought) {
                var successResponse = ResponseHandler<CreatedTicketCommand>.CreateSuccessResponse("Ticked bought successfully", newCreateTicket);
                return StatusCode(successResponse.StatusCode.Value, successResponse);
            }

            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                GetTicketByIdQuery ticketById = new GetTicketByIdQuery(ticketId:id);
                    
                _log.LogInformation("Get ticket {0}", id);
                var ticket = await _mediator.Send(ticketById);
                var successResponse = ResponseHandler<TicketDetailsDto>.CreateSuccessResponse("Ticked by id retrieved successfully", ticket);
                return StatusCode(successResponse.StatusCode.Value, successResponse);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
