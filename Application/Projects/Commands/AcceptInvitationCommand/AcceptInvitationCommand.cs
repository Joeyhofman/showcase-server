using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Projects.Commands.AcceptInvitationCommand
{
    public record AcceptInvitationCommand(Guid invitationToBeAcceptedId) : IRequest<Unit>
    {
    }
}
