namespace BPMLab.Turboreactors.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Turboreactor_StartDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Turboreactor", "StartDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Turboreactor", "StartedDate");
            AlterStoredProcedure(
                "dbo.Turboreactor_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                        Power = p.Int(),
                        BladesCount = p.Int(),
                        StartDate = p.DateTime(),
                        ManufactureID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Turboreactor]([Name], [Power], [BladesCount], [StartDate], [ManufactureID])
                      VALUES (@Name, @Power, @BladesCount, @StartDate, @ManufactureID)
                      
                      DECLARE @ID int
                      SELECT @ID = [ID]
                      FROM [dbo].[Turboreactor]
                      WHERE @@ROWCOUNT > 0 AND [ID] = scope_identity()
                      
                      SELECT t0.[ID]
                      FROM [dbo].[Turboreactor] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            AlterStoredProcedure(
                "dbo.Turboreactor_Update",
                p => new
                    {
                        ID = p.Int(),
                        Name = p.String(maxLength: 255),
                        Power = p.Int(),
                        BladesCount = p.Int(),
                        StartDate = p.DateTime(),
                        ManufactureID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Turboreactor]
                      SET [Name] = @Name, [Power] = @Power, [BladesCount] = @BladesCount, [StartDate] = @StartDate, [ManufactureID] = @ManufactureID
                      WHERE ([ID] = @ID)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Turboreactor", "StartedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Turboreactor", "StartDate");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
