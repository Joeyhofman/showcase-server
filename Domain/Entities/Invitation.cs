using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumerations;

namespace Domain.Entities
{
    public class Invitation
    {
        public Guid Id { get; private set; }
        public Guid MemberToInviteId { get; private set; }
        public Guid ProjectId { get; private set; }
        public InvitationStatus Status { get; private set; }
        public DateTime CreatedOn { get; private set; }

        private const int DAY_UNTIL_EXPIRATION = 3;


        public Invitation()
        {
        }

        public Invitation(Guid id, User memberToInvite, Project project)
        {
            Id = id;
            MemberToInviteId = memberToInvite.Id;
            ProjectId = project.Id;
            Status = InvitationStatus.PENDING;
            CreatedOn = DateTime.UtcNow;
        }

        public Invitation(Guid id, User memberToInvite, Project project, InvitationStatus status)
        {
            Id = id;
            MemberToInviteId = memberToInvite.Id;
            ProjectId = project.Id;
            Status = status;
            CreatedOn = DateTime.UtcNow;
        }

        public Invitation(Guid id, User memberToInvite, Project project, InvitationStatus status, DateTime createdOn)
        {
            Id = id;
            MemberToInviteId = memberToInvite.Id;
            ProjectId = project.Id;
            Status = status;
            CreatedOn = createdOn;
        }

        public void Expire()
        {
            Status = InvitationStatus.EXPIRED;
        }

        public void Reject()
        {
            Status = InvitationStatus.REJECTED;
        }

        public void Accept()
        {
            Status = InvitationStatus.ACCEPTED;
        }
    }
}
