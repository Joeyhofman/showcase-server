using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumerations;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Projects.Commands.RejectInvitationCommand
{
    public class RejectInvitationCommandHandler : IRequestHandler<RejectInvitationCommand, Unit>
    {
        private readonly IInvitationRepository _inviationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RejectInvitationCommandHandler(IInvitationRepository invitationRepository, IUserRepository userRepository, IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _inviationRepository = invitationRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RejectInvitationCommand request, CancellationToken cancellationToken)
        {
            var invitation = await _inviationRepository.GetByIdAsync(request.InvitationId);

            if (invitation == null || invitation.Status != InvitationStatus.PENDING)
            {
                return Unit.Value;
            }

            var user = await _userRepository.getByIdAsync(invitation.MemberToInviteId);

            var project = await _projectRepository.GetByIdAsync(invitation.ProjectId);

            if (user is null || project is null)
            {
                return Unit.Value;
            }

            var expired = invitation.CreatedOn > DateTime.UtcNow.AddDays(3);

            if (expired)
            {
                invitation.Expire();
                await _unitOfWork.SaveChangesAsync();
                return Unit.Value;
            }

            invitation.Reject();
            project.RemoveMember(user);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
