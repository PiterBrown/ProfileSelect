using System.Collections.Generic;

namespace ProfileSelect.Models
{
    /// <summary>
    /// Группа
    /// </summary>
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual Department Department { get; set; }
        public bool IsDistr { get; set; }
        
        public int Count { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<ApplicationUser> CurrentGroupStudents { get; set; }
        public virtual ICollection<ApplicationUser> PreviewGroupStudents { get; set; }
        public virtual ICollection<ApplicationUser> NewGroupStudents { get; set; }
    }
}
