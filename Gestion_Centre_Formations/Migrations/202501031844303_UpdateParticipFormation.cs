namespace Gestion_Centre_Formations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateParticipFormation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormationParticipants", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.FormationParticipants", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FormationParticipants", "Comment");
            DropColumn("dbo.FormationParticipants", "Rating");
        }
    }
}
