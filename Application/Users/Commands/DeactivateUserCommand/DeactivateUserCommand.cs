using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Users.Commands.DeactivateUserCommand
{
    public record DeactivateUserCommand(Guid userId) : IRequest<Unit>
    {
    }
}
