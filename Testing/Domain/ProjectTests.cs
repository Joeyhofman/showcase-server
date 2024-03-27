using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions.Projects;
using Xunit.Sdk;

namespace Testing.Domain
{
    public class ProjectTests
    {

        public ProjectTests()
        {
            
        }

        [Fact]
        public void Inviting_Owner_Throws_SelfInvitationException()
        {

            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId
            };

            var projectId = Guid.NewGuid();
            var project = new Project(projectId, user, "test project", "lorem ipsum", new List<User>());

            Assert.Throws<SelfInvitationException>(() => project.SendInvitation(user));
        }

        [Fact]
        public void Inviting_Null_Member_Should_Throw_NullUserException()
        {

            var project = new Project(Guid.NewGuid(), new User(), "test project", "lorem ipsum", new List<User>());

            Assert.Throws<ProjectMemberNullException>(() => project.AddMember(null));
        }

        [Fact]
        public void Is_Memeber_Should_Be_True_If_Member_Is_Owner()
        {

            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId
            };

            var projectId = Guid.NewGuid();
            var project = new Project(projectId, user, "test project", "lorem ipsum", new List<User>());

            Assert.True(project.IsMember(user.Id));
        }

        [Fact]
        public void Is_Memeber_Should_Be_True_If_Member_Is_In_Member_List()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId
            };

            var ownerId = Guid.NewGuid();
            var owner = new User
            {
                Id = ownerId
            };

            var members = new List<User>
            {
                user
            };

            var project = new Project(Guid.NewGuid(), owner, "test project", "lorem ipsum", members);

            Assert.True(project.IsMember(userId));
        }
    }
}
