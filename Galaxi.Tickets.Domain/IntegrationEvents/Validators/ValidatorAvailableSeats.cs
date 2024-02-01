using FluentValidation;
using Galaxi.Bus.Message;
using Galaxi.Tickets.Domain.DTOs;
using MassTransit;

namespace Galaxi.Tickets.Domain.IntegrationEvents.Validators
{
    public class ValidatorAvailableSeats : AbstractValidator<TicketDto>
    {
        //
        private readonly IRequestClient<CheckFunctionSeats> _client;
        private FunctionStatusSeats? _status;

        public ValidatorAvailableSeats(IRequestClient<CheckFunctionSeats> client)
        {
            _client = client;

            RuleFor(x =>x.FunctionId).NotEmpty().MustAsync(Exists)
           .WithMessage("This function doesn't exist");
            RuleFor(x => x.NumSeats).NotEmpty().Must(BeAvailable)
                .WithMessage("there are not enough seats available");
        }
        private async Task<bool> Exists(int functionId, CancellationToken cancellationToken)
        {
            var response = await _client.GetResponse<FunctionStatusSeats>(new CheckFunctionSeats
            {
                FunctionId = functionId
            }, cancellationToken);
            _status = response.Message;
            return _status.Exist;
        }
        private bool BeAvailable(int seat)
        {
            return _status.NumSeatAvailable >= seat ;
        }
    }
}
