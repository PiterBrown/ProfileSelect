namespace ProfileSelect.ViewModels
{
    public class BlockPriorityViewModel
    {
        public int Id { get; set; }
        public int BlockId { get; set; }
        public int Priority { get; set; }
        public string DepartmentName { get; set; }
        public string ProfileName { get; set; }
        public string StudentId { get; set; }
        public bool IsDelete { get; set; }
    }
}