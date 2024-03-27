using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions.DataflowDiagrams;

namespace Domain.Entities
{
    public class DataflowDiagram
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<DataflowPoint> Points { get; private set; }
        public ICollection<DataflowAssociation> Associations { get; private set; }

        public DataflowDiagram()
        {
            Points = new List<DataflowPoint>();
            Associations = new List<DataflowAssociation>();
        }

        public DataflowDiagram(Guid id, string name, ICollection<DataflowPoint> points, ICollection<DataflowAssociation> associations)
        {
            Id = id;
            if(name is null)
            {
                throw new Exception("Name cant be null while creating diagram");
            }
            Name = name;
            Points = points;
            Associations = associations;
        }

        public void AddPoint(DataflowPoint point)
        {
            Points.Add(point);
        }

        public void UpdatePoint(int index, string name, string color)
        {
            if (index < 0 || index >= Points.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            var pointToUpdate = Points.ElementAt(index);

            if (pointToUpdate == null)
            {
                throw new PointNotFoundException("Point to update not found.");
            }

            pointToUpdate.Classname = name;
            pointToUpdate.fillColor = color;
        }

        public void RemovePoint(DataflowPoint point)
        {
            var associationsToRemove = Associations
                .Where(association => association.P1.Id == point.Id || association.P2.Id == point.Id)
                .ToList();

            foreach (var associationToRemove in associationsToRemove)
            {
                Associations.Remove(associationToRemove);
            }

            Points.Remove(point);
        }

        public void AddAssociation(DataflowAssociation association)
        {
            Associations.Add(association);
        }

        public void SetPoints(List<DataflowPoint> points)
        {
            Points = points;
        }

        public void SetAssociations(List<DataflowAssociation> associations)
        {
            Associations = associations;
        }

        public void RemoveAssociation(DataflowAssociation association)
        {
            Associations.Remove(association);
        }
    }
}
