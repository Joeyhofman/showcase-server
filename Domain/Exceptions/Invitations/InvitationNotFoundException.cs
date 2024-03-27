using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Invitations
{
    public class InvitationNotFoundException : DomainException
    {
        public InvitationNotFoundException(string message) : base(message)
        {
            
        }
    }
}
