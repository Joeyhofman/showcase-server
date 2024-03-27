using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces.Services;
using Infrastructure.Services.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Security.Application;

namespace Application.ContactForm.Commands.SendMessage
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Domain.Entities.ContactForm>
    {
        private readonly IContactEmailService _contactEmailService;

        public SendMessageCommandHandler(IContactEmailService emailService)
        {
            _contactEmailService = emailService;
        }

        public async Task<Domain.Entities.ContactForm> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contactFormModel = new Domain.Entities.ContactForm(
                    Guid.NewGuid(),
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.PhoneNumber,
                    request.Subject,
                    request.Message
                 );
           
                await _contactEmailService.SendMessage(
                   contactFormModel.FirstName,
                   contactFormModel.LastName,
                   contactFormModel.PhoneNumber,
                   contactFormModel.Email,
                   contactFormModel.Subject,
                   contactFormModel.Message);

                return contactFormModel;
            }
            catch (CouldNotSendEmailException cnsee)
            {
                await Console.Out.WriteLineAsync(cnsee.Message);
                return null;
            }
        }
    }
}
