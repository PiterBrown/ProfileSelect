using System.Collections.Generic;

namespace ProfileSelect.ViewModels
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DirectionName { get; set; }
        public int DirectionId { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        //public string StatusName { get; set; }
        //public int StatusId { get; set; }
        public int Count { get; set; }
        public bool IsDist { get; set; }
        public bool IsDelete { get; set; }
        public List<ProfileViewModel> Profiles { get; set; }
        public List<DirectionViewModel> Directions { get; set; }
        public List<DepartmentViewModel> Departments { get; set; }
        public List<StatusViewModel> Statuses { get; set; }
    }
}
