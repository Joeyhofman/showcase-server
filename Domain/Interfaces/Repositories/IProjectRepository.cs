using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        public void Add(Project project);
        public void Remove(Guid projectId);
        public Task<ICollection<Project>> GetByUserAsync(User user);
        public Task<Project> GetByIdAsync(Guid projectId);
    }
}
