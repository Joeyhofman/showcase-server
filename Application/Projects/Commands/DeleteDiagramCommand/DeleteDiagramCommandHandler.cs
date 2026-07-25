using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions.DataflowDiagrams;
using Domain.Exceptions.Projects;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Projects.Commands.DeleteDiagramCommand
{
    public class DeleteDiagramCommandHandler : IRequestHandler<DeleteDiagramCommand, Unit>
    {
        private readonly IDataflowDiagramRepository _dataflowRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteDiagramCommandHandler> _logger;

        public DeleteDiagramCommandHandler(IDataflowDiagramRepository dataflowDiagramRepository, IProjectRepository projectRepository, ILogger<DeleteDiagramCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _dataflowRepository = dataflowDiagramRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDiagramCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if(project is null)
            {
                _logger.LogWarning(
                    "Diagram deletion failed. Project {ProjectId} not found",
                    request.ProjectId
                );
                throw new ProjectNotFoundException("project not found");
            }

            if (!project.IsMember(request.userId))
            {
                _logger.LogWarning(
                    "Diagram deletion denied. User {UserId} is not a member of project {ProjectId}.",
                    request.userId,
                    project.Id);

                throw new DataflowDiagramAccessViolationException("");
            }

            var diagram = await _dataflowRepository.GetById(request.DigramId);
            if(diagram is null)
            {
                _logger.LogWarning(
                    "Diagram deletion failed. Diagram {DiagramId} not found",
                    request.DigramId
                );
                throw new DataflowDiagramNotFoundException("daigram not found");
            }

            if(!project.Diagrams.Any(x => x.Id == request.DigramId))
            {
                _logger.LogWarning("Diagram deletion failed. Digram {DiagramId} not part of project.", request.DigramId);
                throw new DataflowDiagramNotFoundException("daigram not found");
            }

            project.Diagrams.Remove(diagram);
            await _dataflowRepository.Delete(diagram.Id);

            _logger.LogInformation(
                "Diagram {DiagramId} successfully deleted from project {ProjectId}",
                diagram.Id,
                request.ProjectId
            );

            return Unit.Value;
        }
    }
}
