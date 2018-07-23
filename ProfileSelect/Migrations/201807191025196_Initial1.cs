namespace ProfileSelect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Profile_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Score", c => c.Single(nullable: false));
            CreateIndex("dbo.Groups", "Profile_Id");
            AddForeignKey("dbo.Groups", "Profile_Id", "dbo.Profiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "Profile_Id", "dbo.Profiles");
            DropIndex("dbo.Groups", new[] { "Profile_Id" });
            DropColumn("dbo.AspNetUsers", "Score");
            DropColumn("dbo.Groups", "Profile_Id");
        }
    }
}
