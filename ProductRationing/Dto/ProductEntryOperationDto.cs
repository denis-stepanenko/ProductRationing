namespace ProductRationing.Dto
{
    public class ProductEntryOperationDto
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double ProductCount { get; set; }
        public string ProductRoute { get; set; }

        public double OperationCount { get; set; }
        public int OperationDepartment { get; set; }
        public string OperationCode { get; set; }
        public string OperationName { get; set; }
        public decimal OperationLabor { get; set; }
        public string OperationDescription { get; set; }
        public string OperationUnitName { get; set; }
        public string OperationGroupName { get; set; }
        public string BigOperationCode { get; set; }
        public string BigOperationName { get; set; }
    }
}
