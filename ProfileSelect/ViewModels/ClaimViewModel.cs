using System.Collections.Generic;

namespace ProfileSelect.ViewModels
{
    public class ClaimViewModel
    {
        public int ClaimNuber { get; set; }
        public List<ProfilePriorityViewModel> ProfilePriorities { get; set; }
        public List<ProfileViewModel> Profiles { get; set; }
        public List<BlockPriorityViewModel> BlockPriorities { get; set; }
        public List<BlockCompViewModel> BlockComps { get; set; }
        public int  Direction { get; set; }
        public string StudentDirection { get; set; }
    }
}
