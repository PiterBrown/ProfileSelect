namespace ProfileSelect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05072019 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.AspNetUsers", "NewDirection_Id", "dbo.Directions");
            //DropIndex("dbo.AspNetUsers", new[] { "Direction_Id1" });
            //DropIndex("dbo.AspNetUsers", new[] { "NewDirection_Id" });
            //DropColumn("dbo.AspNetUsers", "Direction_Id");
            //RenameColumn(table: "dbo.AspNetUsers", name: "Direction_Id1", newName: "Direction_Id");
            //DropColumn("dbo.AspNetUsers", "NewDirection_Id");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.AspNetUsers", "NewDirection_Id", c => c.Int());
            //RenameColumn(table: "dbo.AspNetUsers", name: "Direction_Id", newName: "Direction_Id1");
            //AddColumn("dbo.AspNetUsers", "Direction_Id", c => c.Int());
            //CreateIndex("dbo.AspNetUsers", "NewDirection_Id");
            //CreateIndex("dbo.AspNetUsers", "Direction_Id1");
            //AddForeignKey("dbo.AspNetUsers", "NewDirection_Id", "dbo.Directions", "Id");
        }
    }
}
