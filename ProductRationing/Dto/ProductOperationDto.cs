namespace ProductRationing.Dto
{
    public class ProductOperationDto
    {
        public string ProductCode { get; set; }
        public double Count { get; set; }
        public string Description { get; set; }

        public int OperationDepartment { get; set; }
        public string OperationCode { get; set; }
        public string OperationName { get; set; }
        public decimal OperationLabor { get; set; }
        public string OperationDescription { get; set; }
        public string OperationUnitName { get; set; }
        public string OperationGroupName { get; set; }

        public string OperationBigOperationCode { get; set; }
        public string OperationBigOperationName { get; set; }
    }
}
