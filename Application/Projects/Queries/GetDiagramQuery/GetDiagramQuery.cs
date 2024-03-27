using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Projects.Queries.GetDiagramQuery
{
    public record GetDiagramQuery(Guid UserId, Guid DiagramId, Guid ProjectId) : IRequest<DataflowDiagram>
    {
    }
}
