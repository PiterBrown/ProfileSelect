using System.Collections.Generic;

namespace ProfileSelect.ViewModels
{
    public class BlockCompViewModel
    {
        public int BlockCompId { get; set; }
        public int BlockId { get; set; }
        public int ProfileId { get; set; }
        public int SubjectsId { get; set; }
        public string SubjectName { get; set; }
        public int DirectionId { get; set; }
    }
}