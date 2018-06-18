using System.Collections.Generic;

namespace ProfileSelect.Models
{
    /// <summary>
    /// Кафедра
    /// </summary>
    public class Department
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public bool IsCompany { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
