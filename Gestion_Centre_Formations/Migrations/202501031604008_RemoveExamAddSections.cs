namespace Gestion_Centre_Formations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveExamAddSections : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Exams", "FormationID", "dbo.Formations");
            DropForeignKey("dbo.Exams", "ParticipantID", "dbo.Participants");
            DropIndex("dbo.Exams", new[] { "ParticipantID" });
            DropIndex("dbo.Exams", new[] { "FormationID" });
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        Supp = c.Boolean(nullable: false),
                        ActualiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Actualites", t => t.ActualiteId, cascadeDelete: true)
                .Index(t => t.ActualiteId);
            
            AddColumn("dbo.Actualites", "Views", c => c.Int(nullable: false));
            DropTable("dbo.Exams");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        ExamID = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        ExamDate = c.DateTime(nullable: false),
                        MaxScore = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Supp = c.Boolean(nullable: false),
                        ParticipantID = c.Int(nullable: false),
                        FormationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExamID);
            
            DropForeignKey("dbo.Sections", "ActualiteId", "dbo.Actualites");
            DropIndex("dbo.Sections", new[] { "ActualiteId" });
            DropColumn("dbo.Actualites", "Views");
            DropTable("dbo.Sections");
            CreateIndex("dbo.Exams", "FormationID");
            CreateIndex("dbo.Exams", "ParticipantID");
            AddForeignKey("dbo.Exams", "ParticipantID", "dbo.Participants", "ParticipantID", cascadeDelete: true);
            AddForeignKey("dbo.Exams", "FormationID", "dbo.Formations", "FormationID", cascadeDelete: true);
        }
    }
}
