using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataTransferObjects.Entities;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Queries.GetAllUsersQuery
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ICollection<UserDTO>>
    {

        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ICollection<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users =await  _userRepository.GetAllAsync();

            return UserDTO.FromUserList(users);
        }
    }
}
