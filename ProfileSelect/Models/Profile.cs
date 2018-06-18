using System.Collections.Generic;

namespace ProfileSelect.Models
{
    /// <summary>
    /// Направление специализации
    /// </summary>
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual Department Department { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<ApplicationUser> Students { get; set; }
    }
}
