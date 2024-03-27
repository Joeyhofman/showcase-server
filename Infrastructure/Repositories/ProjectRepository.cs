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
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDBContext _context;

        public ProjectRepository(ApplicationDBContext context)
        {
            _context = context;

        }

        public void Add(Project project)
        {
            _context.Projects.Add(project);
        }

        public async Task<Project> GetByIdAsync(Guid projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Invitations)
                .Include(p => p.ProjectMembers)
                .Include(p => p.Diagrams)
                .FirstOrDefaultAsync(p => p.Id == projectId);
            return project;
        }

        public async Task<ICollection<Project>> GetByUserAsync(User user)
        {
            var proejcts = _context.Projects.Where(p => p.Owner == user || p.ProjectMembers.Contains(user)).ToList();
            return proejcts;
        }

        public void Remove(Guid projectId)
        {
            _context.Projects.Where(p => p.Id == projectId).ExecuteDelete();
        }
    }
}
