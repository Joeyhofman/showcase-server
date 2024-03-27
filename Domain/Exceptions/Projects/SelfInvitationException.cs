using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Projects
{
    public class SelfInvitationException : DomainException
    {
        public SelfInvitationException(string message) : base(message)
        {
            
        }
    }
}
