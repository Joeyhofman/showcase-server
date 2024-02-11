using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class ContactFormRepository : IContactFormRepository
    {
        public async Task<ContactForm> CreateAsync(ContactForm contactForm)
        {
            return await Task.FromResult(contactForm);
        }
    }
}
