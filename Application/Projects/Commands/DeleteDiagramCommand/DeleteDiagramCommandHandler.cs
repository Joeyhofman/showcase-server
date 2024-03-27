using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions.DataflowDiagrams;
using Domain.Exceptions.Projects;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Projects.Commands.DeleteDiagramCommand
{
    public class DeleteDiagramCommandHandler : IRequestHandler<DeleteDiagramCommand, Unit>
    {
        private readonly IDataflowDiagramRepository _dataflowRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDiagramCommandHandler(IDataflowDiagramRepository dataflowDiagramRepository, IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _dataflowRepository = dataflowDiagramRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteDiagramCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if(project is null)
            {
                throw new ProjectNotFoundException("project not found");
            }

            var diagram = await _dataflowRepository.GetById(request.DigramId);
            if(diagram is null)
            {
                throw new DataflowDiagramNotFoundException("daigram not found");
            }

            project.Diagrams.Remove(diagram);
            await _dataflowRepository.Delete(diagram.Id);

            return Unit.Value;
        }
    }
}
