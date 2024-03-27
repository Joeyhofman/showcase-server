using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.ContactForm.Commands.SendMessage
{
    public record SendMessageCommand(string FirstName, string LastName, string Email, string PhoneNumber, string Subject, string Message) : IRequest<Domain.Entities.ContactForm>
    {
    }
}