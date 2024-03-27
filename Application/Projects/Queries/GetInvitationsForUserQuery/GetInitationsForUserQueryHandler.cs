using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataTransferObjects.Entities;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Projects.Queries.GetInvitationsForUserQuery
{
    public class GetInitationsForUserQueryHandler : IRequestHandler<GetInvitationsForUserQuery, ICollection<InvitationDTO>>
    {
        private readonly IInvitationRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public GetInitationsForUserQueryHandler(IInvitationRepository repository, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<ICollection<InvitationDTO>> Handle(GetInvitationsForUserQuery request, CancellationToken cancellationToken)
        {
            var inviations = await _repository.GetByUserId(request.userId);

            
            return await InvitationDTO.FromInvitationList(inviations, _userRepository, _projectRepository);
        }
    }
}
