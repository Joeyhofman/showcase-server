using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Projects
{
    public class ProjectNotFoundException : DomainException
    {

        public ProjectNotFoundException(string message) : base(message)
        {
            
        }
    }
}
