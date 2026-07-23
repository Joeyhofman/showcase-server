using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Domain.Entities;
using Domain.Exceptions.Projects;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Projects.Commands.CreateDiagramCommand
{
    public class CreateDiagramCommandHandler : IRequestHandler<CreateDiagramCommand, Unit>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDataflowDiagramRepository _diagramRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateDiagramCommandHandler> _logger;

        public CreateDiagramCommandHandler(IProjectRepository pojectRepository, ILogger<CreateDiagramCommandHandler> logger, IDataflowDiagramRepository diagramRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = pojectRepository;
            _diagramRepository = diagramRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateDiagramCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Creating diagram {DiagramName} for project {ProjectId}",
                request.name,
                request.projectId
            );
            var project = await _projectRepository.GetByIdAsync(request.projectId);
            if(project is null)
            {
                _logger.LogWarning(
                    "Diagram creation failed. Project {ProjectId} not found",
                    request.projectId
                );
               throw new ProjectNotFoundException("projct not found");
            }
            var diagram = new DataflowDiagram(Guid.NewGuid(), request.name, new List<DataflowPoint>(), new List<DataflowAssociation>());

            await _diagramRepository.Add(diagram);

            project.Diagrams.Add(diagram);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation(
                "Diagram {DiagramId} successfully created in project {ProjectId}",
                diagram.Id,
                request.projectId
            );

            return Unit.Value;
        }
    }
}
