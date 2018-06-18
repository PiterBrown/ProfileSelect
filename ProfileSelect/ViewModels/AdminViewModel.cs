using System.Collections.Generic;

namespace ProfileSelect.ViewModels
{
    public class AdminViewModel
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public List<StudentViewModel> Students { get; set; }
        public List<GroupViewModel> Groups { get; set; }
    }
}
