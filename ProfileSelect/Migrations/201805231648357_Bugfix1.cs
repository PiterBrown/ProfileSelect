namespace ProfileSelect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bugfix1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "ValidUntil", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "ValidUntil", c => c.DateTime(nullable: false));
        }
    }
}
