using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Projects.Commands.CreateDiagramCommand
{
    public record CreateDiagramCommand(string name, Guid projectId) : IRequest<Unit>
    {
    }
}
