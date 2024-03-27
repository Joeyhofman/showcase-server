using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enumerations;
using Domain.Interfaces.Repositories;

namespace Application.DataTransferObjects.Entities
{
    public class InvitationDTO
    {
        public Guid Id { get; set; }
        public UserDTO MemberToInvite { get; set; }
        public Project Project { get; set; }
        public InvitationStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }


        public static async Task<InvitationDTO> FromInvitation(Invitation invitation, IUserRepository userRepository, IProjectRepository projectRepository)
        {

            var dto = new InvitationDTO
            {
                Id = invitation.Id,
                MemberToInvite = UserDTO.FromUser(await userRepository.getByIdAsync(invitation.MemberToInviteId)),
                Project = await projectRepository.GetByIdAsync(invitation.ProjectId),
                Status = invitation.Status,
                CreatedOn = invitation.CreatedOn
            };

            return dto;
        }

        public static async Task<List<InvitationDTO>> FromInvitationList(ICollection<Invitation> invitations, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            List<InvitationDTO> dtolist = new List<InvitationDTO>();

            foreach (var invite in invitations)
            {
                var dto = await FromInvitation(invite, userRepository, projectRepository);
                dtolist.Add(dto);
            }

            return dtolist;
        }
    }

}
