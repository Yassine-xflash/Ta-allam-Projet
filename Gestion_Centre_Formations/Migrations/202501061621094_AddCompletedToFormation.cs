namespace Gestion_Centre_Formations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompletedToFormation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Formations", "Completed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Formations", "Completed");
        }
    }
}
