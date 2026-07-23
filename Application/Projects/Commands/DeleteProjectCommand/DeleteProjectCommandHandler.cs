using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Projects.Commands.DeleteProjectCommand
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Guid>
    {
        private readonly IProjectRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public DeleteProjectCommandHandler(IProjectRepository repository, IUserRepository userRepository, ILogger<DeleteProjectCommandHandler> logger)
        {
            _repository = repository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Guid> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.getByIdAsync(request.UserId);
            if (user is null)
            {
                throw new Exception($"User with ID {request.UserId} not found.");
            }


            var project = await _repository.GetByIdAsync(request.Id);
            if (project is null)
            {
                throw new Exception($"Project with ID {request.Id} not found.");
            }


            if (!project.Owner.Id.Equals(request.UserId))
            {
                   _logger.LogWarning(
                    "Unauthorized project deletion attempt. ProjectId {ProjectId} UserId {UserId}",
                    request.Id,
                    request.UserId);
                throw new Exception($"User with ID {request.UserId} does not have permission to delete project with ID {request.Id}.");
            }

            _repository.Remove(request.Id);
            _logger.LogInformation("Project deleted. ProjectId {ProjectId}", request.Id);

            return await Task.FromResult(request.Id);
        }
    }
}
