using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProfileSelect.Models;

namespace ProfileSelect
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Block> Blocks { get; set; }
        public DbSet<BlockComp> BlockComps { get; set; }
        public DbSet<BlockPriority> BlockPrioritys { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfilePriority> ProfilePrioritys { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().HasOptional(pi => pi.Status);
            modelBuilder.Entity<ApplicationUser>().HasOptional(pi => pi.CurrentGroup).WithMany(x => x.CurrentGroupStudents).WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>().HasOptional(pi => pi.PreviewGroup).WithMany(x => x.PreviewGroupStudents).WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>().HasOptional(pi => pi.NewGroup).WithMany(x => x.NewGroupStudents).WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>().HasOptional(pi => pi.Direction);
            modelBuilder.Entity<ApplicationUser>().HasOptional(pi => pi.NewProfile).WithMany(e => e.Students).HasForeignKey(e =>  e.NewProfileId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Block>().HasRequired(pi => pi.Profile);
            modelBuilder.Entity<Block>().HasRequired(pi => pi.Department);

            modelBuilder.Entity<BlockComp>().HasRequired(pi => pi.Block);
            modelBuilder.Entity<BlockComp>().HasRequired(pi => pi.Subject);

            modelBuilder.Entity<BlockPriority>().HasRequired(pi => pi.Block);
            modelBuilder.Entity<BlockPriority>().HasRequired(pi => pi.Student).WithMany(x => x.BlockPrioritys).WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>().HasRequired(pi => pi.Direction);
            modelBuilder.Entity<Group>().HasRequired(pi => pi.Department);
            

            modelBuilder.Entity<Profile>().HasRequired(pi => pi.Direction);
            modelBuilder.Entity<Profile>().HasRequired(pi => pi.Department);

            modelBuilder.Entity<ProfilePriority>().HasRequired(pi => pi.Profile);
            modelBuilder.Entity<ProfilePriority>().HasRequired(pi => pi.Student).WithMany(x => x.ProfilePrioritys).WillCascadeOnDelete(false);
        }
    }
}
