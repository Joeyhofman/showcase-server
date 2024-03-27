using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Users
{
    public class UserNotFoundException : DomainException
    {

        public UserNotFoundException(string message) : base (message)
        {
            
        }
    }
}
