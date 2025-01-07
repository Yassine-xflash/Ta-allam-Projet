namespace Gestion_Centre_Formations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePaymentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "FormationID", c => c.Int(nullable: false));
            AddColumn("dbo.Payments", "ParticipantID", c => c.Int(nullable: false));
            AlterColumn("dbo.Payments", "Amount", c => c.Double(nullable: false));
            CreateIndex("dbo.Payments", "FormationID");
            CreateIndex("dbo.Payments", "ParticipantID");
            AddForeignKey("dbo.Payments", "FormationID", "dbo.Formations", "FormationID", cascadeDelete: true);
            AddForeignKey("dbo.Payments", "ParticipantID", "dbo.Participants", "ParticipantID", cascadeDelete: true);
            DropColumn("dbo.Payments", "CourseId");
            DropColumn("dbo.Payments", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Payments", "CourseId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Payments", "ParticipantID", "dbo.Participants");
            DropForeignKey("dbo.Payments", "FormationID", "dbo.Formations");
            DropIndex("dbo.Payments", new[] { "ParticipantID" });
            DropIndex("dbo.Payments", new[] { "FormationID" });
            AlterColumn("dbo.Payments", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Payments", "ParticipantID");
            DropColumn("dbo.Payments", "FormationID");
        }
    }
}
