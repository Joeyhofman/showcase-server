using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataTransferObjects.Entities;
using Domain.Entities;
using MediatR;

namespace Application.Projects.Queries.GetInivtationsForProjectQuery
{
    public record GetInivtationForProjectQuery(Guid projectId) : IRequest<ICollection<InvitationDTO>>
    {
    }
}
