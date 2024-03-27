using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Projects
{
    public class UserIsNotMemberOfProjectException : DomainException
    {
        public UserIsNotMemberOfProjectException(string message) : base(message)
        {
            
        }
    }
}
