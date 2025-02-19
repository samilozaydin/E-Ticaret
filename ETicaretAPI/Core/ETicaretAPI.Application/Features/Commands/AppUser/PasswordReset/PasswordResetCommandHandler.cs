using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Services.RabbitMQ;
using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.Events.EmailEvents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.PasswordReset
{
    public class PasswordResetCommandHandler : IRequestHandler<PasswordResetCommandRequest, PasswordResetCommandResponse>
    {
        readonly IAuthService _authService;
        readonly private IRabbitMQService _rabbitMQService;

        public PasswordResetCommandHandler(IRabbitMQService rabbitMQService, IAuthService authService)
        {
            _rabbitMQService = rabbitMQService;
            _authService = authService;
        }

        public async Task<PasswordResetCommandResponse> Handle(PasswordResetCommandRequest request, CancellationToken cancellationToken)
        {
            await _authService.PasswordResetAsync(request.Email);

            /*_rabbitMQService.SendMessageToExchange(RabbitMQConstants.UserExchangeName,
                RabbitMQConstants.DefaultExchangeType,
                RabbitMQConstants.UserPasswordResetEmailQueueName,
                new PasswordResetEvent { Email= request.Email});

            _rabbitMQService.Consume<PasswordResetEvent>(RabbitMQConstants.UserExchangeName,
                RabbitMQConstants.DefaultExchangeType,
                RabbitMQConstants.UserPasswordResetEmailQueueName,(model) =>
                {
                    _authService.PasswordResetAsync(model.Email);
                });*/
            return new();
        }
    }
}
