namespace Gestion_Centre_Formations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFormationModelConstraints : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Formations", "Titre", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Formations", "Description", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Formations", "Description", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Formations", "Titre", c => c.String(nullable: false, maxLength: 25));
        }
    }
}
