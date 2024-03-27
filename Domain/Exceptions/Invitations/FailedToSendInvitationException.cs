using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Invitations
{
    public class FailedToSendInvitationException : DomainException
    {
        public FailedToSendInvitationException(string message) : base(message)
        {
            
        }
    }
}
