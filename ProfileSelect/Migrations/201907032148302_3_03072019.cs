namespace ProfileSelect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3_03072019 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Block_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Block_Id");
            AddForeignKey("dbo.AspNetUsers", "Block_Id", "dbo.Blocks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Block_Id", "dbo.Blocks");
            DropIndex("dbo.AspNetUsers", new[] { "Block_Id" });
            DropColumn("dbo.AspNetUsers", "Block_Id");
        }
    }
}
