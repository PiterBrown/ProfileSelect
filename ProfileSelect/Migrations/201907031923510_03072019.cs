namespace ProfileSelect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03072019 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.AspNetUsers", "Direction_Id", "dbo.Directions");
            //AddColumn("dbo.AspNetUsers", "Direction_Id1", c => c.Int());
            //AddColumn("dbo.AspNetUsers", "NewDirection_Id", c => c.Int());
            //CreateIndex("dbo.AspNetUsers", "Direction_Id1");
            //CreateIndex("dbo.AspNetUsers", "NewDirection_Id");
            //AddForeignKey("dbo.AspNetUsers", "NewDirection_Id", "dbo.Directions", "Id");
            //AddForeignKey("dbo.AspNetUsers", "Direction_Id1", "dbo.Directions", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.AspNetUsers", "Direction_Id1", "dbo.Directions");
            //DropForeignKey("dbo.AspNetUsers", "NewDirection_Id", "dbo.Directions");
            //DropIndex("dbo.AspNetUsers", new[] { "NewDirection_Id" });
            //DropIndex("dbo.AspNetUsers", new[] { "Direction_Id1" });
            //DropColumn("dbo.AspNetUsers", "NewDirection_Id");
            //DropColumn("dbo.AspNetUsers", "Direction_Id1");
            //AddForeignKey("dbo.AspNetUsers", "Direction_Id", "dbo.Directions", "Id");
        }
    }
}
