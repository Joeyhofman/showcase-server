using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Projects
{
    public class ProjectMemberNullException : DomainException
    {
        public ProjectMemberNullException(string message) : base(message) { }
    }
}
