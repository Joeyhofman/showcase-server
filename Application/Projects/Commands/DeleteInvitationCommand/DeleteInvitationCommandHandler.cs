using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions.Projects;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Projects.Commands.DeleteInvitationCommand
{
    public class DeleteInvitationCommandHandler : IRequestHandler<DeleteInvitationCommand, Unit>
    {
        private readonly IInvitationRepository _repository;
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger<DeleteInvitationCommandHandler> _logger;

        public DeleteInvitationCommandHandler(IInvitationRepository repository, IProjectRepository projectRepository, ILogger<DeleteInvitationCommandHandler> logger)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteInvitationCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.projectId);
            if(project.Owner.Id != request.userRequestingDeleteId)
            {
                _logger.LogWarning(
                    "Unauthorized invitation deletion attempt. User {UserId} attempted to delete invitation {InvitationId} from project {ProjectId}",
                    request.userRequestingDeleteId,
                    request.invitationToDeleteId,
                    request.projectId
                );
                throw new UserDoesNotOwnProjectException("user is not owner of the project");
            }

            await _repository.DeleteById(request.invitationToDeleteId);

            _logger.LogInformation(
                "Invitation {InvitationId} successfully deleted from project {ProjectId} by user {UserId}",
                request.invitationToDeleteId,
                request.projectId,
                request.userRequestingDeleteId
            );

            return Unit.Value;
        }
    }
}
