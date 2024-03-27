using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.DataflowDiagrams
{
    public class DataflowDiagramNotFoundException : DomainException
    {
        public DataflowDiagramNotFoundException(string message) : base(message)
        {
            
        }
    }
}
