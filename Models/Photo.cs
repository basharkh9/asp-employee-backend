namespace EmployeeCRUD.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public bool isMain { get; set; }
        public User User { get; set; } = new User();
        public int UserId { get; set; }
    }
}