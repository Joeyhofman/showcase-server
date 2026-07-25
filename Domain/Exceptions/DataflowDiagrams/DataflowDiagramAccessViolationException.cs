using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.DataflowDiagrams
{
    public class DataflowDiagramAccessViolationException : DomainException
    {
        public DataflowDiagramAccessViolationException(string message) : base(message)
        {
            
        }
    }
}
