namespace BPMLab.Turboreactors.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BPMLab.Turboreactors.Models;
    using BPMLab.Turboreactors.DAL;

    internal sealed class Configuration : DbMigrationsConfiguration<BPMLab.Turboreactors.DAL.TurboreactorsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BPMLab.Turboreactors.DAL.TurboreactorsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var manufactures = new List<Manufacture>
            {
                new Manufacture { Name = "GE Aviation" },
                new Manufacture { Name = "Honeywell" }
            };
            manufactures.ForEach(s => context.Manufactures.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var turboreactors = new List<Turboreactor>
            {
                new Turboreactor { Name = "CF34-3", Power = 8729, BladesCount = 40,
                    StartDate = DateTime.Parse("1973-09-01"),
                    ManufactureID = manufactures.Single( m => m.Name == "GE Aviation").ID},
                new Turboreactor { Name = "CF34-8", Power = 41000, BladesCount = 60,
                    StartDate = DateTime.Parse("1980-09-12"),
                    ManufactureID = manufactures.Single( m => m.Name == "GE Aviation").ID }
            };
            turboreactors.ForEach(s => context.Turboreactors.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var types = new List<TurboreactorType>
            {
                new TurboreactorType { Name = "Regular" },
                new TurboreactorType { Name = "Military" },
                new TurboreactorType { Name = "14500 lb class"}
            };
            types.ForEach(s => context.TurboreactorTypes.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            AddOrUpdateTurboreactorType(context, "CF34-3", "Regular");
            AddOrUpdateTurboreactorType(context, "CF34-3", "14500 lb class");
            AddOrUpdateTurboreactorType(context, "CF34-8", "Regular");
        }

        void AddOrUpdateTurboreactorType(TurboreactorsContext context, string turboreactorName, string typeName)
        {
            var turboreactor = context.Turboreactors.SingleOrDefault(t => t.Name == turboreactorName);
            var type = turboreactor.Types.SingleOrDefault(t => t.Name == typeName);
            if (type == null)
                turboreactor.Types.Add(context.TurboreactorTypes.Single(t => t.Name == typeName));
        }
    }
}
