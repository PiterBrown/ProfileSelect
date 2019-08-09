namespace ProfileSelect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_03072019 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NewDepartment_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "NewDepartment_Id");
            AddForeignKey("dbo.AspNetUsers", "NewDepartment_Id", "dbo.Departments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "NewDepartment_Id", "dbo.Departments");
            DropIndex("dbo.AspNetUsers", new[] { "NewDepartment_Id" });
            DropColumn("dbo.AspNetUsers", "NewDepartment_Id");
        }
    }
}
