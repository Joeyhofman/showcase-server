using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Projects.Queries.GetAllDiagramsForProjectQuery
{
    public class GetAllDiagramsForProjectQueryHandler : IRequestHandler<GetDiagramsForProjectQuery, ICollection<DataflowDiagram>>
    {
        private readonly IProjectRepository _projectRepository;

        public GetAllDiagramsForProjectQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ICollection<DataflowDiagram>> Handle(GetDiagramsForProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.projectId);
            return project.Diagrams;
        }
    }
}
