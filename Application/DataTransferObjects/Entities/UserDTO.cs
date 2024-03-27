using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.DataTransferObjects.Entities
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public static UserDTO FromUser(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email!,
            };
        }

        public static ICollection<UserDTO> FromUserList(ICollection<User> users)
        {
            List<UserDTO> dtoList = users.Select(user => new UserDTO
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email!,
            }).ToList();

            return dtoList;
        }
    }
}
