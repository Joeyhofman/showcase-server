using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Projects.Commands.CreateProjectCommand
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
    {
        private readonly IProjectRepository _repostiroy;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _repostiroy = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project(
                Guid.NewGuid(),
                request.owner,
                request.name,
                request.description,
                new List<User>()
            );

            _repostiroy.Add(project);
            await _unitOfWork.SaveChangesAsync();

            return project;
        }
    }
}
