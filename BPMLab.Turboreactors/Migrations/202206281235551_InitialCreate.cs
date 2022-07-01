namespace BPMLab.Turboreactors.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Manufacture",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        OfficeBuildingColor = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Turboreactor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Power = c.Int(nullable: false),
                        BladesCount = c.Int(nullable: false),
                        StartedDate = c.DateTime(nullable: false),
                        ManufactureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Manufacture", t => t.ManufactureID, cascadeDelete: true)
                .Index(t => t.ManufactureID);
            
            CreateTable(
                "dbo.TurboreactorType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TurboreactorTurboreactorType",
                c => new
                    {
                        TurboreactorID = c.Int(nullable: false),
                        TurboreactorTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TurboreactorID, t.TurboreactorTypeID })
                .ForeignKey("dbo.Turboreactor", t => t.TurboreactorID, cascadeDelete: true)
                .ForeignKey("dbo.TurboreactorType", t => t.TurboreactorTypeID, cascadeDelete: true)
                .Index(t => t.TurboreactorID)
                .Index(t => t.TurboreactorTypeID);
            
            CreateStoredProcedure(
                "dbo.Turboreactor_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                        Power = p.Int(),
                        BladesCount = p.Int(),
                        StartedDate = p.DateTime(),
                        ManufactureID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Turboreactor]([Name], [Power], [BladesCount], [StartedDate], [ManufactureID])
                      VALUES (@Name, @Power, @BladesCount, @StartedDate, @ManufactureID)
                      
                      DECLARE @ID int
                      SELECT @ID = [ID]
                      FROM [dbo].[Turboreactor]
                      WHERE @@ROWCOUNT > 0 AND [ID] = scope_identity()
                      
                      SELECT t0.[ID]
                      FROM [dbo].[Turboreactor] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            CreateStoredProcedure(
                "dbo.Turboreactor_Update",
                p => new
                    {
                        ID = p.Int(),
                        Name = p.String(maxLength: 255),
                        Power = p.Int(),
                        BladesCount = p.Int(),
                        StartedDate = p.DateTime(),
                        ManufactureID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Turboreactor]
                      SET [Name] = @Name, [Power] = @Power, [BladesCount] = @BladesCount, [StartedDate] = @StartedDate, [ManufactureID] = @ManufactureID
                      WHERE ([ID] = @ID)"
            );
            
            CreateStoredProcedure(
                "dbo.Turboreactor_Delete",
                p => new
                    {
                        ID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Turboreactor]
                      WHERE ([ID] = @ID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Turboreactor_Delete");
            DropStoredProcedure("dbo.Turboreactor_Update");
            DropStoredProcedure("dbo.Turboreactor_Insert");
            DropForeignKey("dbo.TurboreactorTurboreactorType", "TurboreactorTypeID", "dbo.TurboreactorType");
            DropForeignKey("dbo.TurboreactorTurboreactorType", "TurboreactorID", "dbo.Turboreactor");
            DropForeignKey("dbo.Turboreactor", "ManufactureID", "dbo.Manufacture");
            DropIndex("dbo.TurboreactorTurboreactorType", new[] { "TurboreactorTypeID" });
            DropIndex("dbo.TurboreactorTurboreactorType", new[] { "TurboreactorID" });
            DropIndex("dbo.Turboreactor", new[] { "ManufactureID" });
            DropTable("dbo.TurboreactorTurboreactorType");
            DropTable("dbo.TurboreactorType");
            DropTable("dbo.Turboreactor");
            DropTable("dbo.Manufacture");
        }
    }
}
