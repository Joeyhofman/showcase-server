using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Projects.Queries.GetAllDiagramsForProjectQuery
{
    public record GetDiagramsForProjectQuery(Guid projectId) : IRequest<ICollection<DataflowDiagram>>
    {
    }
}
