namespace ProductRationing.DAL.Models
{
    public class OldOperation
    {
        public int Id { get; set; }
        public int Department { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Labor { get; set; }
        public string Description { get; set; }

        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int? BigOperationId { get; set; }
        public BigOperation BigOperation { get; set; }
    }
}