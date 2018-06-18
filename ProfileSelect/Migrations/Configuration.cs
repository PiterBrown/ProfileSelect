using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProfileSelect.Models;

namespace ProfileSelect.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == Constants.RolesConstants.Admin.Name))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                manager.Create(Constants.RolesConstants.Admin);
            }
            if (!context.Users.Any(u => u.UserName == Constants.UsersConstants.Admin.UserName))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                manager.PasswordHasher = new CustomPasswordHasher();
                var user = Constants.UsersConstants.Admin;

                manager.Create(user, Constants.Password);
                manager.AddToRole(user.Id, Constants.RolesConstants.Admin.Name);
            }

            if (!context.Roles.Any(r => r.Name == Constants.RolesConstants.Student.Name))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                manager.Create(Constants.RolesConstants.Student);
            }
        }
    }

    internal class CustomPasswordHasher : PasswordHasher
    {
        public override string HashPassword(string password)
        {
            return password;
        }

        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return hashedPassword.Equals(providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
