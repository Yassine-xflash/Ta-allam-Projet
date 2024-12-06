namespace Gestion_Centre_Formations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 50),
                        Prenom = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false),
                        NumTel = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AdminID);
            
            CreateTable(
                "dbo.Formateurs",
                c => new
                    {
                        FormateurID = c.Int(nullable: false, identity: true),
                        Specialisation = c.String(nullable: false, maxLength: 25),
                        NbrFormations = c.Int(nullable: false),
                        Nom = c.String(nullable: false, maxLength: 50),
                        Prenom = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false),
                        NumTel = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FormateurID);
            
            CreateTable(
                "dbo.Formations",
                c => new
                    {
                        FormationID = c.Int(nullable: false, identity: true),
                        Titre = c.String(nullable: false, maxLength: 25),
                        Description = c.String(nullable: false, maxLength: 100),
                        Categorie = c.String(nullable: false, maxLength: 25),
                        Duration = c.Int(nullable: false),
                        Prix = c.Double(nullable: false),
                        FormateurID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FormationID)
                .ForeignKey("dbo.Formateurs", t => t.FormateurID, cascadeDelete: true)
                .Index(t => t.FormateurID);
            
            CreateTable(
                "dbo.FormationParticipants",
                c => new
                    {
                        FormationParticipantID = c.Int(nullable: false, identity: true),
                        FormationID = c.Int(nullable: false),
                        ParticipantID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FormationParticipantID)
                .ForeignKey("dbo.Formations", t => t.FormationID, cascadeDelete: true)
                .ForeignKey("dbo.Participants", t => t.ParticipantID, cascadeDelete: true)
                .Index(t => t.FormationID)
                .Index(t => t.ParticipantID);
            
            CreateTable(
                "dbo.Participants",
                c => new
                    {
                        ParticipantID = c.Int(nullable: false, identity: true),
                        DateInscription = c.DateTime(nullable: false),
                        NbrFormations = c.Int(nullable: false),
                        Nom = c.String(nullable: false, maxLength: 50),
                        Prenom = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false),
                        NumTel = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ParticipantID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FormationParticipants", "ParticipantID", "dbo.Participants");
            DropForeignKey("dbo.FormationParticipants", "FormationID", "dbo.Formations");
            DropForeignKey("dbo.Formations", "FormateurID", "dbo.Formateurs");
            DropIndex("dbo.FormationParticipants", new[] { "ParticipantID" });
            DropIndex("dbo.FormationParticipants", new[] { "FormationID" });
            DropIndex("dbo.Formations", new[] { "FormateurID" });
            DropTable("dbo.Participants");
            DropTable("dbo.FormationParticipants");
            DropTable("dbo.Formations");
            DropTable("dbo.Formateurs");
            DropTable("dbo.Admins");
        }
    }
}
