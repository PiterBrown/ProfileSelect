using System;
using System.Collections.Generic;

namespace ProfileSelect.ViewModels
{
    public class ProfileSubjectSelectViewModel
    {
        public List<ProfilePriorityViewModel> ProfilePriority { get; set; }
        public List<StudentViewModel> Student { get; set; }
        public List<BlockPriorityViewModel> BlockPriority { get; set; }
    }
}
