using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions.Projects;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Projects.Commands.DeleteInvitationCommand
{
    public class DeleteInvitationCommandHandler : IRequestHandler<DeleteInvitationCommand, Unit>
    {
        private readonly IInvitationRepository _repository;
        private readonly IProjectRepository _projectRepository;

        public DeleteInvitationCommandHandler(IInvitationRepository repository, IProjectRepository projectRepository)
        {
            _repository = repository;
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(DeleteInvitationCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.projectId);
            if(project.Owner.Id != request.userRequestingDeleteId)
            {
                throw new UserDoesNotOwnProjectException("user is not owner of the project");
            }

            await _repository.DeleteById(request.invitationToDeleteId);

            return Unit.Value;
        }
    }
}
