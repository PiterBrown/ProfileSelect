using System.Collections.Generic;

namespace ProfileSelect.ViewModels
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DirectionName { get; set; }
        public int DirectionId { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public string BaseDepartmentName { get; set; }
        public int? BaseDepartmentId { get; set; }
        public List<DirectionViewModel> Directions { get; set; }
        public List<DepartmentViewModel> Departments { get; set; }
    }
}
