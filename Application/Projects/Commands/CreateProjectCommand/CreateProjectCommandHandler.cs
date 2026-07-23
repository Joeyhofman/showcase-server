using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Projects.Commands.CreateProjectCommand
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
    {
        private readonly IProjectRepository _repostiroy;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CreateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork, ILogger<CreateProjectCommandHandler> logger)
        {
            _repostiroy = projectRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
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
            _logger.LogInformation(
                "Project created. ProjectId {ProjectId} by User {UserId}",
                project.Id,
                request.owner.Id);

            return project;
        }
    }
}
