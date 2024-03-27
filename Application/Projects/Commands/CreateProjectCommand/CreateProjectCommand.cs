using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Projects.Commands.CreateProjectCommand
{
    public record CreateProjectCommand(Guid id, User owner,  string name, string description) : IRequest<Project>
    {
    }
}
