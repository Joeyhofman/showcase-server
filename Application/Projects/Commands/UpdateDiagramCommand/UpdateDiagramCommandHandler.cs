using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions.DataflowDiagrams;
using Domain.Exceptions.Projects;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Projects.Commands.UpdateDiagramCommand
{
    public class UpdateDiagramCommandHandler : IRequestHandler<UpdateDiagramCommand, Unit>
    {
        private readonly IDataflowDiagramRepository _dataflowDiagramRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDiagramCommandHandler(IDataflowDiagramRepository dataflowDiagramRepository, IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _dataflowDiagramRepository = dataflowDiagramRepository;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateDiagramCommand request, CancellationToken cancellationToken)
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

            var diagram = await _dataflowDiagramRepository.GetById(request.Diagram.Id);
            if (diagram == null)
            {
                throw new DataflowDiagramNotFoundException("diagram not found");
            }

            diagram.Name = request.Diagram.Name;
            diagram.SetPoints((List<Domain.Entities.DataflowPoint>)request.Diagram.Points);
            diagram.SetAssociations((List<Domain.Entities.DataflowAssociation>)request.Diagram.Associations);

            await _unitOfWork.SaveChangesAsync();


            return Unit.Value;
        }
    }
}
