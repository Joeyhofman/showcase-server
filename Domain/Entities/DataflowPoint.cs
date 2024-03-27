using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DataflowPoint
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public const string TYPE = "DataflowPoint";
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Classname { get; set; }
        public string Color { get; set; }
        public string fillColor { get; set; }
        public string? InputValue { get; set; }
        public bool isEditing { get; set; } = false;
    }
}
