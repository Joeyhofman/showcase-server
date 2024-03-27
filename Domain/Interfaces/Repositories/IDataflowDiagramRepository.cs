using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IDataflowDiagramRepository
    {
        public Task<ICollection<DataflowDiagram>> GetAll();
        public Task<DataflowDiagram> GetById(Guid id);
        public Task Delete(Guid Id);
        public Task Add(DataflowDiagram diagram);
    }
}
