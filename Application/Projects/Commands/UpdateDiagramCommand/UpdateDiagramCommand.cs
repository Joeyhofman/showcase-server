using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Projects.Commands.UpdateDiagramCommand
{
    public record UpdateDiagramCommand(Guid UserId, Guid ProjectId, DataflowDiagram Diagram) : IRequest<Unit>
    {
    }
}
