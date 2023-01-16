namespace ProductRationing.DAL.Models
{
    public class ProductOperation
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public double Count { get; set; }
        public string Description { get; set; }
        public int DifficultyGroup { get; set; }
        public decimal MachineTime { get; set; }
        public int Number { get; set; }

        public int OperationId { get; set; }
        public Operation Operation { get; set; }
    }
}
