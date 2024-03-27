using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions.DataflowDiagrams;
using Domain.Exceptions.Projects;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Projects.Queries.GetDiagramQuery
{
    public class GetDiagramQueryHandler : IRequestHandler<GetDiagramQuery, DataflowDiagram>
    {

        private readonly IDataflowDiagramRepository _dataflowDiagramRepository;
        private readonly IProjectRepository _projectRepository;

        public GetDiagramQueryHandler(IDataflowDiagramRepository dataflowDiagramRepository, IProjectRepository projectRepository)
        {
            _dataflowDiagramRepository = dataflowDiagramRepository;
            _projectRepository = projectRepository;
        }

        public async Task<DataflowDiagram> Handle(GetDiagramQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                throw new ProjectNotFoundException("could not find project");
            }

            if (!project.IsMember(request.UserId))
            {
                throw new UserIsNotMemberOfProjectException("User is not member of the proejct");
            }

            var diagram = await _dataflowDiagramRepository.GetById(request.DiagramId);
            if(diagram == null)
            {
                throw new DataflowDiagramNotFoundException("diagram not found");
            }


            return diagram;
        }
    }
}
