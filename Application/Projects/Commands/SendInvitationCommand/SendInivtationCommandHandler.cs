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

namespace Application.Projects.Commands.SendInvitationCommand
{
    public class SendInivtationCommandHandler : IRequestHandler<SendInvitationCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SendInivtationCommandHandler(IUserRepository userRepository, IProjectRepository projectRepository, IInvitationRepository invitationRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _invitationRepository = invitationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
        {
            var userToInvite = await _userRepository.getByIdAsync(request.userToInviteId);

            var project = await _projectRepository.GetByIdAsync(request.projectId);


            var invitation = project.SendInvitation(userToInvite);
            
            if(invitation is null)
            {
                throw new FailedToSendInvitationException("failed to create invitaion");
            }

            _invitationRepository.Add(invitation);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
