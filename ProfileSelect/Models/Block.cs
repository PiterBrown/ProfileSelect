namespace ProfileSelect.Models
{
    public class Block
    {
        public int Id { get; set; }
        public ApplicationUser Profile { get; set; }
        public Department Department { get; set; }
    }
}
