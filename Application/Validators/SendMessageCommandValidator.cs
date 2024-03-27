using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ContactForm.Commands.SendMessage;
using FluentValidation;

namespace Application.Validators
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {

        public SendMessageCommandValidator()
        {

                RuleFor(c => c.FirstName)
                    .NotEmpty().When(x => string.IsNullOrWhiteSpace(x.FirstName)).WithMessage("uw voornaam mag niet leeg zijn.")
                    .MaximumLength(60).WithMessage("Uw voornaam mag niet langer zijn dan 60 tekens.");


            RuleFor(c => c.LastName)
                .NotEmpty().When(x => string.IsNullOrWhiteSpace(x.LastName)).WithMessage("Uw achternaam mag niet leeg zijn.")
                .MaximumLength(60).WithMessage("Uw achternaam mag niet langer zijn dan 60 tekens.");


            RuleFor(c => c.PhoneNumber)
                .NotEmpty().When(x => string.IsNullOrWhiteSpace(x.PhoneNumber)).WithMessage("Uw telefoonnummer mag niet leeg zijn.")
                .MinimumLength(10).WithMessage("Uw telefoonnummer moet minimaal 10 cijfers bevatten.")
                .Matches(@"^(\+\d{8,14}|\d{8,14})$").WithMessage("Uw telefoonnummer is omgeldig.")
                .MaximumLength(12).WithMessage("Uw telefoonnummer mag niet langer zijnd an 12 cijfers.");


            RuleFor(c => c.Email)
                .NotEmpty().When(x => string.IsNullOrWhiteSpace(x.Email)).WithMessage("Uw emailadres mag niet leeg zijn.")
                .EmailAddress().WithMessage("Geef een geldige emailadres op.")
                .MaximumLength(80).WithMessage("Uw emailadres mag niet langer zijn dan 80 tekens.");
            
            RuleFor(c => c.Subject)
                .NotEmpty().When(x => string.IsNullOrWhiteSpace(x.Subject)).WithMessage("Uw onderwerp mag niet leeg zijn.")
                .MaximumLength(200).WithMessage("Uw onderwerp mag niet langer zijn dan 200 tekens");

            RuleFor(c => c.Message)
                .NotEmpty().When(x => string.IsNullOrWhiteSpace(x.Message)).WithMessage("Uw bericht mag niet leeg zijn.")
                .MaximumLength(600).WithMessage("Uw bericht mag niet langer zijn dan 600 tekens.");
        }
    }
}
