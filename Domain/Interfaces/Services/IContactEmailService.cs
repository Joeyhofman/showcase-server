using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IContactEmailService
    {
        public Task SendMessage(string firstname, string lastname, string phonenumber, string email, string subject, string message);
    }
}
