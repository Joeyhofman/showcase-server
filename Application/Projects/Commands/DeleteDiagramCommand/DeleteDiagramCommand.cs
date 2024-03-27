using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Projects.Commands.DeleteDiagramCommand
{
    public record DeleteDiagramCommand(Guid ProjectId, Guid DigramId, Guid userId) : IRequest<Unit>
    {
    }
}
