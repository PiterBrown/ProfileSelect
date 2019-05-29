using System;
using Microsoft.AspNet.Identity.EntityFramework;
using ProfileSelect.Models;

namespace ProfileSelect.Migrations
{
    public static class Constants
    {
        public static class RolesConstants
        {
            /// <summary>
            /// Администратор
            /// </summary>
            public static IdentityRole Admin => new IdentityRole("Admin");
            /// <summary>
            /// Студент
            /// </summary>
            public static IdentityRole Student => new IdentityRole("Student");
        }

        public static class UsersConstants
        {
            public static ApplicationUser Admin => new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin@mail.ru",
                Email = "admin@mail.ru",
                EmailConfirmed = true,
                CreateDate = DateTime.Now,
                ValidUntil = DateTime.Now
            };
        }

        public static string Password => "123456a";
    }
}