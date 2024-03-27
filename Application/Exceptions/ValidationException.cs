using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Application.Exceptions
{
    public class ValidationException : DomainException
    {

        public Dictionary<string, string[]> Errors { get; set; }

        public ValidationException(Dictionary<string, string[]> errors) : base("a validation error occurred")
        {
            Errors = errors;
        }
    }
}
