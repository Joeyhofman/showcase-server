using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions.Users;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.UpdateUserCommand
{
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {

        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(IUserRepository userRepository, ILogger<UpdateUserCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Updating user {UserId}",
                request.id
            );
            var userToUpdate = await _userRepository.getByIdAsync(request.id);

            if(userToUpdate == null)
            {
                _logger.LogWarning(
                        "User update failed. User {UserId} was not found",
                        request.id
                    );
                throw new UserNotFoundException("could not find user");
            }

            userToUpdate.UserName = request.user.UserName;
            userToUpdate.AccessFailedCount = request.user.AccessFailedCount;
            userToUpdate.NormalizedUserName = request.user.NormalizedUserName;
            userToUpdate.PhoneNumberConfirmed = request.user.PhoneNumberConfirmed;
            userToUpdate.EmailConfirmed = request.user.EmailConfirmed;
            userToUpdate.SecurityStamp = request.user.SecurityStamp;
            userToUpdate.ConcurrencyStamp = request.user.ConcurrencyStamp;
            userToUpdate.LockoutEnabled = request.user.LockoutEnabled;
            userToUpdate.LockoutEnd = request.user.LockoutEnd;
            userToUpdate.NormalizedEmail = request.user.NormalizedEmail;
            userToUpdate.PhoneNumber = request.user.PhoneNumber;
            userToUpdate.TwoFactorEnabled = request.user.TwoFactorEnabled;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation(
                "User {UserId} successfully updated",
                request.id
            );


            return Unit.Value;
        }
    }
}
