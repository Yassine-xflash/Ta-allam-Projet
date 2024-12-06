namespace Gestion_Centre_Formations.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Gestion_Centre_Formations.Data.Gestion_Centre_FormationsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Gestion_Centre_Formations.Data.Gestion_Centre_FormationsContext";
        }

        protected override void Seed(Gestion_Centre_Formations.Data.Gestion_Centre_FormationsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
