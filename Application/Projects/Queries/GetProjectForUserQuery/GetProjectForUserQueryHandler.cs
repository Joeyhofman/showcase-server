using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Projects.Queries.GetProjectForUserQuery
{
    public class GetProjectForUserQueryHandler : IRequestHandler<GetProjectForUserQuery, ICollection<Project>>
    {
        private readonly IProjectRepository _repostiory;

        public GetProjectForUserQueryHandler(IProjectRepository repository)
        {
            _repostiory = repository;
        }

        public async Task<ICollection<Project>> Handle(GetProjectForUserQuery request, CancellationToken cancellationToken)
        {
            var proejcts = await _repostiory.GetByUserAsync(request.user);
            return proejcts;
        }
    }
}
