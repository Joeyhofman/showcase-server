using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Exceptions;



public class CouldNotSendEmailException : Exception
{
	public CouldNotSendEmailException() { }
	public CouldNotSendEmailException(string message) : base(message) { }
	public CouldNotSendEmailException(string message, Exception inner) : base(message, inner) { }
}
