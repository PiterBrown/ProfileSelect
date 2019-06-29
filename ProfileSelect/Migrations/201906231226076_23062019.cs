namespace ProfileSelect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _23062019 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsParc", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsParc");
        }
    }
}
