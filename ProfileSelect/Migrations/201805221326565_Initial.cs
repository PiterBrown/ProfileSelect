namespace ProfileSelect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlockComps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Block_Id = c.Int(nullable: false),
                        Subject_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blocks", t => t.Block_Id, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.Subject_Id, cascadeDelete: true)
                .Index(t => t.Block_Id)
                .Index(t => t.Subject_Id);
            
            CreateTable(
                "dbo.Blocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Department_Id = c.Int(nullable: false),
                        Profile_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Profile_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.Profile_Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        ShortName = c.String(),
                        IsCompany = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Count = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Department_Id = c.Int(nullable: false),
                        Direction_Id = c.Int(nullable: false),
                        Status_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.Directions", t => t.Direction_Id, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.Status_Id)
                .Index(t => t.Department_Id)
                .Index(t => t.Direction_Id)
                .Index(t => t.Status_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Patronymic = c.String(),
                        FullName = c.String(),
                        Number = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        StatusComm = c.String(),
                        NewProfile_Id = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ValidUntil = c.DateTime(nullable: false),
                        AverageScore = c.Single(nullable: false),
                        ClaimNumber = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        CurrentGroup_Id = c.Int(),
                        Direction_Id = c.Int(),
                        NewGroup_Id = c.Int(),
                        PreviewGroup_Id = c.Int(),
                        Status_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.CurrentGroup_Id)
                .ForeignKey("dbo.Directions", t => t.Direction_Id)
                .ForeignKey("dbo.Groups", t => t.NewGroup_Id)
                .ForeignKey("dbo.Profiles", t => t.NewProfile_Id)
                .ForeignKey("dbo.Groups", t => t.PreviewGroup_Id)
                .ForeignKey("dbo.Status", t => t.Status_Id)
                .Index(t => t.NewProfile_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.CurrentGroup_Id)
                .Index(t => t.Direction_Id)
                .Index(t => t.NewGroup_Id)
                .Index(t => t.PreviewGroup_Id)
                .Index(t => t.Status_Id);
            
            CreateTable(
                "dbo.BlockPriorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Priority = c.Int(nullable: false),
                        Block_Id = c.Int(nullable: false),
                        Student_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blocks", t => t.Block_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id)
                .Index(t => t.Block_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Directions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Department_Id = c.Int(nullable: false),
                        Direction_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.Directions", t => t.Direction_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.Direction_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ProfilePriorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Priority = c.Int(nullable: false),
                        Profile_Id = c.Int(nullable: false),
                        Student_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profiles", t => t.Profile_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id)
                .Index(t => t.Profile_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.BlockComps", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.BlockComps", "Block_Id", "dbo.Blocks");
            DropForeignKey("dbo.Blocks", "Profile_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Blocks", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.Groups", "Status_Id", "dbo.Status");
            DropForeignKey("dbo.Groups", "Direction_Id", "dbo.Directions");
            DropForeignKey("dbo.Groups", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.AspNetUsers", "Status_Id", "dbo.Status");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProfilePriorities", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProfilePriorities", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.AspNetUsers", "PreviewGroup_Id", "dbo.Groups");
            DropForeignKey("dbo.AspNetUsers", "NewProfile_Id", "dbo.Profiles");
            DropForeignKey("dbo.AspNetUsers", "NewGroup_Id", "dbo.Groups");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Direction_Id", "dbo.Directions");
            DropForeignKey("dbo.Profiles", "Direction_Id", "dbo.Directions");
            DropForeignKey("dbo.Profiles", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.AspNetUsers", "CurrentGroup_Id", "dbo.Groups");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BlockPriorities", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BlockPriorities", "Block_Id", "dbo.Blocks");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.ProfilePriorities", new[] { "Student_Id" });
            DropIndex("dbo.ProfilePriorities", new[] { "Profile_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Profiles", new[] { "Direction_Id" });
            DropIndex("dbo.Profiles", new[] { "Department_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.BlockPriorities", new[] { "Student_Id" });
            DropIndex("dbo.BlockPriorities", new[] { "Block_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Status_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "PreviewGroup_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "NewGroup_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Direction_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "CurrentGroup_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "NewProfile_Id" });
            DropIndex("dbo.Groups", new[] { "Status_Id" });
            DropIndex("dbo.Groups", new[] { "Direction_Id" });
            DropIndex("dbo.Groups", new[] { "Department_Id" });
            DropIndex("dbo.Blocks", new[] { "Profile_Id" });
            DropIndex("dbo.Blocks", new[] { "Department_Id" });
            DropIndex("dbo.BlockComps", new[] { "Subject_Id" });
            DropIndex("dbo.BlockComps", new[] { "Block_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Subjects");
            DropTable("dbo.Status");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.ProfilePriorities");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Profiles");
            DropTable("dbo.Directions");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.BlockPriorities");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Groups");
            DropTable("dbo.Departments");
            DropTable("dbo.Blocks");
            DropTable("dbo.BlockComps");
        }
    }
}
