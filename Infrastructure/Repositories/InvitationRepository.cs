using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly ApplicationDBContext _context;

        public InvitationRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public void Add(Invitation invitation)
        {
            _context.Invitations.Add(invitation);
        }

        public async Task<Invitation> GetByIdAsync(Guid invitationId)
        {
            var invitation = await _context.Invitations.FindAsync(invitationId);
            return invitation;
        }

        public async Task<ICollection<Invitation>> GetByProjectId(Guid projectId)
        {
            var invitations = await _context.Invitations.Where(i => i.ProjectId == projectId).ToListAsync();
            return invitations;
        }

        public async Task<ICollection<Invitation>> GetByUserId(Guid userId)
        {
            var invitations = await _context.Invitations.Where(i => i.MemberToInviteId == userId).ToListAsync();
            return invitations;
        }

        public async Task DeleteById(Guid invitationId)
        {
            await _context.Invitations.Where(i => i.Id == invitationId).ExecuteDeleteAsync();
        }
    }
}
