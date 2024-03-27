using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Projects.Commands.DeleteProjectCommand
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Guid>
    {
        private readonly IProjectRepository _repository;

        public DeleteProjectCommandHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public Task<Guid> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            _repository.Remove(request.Id);

            return Task.Run(() => request.Id);
        }
    }
}
