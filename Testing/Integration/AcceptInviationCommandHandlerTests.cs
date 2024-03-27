using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Projects.Commands.AcceptInvitationCommand;
using Application.Projects.Commands.SendInvitationCommand;
using Domain.Entities;
using Domain.Enumerations;
using Domain.Exceptions.Invitations;
using Domain.Exceptions.Projects;
using Domain.Interfaces.Repositories;
using Moq;

namespace Testing.Integration
{
    public class AcceptInviationCommandHandlerTests
    {

        private Mock<IProjectRepository> _projectRepositoryMock;
        private Mock<IInvitationRepository> _invitationRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        public AcceptInviationCommandHandlerTests()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _invitationRepositoryMock = new Mock<IInvitationRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Test_InvitationIsExpired_ShouldExpireInvitation()
        {
            // Arrange
            var invitationId = Guid.NewGuid();

            var project = new Project(Guid.NewGuid(), new User(), "test", "test", new List<User>());

            var userToInvite = new User
            {
                Id = Guid.NewGuid()
            };

            var invitation = new Invitation(invitationId, userToInvite, project, InvitationStatus.PENDING, DateTime.UtcNow.AddDays(5));
            
            _invitationRepositoryMock.Setup(r => r.GetByIdAsync(invitationId)).ReturnsAsync(invitation);
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
            _userRepositoryMock.Setup(r => r.getByIdAsync(userToInvite.Id)).ReturnsAsync(userToInvite);

            var handler = new AcceptInvitationCommandHandler(_invitationRepositoryMock.Object, _userRepositoryMock.Object, _projectRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new AcceptInvitationCommand(invitationId);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _invitationRepositoryMock.Verify(r => r.GetByIdAsync(invitationId), Times.Once);
            Assert.Equal(InvitationStatus.EXPIRED, invitation.Status);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Test_InvitationAccepted_ShouldAddUserToProjectMembers()
        {
            // Arrange
            var invitationId = Guid.NewGuid();
            var user = new User();
            var project = new Project();
            var invitation = new Invitation(invitationId, user, project);

            _invitationRepositoryMock = new Mock<IInvitationRepository>();
            _invitationRepositoryMock.Setup(r => r.GetByIdAsync(invitationId)).ReturnsAsync(invitation);

            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock.Setup(r => r.getByIdAsync(user.Id)).ReturnsAsync(user);

            _projectRepositoryMock = new Mock<IProjectRepository>();
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);

            var handler = new AcceptInvitationCommandHandler(_invitationRepositoryMock.Object, _userRepositoryMock.Object, _projectRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new AcceptInvitationCommand(invitationId);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _invitationRepositoryMock.Verify(r => r.GetByIdAsync(invitationId), Times.Once);
            _userRepositoryMock.Verify(r => r.getByIdAsync(user.Id), Times.Once);
            _projectRepositoryMock.Verify(r => r.GetByIdAsync(project.Id), Times.Once);
            Assert.Equal(InvitationStatus.ACCEPTED, invitation.Status);
            Assert.Contains(user, project.ProjectMembers);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Test_InvitationNotFound_ShouldThrowInvitationNotFoundException()
        {
            // Arrange
            var invitationId = Guid.NewGuid();

            _invitationRepositoryMock.Setup(r => r.GetByIdAsync(invitationId)).ReturnsAsync((Invitation)null);

            var handler = new AcceptInvitationCommandHandler(_invitationRepositoryMock.Object, _userRepositoryMock.Object, _projectRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new AcceptInvitationCommand(invitationId);

            await Assert.ThrowsAsync<InvitationNotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Test_InvitationAlreadyAccepted_ShouldThrowNotFoundException()
        {
            // Arrange
            var invitationId = Guid.NewGuid();
            var invitation = new Invitation(invitationId, new User(), new Project(), InvitationStatus.ACCEPTED);

            var invitationRepositoryMock = new Mock<IInvitationRepository>();
            invitationRepositoryMock.Setup(r => r.GetByIdAsync(invitationId)).ReturnsAsync(invitation);

            _userRepositoryMock = new Mock<IUserRepository>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var handler = new AcceptInvitationCommandHandler(invitationRepositoryMock.Object, _userRepositoryMock.Object, _projectRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new AcceptInvitationCommand(invitationId);


            //act & Assert
            await Assert.ThrowsAsync<InvitationNotFoundException>(() => handler.Handle(command, CancellationToken.None));
            _invitationRepositoryMock.Verify(r => r.GetByIdAsync(invitationId), Times.Never);
            _userRepositoryMock.VerifyNoOtherCalls();
            _projectRepositoryMock.VerifyNoOtherCalls();
            _unitOfWorkMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Test_ExpiredInvitation_ShouldExpireInvitationAndNotAddUserToProjectMembers()
        {
            // Arrange
            var invitationId = Guid.NewGuid();
            var user = new User();
            var project = new Project();
            var invitation = new Invitation(invitationId, user, project, InvitationStatus.PENDING, DateTime.UtcNow.AddDays(4));

            _invitationRepositoryMock = new Mock<IInvitationRepository>();
            _invitationRepositoryMock.Setup(r => r.GetByIdAsync(invitationId)).ReturnsAsync(invitation);

            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock.Setup(r => r.getByIdAsync(user.Id)).ReturnsAsync(user);

            _projectRepositoryMock = new Mock<IProjectRepository>();
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);

            var handler = new AcceptInvitationCommandHandler(_invitationRepositoryMock.Object, _userRepositoryMock.Object, _projectRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new AcceptInvitationCommand(invitationId);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _invitationRepositoryMock.Verify(r => r.GetByIdAsync(invitationId), Times.Once);
            _userRepositoryMock.Verify(r => r.getByIdAsync(user.Id), Times.Once);
            _projectRepositoryMock.Verify(r => r.GetByIdAsync(project.Id), Times.Once);
            Assert.Equal(InvitationStatus.EXPIRED, invitation.Status);
            Assert.DoesNotContain(user, project.ProjectMembers);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Test_ValidInvitation_ShouldAcceptInvitationAndAddUserToProjectMembers()
        {
            // Arrange
            var invitationId = Guid.NewGuid();
            var user = new User();
            var project = new Project();
            var invitation = new Invitation(invitationId, user, project, InvitationStatus.PENDING, DateTime.UtcNow.AddDays(-4));

            _invitationRepositoryMock = new Mock<IInvitationRepository>();
            _invitationRepositoryMock.Setup(r => r.GetByIdAsync(invitationId)).ReturnsAsync(invitation);

            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock.Setup(r => r.getByIdAsync(user.Id)).ReturnsAsync(user);

            _projectRepositoryMock = new Mock<IProjectRepository>();
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);

            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var handler = new AcceptInvitationCommandHandler(_invitationRepositoryMock.Object, _userRepositoryMock.Object, _projectRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new AcceptInvitationCommand(invitationId);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _invitationRepositoryMock.Verify(r => r.GetByIdAsync(invitationId), Times.Once);
            _userRepositoryMock.Verify(r => r.getByIdAsync(user.Id), Times.Once);
            _projectRepositoryMock.Verify(r => r.GetByIdAsync(project.Id), Times.Once);
            Assert.Equal(InvitationStatus.ACCEPTED, invitation.Status);
            Assert.Contains(user, project.ProjectMembers);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
