using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataTransferObjects.Entities;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Projects.Queries.GetInivtationsForProjectQuery
{
    public class GetInitationsForProjectQueryHandler : IRequestHandler<GetInivtationForProjectQuery, ICollection<InvitationDTO>>
    {
        private readonly IInvitationRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;


        public GetInitationsForProjectQueryHandler(IInvitationRepository repository, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<ICollection<InvitationDTO>> Handle(GetInivtationForProjectQuery request, CancellationToken cancellationToken)
        {
            var invitations = await _repository.GetByProjectId(request.projectId);
            return await InvitationDTO.FromInvitationList(invitations, _userRepository, _projectRepository);
        }
    }
}
