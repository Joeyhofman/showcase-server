using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions.Projects;

namespace Domain.Entities
{
    public class Project
    {
        public Guid Id { get; private set; }
        public User Owner { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ICollection<User> ProjectMembers { get; private set; }
        public ICollection<Invitation> Invitations { get; private set; }
        public ICollection<DataflowDiagram> Diagrams { get; set; }


        public Project()
        {
            ProjectMembers = new List<User>();
        }

        public Project(Guid id, User owner, string name, string description, ICollection<User> projectMembers)
        {
            Id = id;
            Name = name;
            Description = description;
            Owner = owner;
            ProjectMembers = projectMembers ?? new List<User>();
        }

        public void AddDiagram(DataflowDiagram diagram)
        {
            Diagrams.Add(diagram);
        }

        public void AddMember(User projectMember)
        {
            if(projectMember is null)
            {
                throw new ProjectMemberNullException("Project memeber cannot be null");
            }

            ProjectMembers.Add(projectMember);
        }

        public bool IsMember(Guid userID)
        {
            var isMember = ProjectMembers.Any(u => u.Id == userID);
            var isOwner = (userID == Owner.Id);
            return (!isMember || !isOwner);
        }
        
        public void RemoveMember(User projectMember)
        {
            if (projectMember is null)
            {
                throw new ProjectMemberNullException("Project memeber cannot be null");
            }

            ProjectMembers.Remove(projectMember);
        }

        public Invitation SendInvitation(User userToInvite)
        {

            if(userToInvite.Id == Owner.Id)
            {
                throw new SelfInvitationException("You can't invite yourself to the project");
            }

            var invitation = new Invitation(Guid.NewGuid(), userToInvite, this);
            Invitations.Add(invitation);
            return invitation;
        }
    }
}
