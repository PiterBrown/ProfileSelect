namespace ProfileSelect.Models
{
    /// <summary>
    /// Приоритет специализации
    /// </summary>
    public class ProfilePriority
    {
        public int Id { get; set; }
        public virtual ApplicationUser Student { get; set; }
        public virtual Profile Profile { get; set; }
        public int Priority { get; set; }
    }
}
