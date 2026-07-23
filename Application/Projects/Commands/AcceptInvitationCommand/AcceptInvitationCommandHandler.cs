using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Projects.Queries.GetInivtationsForProjectQuery;
using Domain.Enumerations;
using Domain.Exceptions.Invitations;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Projects.Commands.AcceptInvitationCommand
{
    public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Unit>
    {
        private readonly IInvitationRepository _inviationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger<AcceptInvitationCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AcceptInvitationCommandHandler(IInvitationRepository invitationRepository, IUserRepository userRepository, IProjectRepository projectRepository, ILogger<AcceptInvitationCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _inviationRepository = invitationRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
        {

            var invitation = await _inviationRepository.GetByIdAsync(request.invitationToBeAcceptedId);


            if (invitation == null  || invitation.Status != InvitationStatus.PENDING)
            {
                _logger.LogWarning(
                    "Accept invitation failed. Invitation not found. InvitationId: {InvitationId}",
                    request.invitationToBeAcceptedId);
                throw new InvitationNotFoundException("Can't find invitation.");
            }


            var user = await _userRepository.getByIdAsync(invitation.MemberToInviteId);

            var project = await  _projectRepository.GetByIdAsync(invitation.ProjectId);

            if(user is null || project is null)
            {
                return Unit.Value;
            }

            var expired = invitation.CreatedOn > DateTime.UtcNow.AddDays(3);

            if (expired)
            {
                invitation.Expire();
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation(
                    "Invitation expired during acceptance attempt. InvitationId: {InvitationId}",
                    invitation.Id);
                return Unit.Value;
            }

            invitation.Accept();
            project.AddMember(user);
            await _unitOfWork.SaveChangesAsync();

               _logger.LogInformation(
                    "Invitation accepted successfully. InvitationId: {InvitationId}, ProjectId: {ProjectId}, UserId: {UserId}",
                    invitation.Id,
                    project.Id,
                    user.Id);

            return Unit.Value;
        }
    }
}
