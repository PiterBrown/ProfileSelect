using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProfileSelect.Models
{
    /// <summary>
    /// Студент или админ
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string FullName { get; set; }
        public string Number { get; set; }
        public bool IsActive { get; set; }
        public string StatusComm { get; set; }
        public virtual Status Status { get; set; }
        public virtual Group CurrentGroup { get; set; }
        public virtual Group PreviewGroup { get; set; }
        public virtual Group NewGroup { get; set; }
        public virtual Direction Direction { get; set; }
        [Column("NewProfile_Id")]
        public int? NewProfileId { get; set; }
        public virtual Profile NewProfile { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public float AverageScore { get; set; }
        public int ClaimNumber { get; set; }
        public virtual ICollection<ProfilePriority> ProfilePrioritys { get; set; }
        public virtual ICollection<BlockPriority> BlockPrioritys { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
