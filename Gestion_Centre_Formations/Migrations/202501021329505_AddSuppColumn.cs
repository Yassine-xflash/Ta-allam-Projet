namespace Gestion_Centre_Formations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSuppColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Actualites", "Supp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Admins", "Supp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Avis", "Supp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Formations", "Supp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Formateurs", "Supp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Participants", "Supp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Certificates", "Supp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Exams", "Supp", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exams", "Supp");
            DropColumn("dbo.Certificates", "Supp");
            DropColumn("dbo.Participants", "Supp");
            DropColumn("dbo.Formateurs", "Supp");
            DropColumn("dbo.Formations", "Supp");
            DropColumn("dbo.Avis", "Supp");
            DropColumn("dbo.Admins", "Supp");
            DropColumn("dbo.Actualites", "Supp");
        }
    }
}
