using ProductRationing.Models;

namespace ProductRationing.DAL.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public int Department { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Labor { get; set; }
        public string Description { get; set; }

        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int? BigOperationId { get; set; }
        public BigOperation BigOperation { get; set; }

        public int? ProfessionId { get; set; }
        public Profession Profession { get; set; }

        public int? TechProcessTypeId { get; set; }
        public TechProcessType TechProcessType { get; set; }

        public string MaterialName { get; set; }

        public int Rank { get; set; }

        public string CodifierCode { get; set; }
        public string CodifierName { get; set; }
        public string CodifierGroupCode { get; set; }
        public string CodifierGroupName { get; set; }

        public string Description2 { get; set; }
        public string TechProOperationName { get; set; }
    }
}