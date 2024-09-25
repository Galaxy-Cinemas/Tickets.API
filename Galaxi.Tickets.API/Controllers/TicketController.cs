using Galaxi.Tickets.Domain.DTOs;
using Galaxi.Tickets.Domain.Infrastructure.Commands;
using Galaxi.Tickets.Domain.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Galaxi.Tickets.Domain.Services;
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

        public TicketController(ITicketRepository repo, ILogger<TicketController> log, IMediator mediator, ITicketServices serviceTicket)
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
            var successResponse = ResponseHandler<string>.SuccessResponse("DB has been migrated successfully", null);
            return StatusCode(successResponse.StatusCode.Value, successResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTicketsByUser()
        {
            try
            {
                _log.LogInformation("Get all tickets");
                string Authorization = HttpContext.Request.Headers["Authorization"];
                TokenUserInfo jwtPayload = _serviceTicket.DeserealizeToken(Authorization);

                var tickets = await _mediator.Send(new GetAllTicketQuery(jwtPayload.email));
                var successResponse = ResponseHandler<IEnumerable<TicketSummaryDto>>.SuccessResponse("Tickets retrieved successfully", tickets);
                return StatusCode(successResponse.StatusCode.Value, successResponse);
            }
            catch (KeyNotFoundException ex)
            {
                _log.LogWarning(ex.Message);
                var response = ResponseHandler<string>.NotFoundResponse("Ticket not found.", ex.Message);
                return StatusCode(response.StatusCode.Value, response);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                var errorResponse = ResponseHandler<string>.ErrorResponse("An internal server error occurred", ex);
                return StatusCode(errorResponse.StatusCode.Value, errorResponse);
            }
        }

        [HttpPost]
        public async Task<IActionResult> BuyTicked(BuyTicketCommand ticketToCreate)
        {
            try
            {
                string Authorization = HttpContext.Request.Headers["Authorization"];
                TokenUserInfo jwtPayload = _serviceTicket.DeserealizeToken(Authorization);

                BuyTicketCommand newCreateTicket = new BuyTicketCommand
                    (
                        FunctionId: ticketToCreate.FunctionId,
                        AdditionalPrice: ticketToCreate.AdditionalPrice,
                        UserEmail: jwtPayload.email,
                        UserName: jwtPayload.unique_name + " " + jwtPayload.family_name,
                        NumSeats: ticketToCreate.NumSeats
                    );

                var TickedBought = await _mediator.Send(newCreateTicket);
                var successResponse = ResponseHandler<BuyTicketCommand>.SuccessResponse("Ticked bought successfully", newCreateTicket);
                return StatusCode(successResponse.StatusCode.Value, successResponse);
            }
            catch (KeyNotFoundException ex)
            {
                _log.LogWarning(ex.Message);
                var response = ResponseHandler<string>.NotFoundResponse("Function not found.", "The Function with the specified ID does not exist.");
                return StatusCode(response.StatusCode.Value, response);
            }
            catch (InvalidOperationException ex)
            {
                _log.LogWarning(ex.Message);
                var errorResponse = ResponseHandler<string>.ErrorResponse("Failed to save changes to the database.", ex);
                return StatusCode(errorResponse.StatusCode.Value, errorResponse);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                var errorResponse = ResponseHandler<string>.ErrorResponse("An internal server error occurred", ex);
                return StatusCode(errorResponse.StatusCode.Value, errorResponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                _log.LogInformation($"Get ticket {id}");
                var ticket = await _mediator.Send(new GetTicketByIdQuery(id));
                var successResponse = ResponseHandler<TicketDetailsDto>.SuccessResponse("Ticked by id retrieved successfully", ticket);
                return StatusCode(successResponse.StatusCode.Value, successResponse);
            }
            catch (KeyNotFoundException ex)
            {
                _log.LogWarning(ex.Message);
                var response = ResponseHandler<string>.NotFoundResponse("Ticket not found.", "The Ticket with the specified ID does not exist.");
                return StatusCode(response.StatusCode.Value, response);
            }
            catch (InvalidOperationException ex)
            {
                _log.LogWarning(ex.Message);
                var errorResponse = ResponseHandler<string>.ErrorResponse("Failed to save changes to the database.", ex);
                return StatusCode(errorResponse.StatusCode.Value, errorResponse);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                var errorResponse = ResponseHandler<string>.ErrorResponse("An internal server error occurred", ex);
                return StatusCode(errorResponse.StatusCode.Value, errorResponse);
            }
        }
    }
}
