using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataTransferObjects.Entities;
using Domain.Entities;
using Domain.Exceptions.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Queries.GetUserByEmailQuery
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, User>
    {
        private UserManager<User> _userManager;

        public GetUserByEmailQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.email);
            if (user == null)
            {
                throw new UserNotFoundException("no user found with email");
            }

            return user;
        }
    }
}
