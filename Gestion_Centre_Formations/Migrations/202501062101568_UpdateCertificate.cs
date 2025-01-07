namespace Gestion_Centre_Formations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCertificate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormationParticipants", "HasCertificate", c => c.Boolean(nullable: false));
            DropColumn("dbo.Certificates", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Certificates", "Description", c => c.String());
            DropColumn("dbo.FormationParticipants", "HasCertificate");
        }
    }
}
