namespace ProductRationing.DAL.Models
{
    public class OperationByCompaund
    {
        public double OperationCount { get; set; }
        public int OperationDepartment { get; set; }
        public string OperationCode { get; set; }
        public string OperationName { get; set; }
        public double OperationLabor { get; set; }
        public string OperationDescription { get; set; }
        public string OperationUnitName { get; set; }
        public string OperationGroupName { get; set; }
    }
}
