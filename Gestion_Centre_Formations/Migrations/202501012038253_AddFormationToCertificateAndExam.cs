namespace Gestion_Centre_Formations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFormationToCertificateAndExam : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Certificates",
                c => new
                    {
                        CertificateID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        DateIssued = c.DateTime(nullable: false),
                        ParticipantID = c.Int(nullable: false),
                        FormationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CertificateID)
                .ForeignKey("dbo.Formations", t => t.FormationID, cascadeDelete: true)
                .ForeignKey("dbo.Participants", t => t.ParticipantID, cascadeDelete: true)
                .Index(t => t.ParticipantID)
                .Index(t => t.FormationID);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        ExamID = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        ExamDate = c.DateTime(nullable: false),
                        MaxScore = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ParticipantID = c.Int(nullable: false),
                        FormationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExamID)
                .ForeignKey("dbo.Formations", t => t.FormationID, cascadeDelete: true)
                .ForeignKey("dbo.Participants", t => t.ParticipantID, cascadeDelete: true)
                .Index(t => t.ParticipantID)
                .Index(t => t.FormationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exams", "ParticipantID", "dbo.Participants");
            DropForeignKey("dbo.Exams", "FormationID", "dbo.Formations");
            DropForeignKey("dbo.Certificates", "ParticipantID", "dbo.Participants");
            DropForeignKey("dbo.Certificates", "FormationID", "dbo.Formations");
            DropIndex("dbo.Exams", new[] { "FormationID" });
            DropIndex("dbo.Exams", new[] { "ParticipantID" });
            DropIndex("dbo.Certificates", new[] { "FormationID" });
            DropIndex("dbo.Certificates", new[] { "ParticipantID" });
            DropTable("dbo.Exams");
            DropTable("dbo.Certificates");
        }
    }
}
