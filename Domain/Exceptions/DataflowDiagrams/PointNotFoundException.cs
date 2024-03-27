using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.DataflowDiagrams
{
    public class PointNotFoundException : DomainException
    {

        public PointNotFoundException(string message) : base(message)
        {
            
        }
    }
}
