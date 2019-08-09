using System.Collections.Generic;

namespace ProfileSelect.Models
{
    public class Block
    {
        public int Id { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual Department Department { get; set; }
        public bool IsDelete { get; set; }
        public virtual ICollection<ApplicationUser> Students { get; set; }
    }
}
