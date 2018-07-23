using System;
using System.Collections.Generic;

namespace ProfileSelect.ViewModels
{
    public class StudentViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string FullName { get; set; }
        public string Number { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string StatusComm { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        public string CurrentGroupName { get; set; }
        public int CurrentGroupId { get; set; }
        public string PreviewGroupName { get; set; }
        public int? PreviewGroupId { get; set; }
        public string NewGroupName { get; set; }
        public int? NewGroupId { get; set; }
        public string DirectionName { get; set; }
        public int DirectionId { get; set; }
        public string NewProfileName { get; set; }
        public int? NewProfileId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public float AverageScore { get; set; }
        public float Score { get; set; }
        public int ClaimNumber { get; set; }
        public List<StatusViewModel> Statuses { get; set; }
        public List<DirectionViewModel> Directions { get; set; }
        public List<GroupViewModel> Groups { get; set; }
        public List<ProfileViewModel> Profiles { get; set; }
    }
}
