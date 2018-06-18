using System.Collections.Generic;

namespace ProfileSelect.Models
{
    /// <summary>
    /// Статус
    /// </summary>
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
