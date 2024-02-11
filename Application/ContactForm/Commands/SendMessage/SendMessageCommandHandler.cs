using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.ContactForm.Commands.SendMessage
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Domain.Entities.ContactForm>
    {
        private readonly IContactFormRepository _contactFormRepository;

        public SendMessageCommandHandler(IContactFormRepository repository)
        {
            _contactFormRepository = repository;
        }

        public async Task<Domain.Entities.ContactForm> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var contactFormModel = new Domain.Entities.ContactForm(new Guid(), request.Email, request.Message);
            return await _contactFormRepository.CreateAsync(contactFormModel);
        }
    }
}
