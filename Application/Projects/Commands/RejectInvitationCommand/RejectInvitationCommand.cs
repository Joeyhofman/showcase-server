using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumerations;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Projects.Commands.RejectInvitationCommand
{
    public record RejectInvitationCommand(Guid UserId, Guid InvitationId) : IRequest<Unit>
    {
    }
}
