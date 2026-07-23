using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions.Invitations;
using Domain.Interfaces.Repositories;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Projects.Commands.SendInvitationCommand
{
    public class SendInivtationCommandHandler : IRequestHandler<SendInvitationCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SendInivtationCommandHandler> _logger;

        public SendInivtationCommandHandler(IUserRepository userRepository, IProjectRepository projectRepository, IInvitationRepository invitationRepository, ILogger<SendInivtationCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _invitationRepository = invitationRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
        {
             _logger.LogInformation(
                "Send invitation started. ProjectId: {ProjectId}, InvitedUserId: {InvitedUserId}",
                request.projectId,
                request.userToInviteId);
            var userToInvite = await _userRepository.getByIdAsync(request.userToInviteId);

            var project = await _projectRepository.GetByIdAsync(request.projectId);


            var invitation = project.SendInvitation(userToInvite);
            
            if(invitation is null)
            {
                  _logger.LogWarning(
                    "Send invitation failed. Domain rejected invitation. ProjectId: {ProjectId}, UserId: {UserId}",
                    project.Id,
                    userToInvite.Id);
                throw new FailedToSendInvitationException("failed to create invitaion");
            }

            _invitationRepository.Add(invitation);
            await _unitOfWork.SaveChangesAsync();

             _logger.LogInformation(
                "Invitation sent successfully. InvitationId: {InvitationId}, ProjectId: {ProjectId}, InvitedUserId: {UserId}",
                invitation.Id,
                project.Id,
                userToInvite.Id);

            return Unit.Value;
        }
    }
}
