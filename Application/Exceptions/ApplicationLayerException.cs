using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{

	[Serializable]
	public class ApplicationLayerException : Exception
	{
		public ApplicationLayerException() { }
		public ApplicationLayerException(string message) : base(message) { }
		public ApplicationLayerException(string message, Exception inner) : base(message, inner) { }
		protected ApplicationLayerException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
