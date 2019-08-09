namespace ProfileSelect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_02072019 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlockPriorities", "IsDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProfilePriorities", "IsDelete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProfilePriorities", "IsDelete");
            DropColumn("dbo.BlockPriorities", "IsDelete");
        }
    }
}
