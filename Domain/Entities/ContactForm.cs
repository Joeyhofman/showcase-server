using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ContactForm
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }

        public ContactForm(Guid id, string email, string message)
        {
            Id = id;
            Email = email;
            Message = message;
        }
    }
}
