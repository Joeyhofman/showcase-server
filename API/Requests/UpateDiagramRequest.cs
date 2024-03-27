using Domain.Entities;

namespace API.Requests
{
    public class UpateDiagramRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<DataflowPoint> Points { get; set; }
        public List<DataflowAssociation> Associations { get; set; }
    }
}
