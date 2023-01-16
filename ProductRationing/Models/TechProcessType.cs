namespace ProductRationing.Models
{
    public class TechProcessType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public override string ToString() => $"{Code} - {Name}";
    }
}
