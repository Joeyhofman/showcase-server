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
    public class DataflowDiagramRepository : IDataflowDiagramRepository
    {
        private readonly ApplicationDBContext _context;

        public DataflowDiagramRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task Add(DataflowDiagram diagram)
        {
            await _context.AddAsync(diagram);
        }

        public async Task Delete(Guid Id)
        {
            await _context.DataflowDiagrams.Where(d => d.Id == Id).ExecuteDeleteAsync();
        }

        public async Task<ICollection<DataflowDiagram>> GetAll()
        {
            var diagrams = await _context.DataflowDiagrams.ToListAsync();
            return diagrams;
        }

        public async Task<DataflowDiagram> GetById(Guid id)
        {
            var diagram = await _context.DataflowDiagrams.Where(d => d.Id == id)
                .Include(d => d.Points)
                .Include(d => d.Associations)
                .FirstOrDefaultAsync();
            return diagram;
        }
    }
}
