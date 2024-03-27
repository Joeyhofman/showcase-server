using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Projects.Commands.SendInvitationCommand
{
    public record SendInvitationCommand(Guid userToInviteId, Guid projectId) : IRequest<Unit>
    {
    }
}
