using System.Collections.Generic;

namespace ProfileSelect.Models
{
    /// <summary>
    /// Специализация
    /// </summary>
    public class Direction
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<ApplicationUser> Students { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }
    }
}
