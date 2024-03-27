using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Projects.Commands.CreateDiagramCommand;
using FluentValidation;

namespace Application.Validators
{
    public class CreateDiagramCommandValidator : AbstractValidator<CreateDiagramCommand>
    {
        public CreateDiagramCommandValidator()
        {
            RuleFor(c => c.name)
                .NotEmpty().WithMessage("De naam van de diagram mag niet leeg zijn.")
                .NotNull().WithMessage("De naam van de diagram mag niet leeg zijn.")
                .MaximumLength(60).WithMessage("De naam van de diagram mag niet langer zijn dan 60 tekens.");
        }
    }
}
