using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataTransferObjects.Entities;
using Domain.Entities;
using MediatR;

namespace Application.Users.Queries.GetUserByEmailQuery
{
    public record GetUserByEmailQuery(string email) : IRequest<User>
    {
    }
}
