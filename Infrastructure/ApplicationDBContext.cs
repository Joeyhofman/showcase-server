using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure
{
    public class ApplicationDBContext : IdentityDbContext<Domain.Entities.User, IdentityRole<Guid>, Guid>
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        public DbSet<DataflowDiagram> DataflowDiagrams { get; set; }
        public DbSet<DataflowAssociation> DataflowAssociations { get; set; }
        public DbSet<DataflowPoint> DataflowPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
