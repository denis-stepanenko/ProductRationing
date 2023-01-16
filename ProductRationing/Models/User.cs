namespace ProductRationing.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Department { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}