using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Projects
{
    public class UserDoesNotOwnProjectException : DomainException
    {
        public UserDoesNotOwnProjectException(string message) : base(message)
        {
            
        }
    }
}
