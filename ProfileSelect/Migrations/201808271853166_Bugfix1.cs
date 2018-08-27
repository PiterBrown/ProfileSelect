namespace ProfileSelect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bugfix1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsBusy", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "BusyReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BusyReason");
            DropColumn("dbo.AspNetUsers", "IsBusy");
        }
    }
}
