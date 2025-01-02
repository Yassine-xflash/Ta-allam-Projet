namespace Gestion_Centre_Formations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvisAndActualites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actualites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            CreateTable(
                "dbo.Avis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParticipantID = c.Int(nullable: false),
                        FormationID = c.Int(nullable: false),
                        Review = c.String(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Formations", t => t.FormationID, cascadeDelete: true)
                .ForeignKey("dbo.Participants", t => t.ParticipantID, cascadeDelete: true)
                .Index(t => t.ParticipantID)
                .Index(t => t.FormationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Avis", "ParticipantID", "dbo.Participants");
            DropForeignKey("dbo.Avis", "FormationID", "dbo.Formations");
            DropIndex("dbo.Avis", new[] { "FormationID" });
            DropIndex("dbo.Avis", new[] { "ParticipantID" });
            DropTable("dbo.Avis");
            DropTable("dbo.Actualites");
        }
    }
}
