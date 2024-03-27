using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DataflowAssociation
    {
        public Guid Id { get; set; }
        public const string TYPE = "DataflowAssociation";
        public DataflowPoint P1 { get; set; }
        public DataflowPoint P2{ get; set; }
    }
}
