using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Projects.Commands.CreateProjectCommand;
using FluentValidation;

namespace Application.Validators
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(c => c.name)
                .NotEmpty().WithMessage("De naam van het project is verplicht.")
                .NotNull().WithMessage("De naam van het project is verplicht.")
                .MaximumLength(60).WithMessage("De naam van het project mag niet langer zijn dan 60 tekens.");

            RuleFor(c => c.description)
                .NotEmpty().WithMessage("De beschrijving van het project is verplicht.")
                .NotNull().WithMessage("De beschrijving van het project is verplicht.")
                .MaximumLength(200).WithMessage("De beschrijving van het project mag niet langer zijn dan 60 tekens.");
        }
    }
}
