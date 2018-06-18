namespace ProfileSelect.Models
{
    public class BlockPriority
    {
        public int Id { get; set; }
        public ApplicationUser Student { get; set; }
        public Block Block { get; set; }
        public int Priority { get; set; }
    }
}
