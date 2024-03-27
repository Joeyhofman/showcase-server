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

namespace Application.Projects.Commands.CreateDiagramCommand
{
    public class CreateDiagramCommandHandler : IRequestHandler<CreateDiagramCommand, Unit>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDataflowDiagramRepository _diagramRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDiagramCommandHandler(IProjectRepository pojectRepository, IDataflowDiagramRepository diagramRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = pojectRepository;
            _diagramRepository = diagramRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateDiagramCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.projectId);
            if(project is null)
            {
               throw new ProjectNotFoundException("projct not found");
            }
            var diagram = new DataflowDiagram(Guid.NewGuid(), request.name, new List<DataflowPoint>(), new List<DataflowAssociation>());

            await _diagramRepository.Add(diagram);

            project.Diagrams.Add(diagram);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
