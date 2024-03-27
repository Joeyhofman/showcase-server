using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IInvitationRepository
    {
        public void Add(Invitation invitation);
        public Task<Invitation> GetByIdAsync(Guid invitationId);
        public Task<ICollection<Invitation>> GetByProjectId(Guid projectId);
        public Task<ICollection<Invitation>> GetByUserId(Guid userId);
        public Task DeleteById(Guid invitationId);
    }
}
