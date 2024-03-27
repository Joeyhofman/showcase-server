using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<User> getByIdAsync(Guid userId);
        public Task<ICollection<User>> GetAllAsync();
    }
}
