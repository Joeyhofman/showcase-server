using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumerations;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Projects.Commands.RejectInvitationCommand
{
    public class RejectInvitationCommandHandler : IRequestHandler<RejectInvitationCommand, Unit>
    {
        private readonly IInvitationRepository _inviationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RejectInvitationCommandHandler> _logger;

        public RejectInvitationCommandHandler(IInvitationRepository invitationRepository, IUserRepository userRepository, IProjectRepository projectRepository, ILogger<RejectInvitationCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _inviationRepository = invitationRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(RejectInvitationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
            "Reject invitation started. InvitationId: {InvitationId}",
            request.InvitationId);
            var invitation = await _inviationRepository.GetByIdAsync(request.InvitationId);

            if (invitation == null || invitation.Status != InvitationStatus.PENDING)
            {
                 _logger.LogWarning(
                    "Reject invitation failed. Invitation not found. InvitationId: {InvitationId}",
                    request.InvitationId);
                return Unit.Value;
            }

            var user = await _userRepository.getByIdAsync(invitation.MemberToInviteId);

            var project = await _projectRepository.GetByIdAsync(invitation.ProjectId);

            if (user is null || project is null)
            {
                _logger.LogError(
                    "Reject invitation failed. Related entity missing. InvitationId: {InvitationId}, UserExists: {UserExists}, ProjectExists: {ProjectExists}",
                    invitation.Id,
                    user != null,
                    project != null);
                return Unit.Value;
            }

            var expired = invitation.CreatedOn > DateTime.UtcNow.AddDays(3);

            if (expired)
            {
                invitation.Expire();
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation(
                    "Expired invitation marked as expired during rejection attempt. InvitationId: {InvitationId}",
                    invitation.Id);
                return Unit.Value;
            }

            invitation.Reject();
            project.RemoveMember(user);
            await _unitOfWork.SaveChangesAsync();

             _logger.LogInformation(
                "Invitation rejected successfully. InvitationId: {InvitationId}, ProjectId: {ProjectId}, UserId: {UserId}",
                invitation.Id,
                project.Id,
                user.Id);

            return Unit.Value;
        }
    }
}
