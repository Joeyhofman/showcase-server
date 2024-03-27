using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Projects.Commands.DeleteProjectCommand
{
    public record DeleteProjectCommand(Guid Id) : IRequest<Guid>
    {
    }
}
