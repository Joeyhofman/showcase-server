using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Projects.Commands.DeleteInvitationCommand
{
    public record DeleteInvitationCommand(Guid userRequestingDeleteId, Guid projectId, Guid invitationToDeleteId) : IRequest<Unit>
    {
    }
}
