using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions.Users;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Commands.DeactivateUserCommand
{
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var userToUpdate = await _userRepository.getByIdAsync(request.userId);

            if (userToUpdate == null)
            {
                throw new UserNotFoundException("could not find user");
            }

            userToUpdate.LockoutEnabled = !userToUpdate.LockoutEnabled;

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
