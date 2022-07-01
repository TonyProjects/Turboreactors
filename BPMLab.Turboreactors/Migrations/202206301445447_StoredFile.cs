namespace BPMLab.Turboreactors.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoredFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoredFile",
                c => new
                    {
                        StoredFileID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StoredName = c.String(),
                    })
                .PrimaryKey(t => t.StoredFileID);
            
            AddColumn("dbo.Manufacture", "LogoImage_StoredFileID", c => c.Int());
            CreateIndex("dbo.Manufacture", "LogoImage_StoredFileID");
            AddForeignKey("dbo.Manufacture", "LogoImage_StoredFileID", "dbo.StoredFile", "StoredFileID");
            CreateStoredProcedure(
                "dbo.StoredFile_Insert",
                p => new
                    {
                        Name = p.String(),
                        StoredName = p.String(),
                    },
                body:
                    @"INSERT [dbo].[StoredFile]([Name], [StoredName])
                      VALUES (@Name, @StoredName)
                      
                      DECLARE @StoredFileID int
                      SELECT @StoredFileID = [StoredFileID]
                      FROM [dbo].[StoredFile]
                      WHERE @@ROWCOUNT > 0 AND [StoredFileID] = scope_identity()
                      
                      SELECT t0.[StoredFileID]
                      FROM [dbo].[StoredFile] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[StoredFileID] = @StoredFileID"
            );
            
            CreateStoredProcedure(
                "dbo.StoredFile_Update",
                p => new
                    {
                        StoredFileID = p.Int(),
                        Name = p.String(),
                        StoredName = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[StoredFile]
                      SET [Name] = @Name, [StoredName] = @StoredName
                      WHERE ([StoredFileID] = @StoredFileID)"
            );
            
            CreateStoredProcedure(
                "dbo.StoredFile_Delete",
                p => new
                    {
                        StoredFileID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[StoredFile]
                      WHERE ([StoredFileID] = @StoredFileID)"
            );
            
            CreateStoredProcedure(
                "dbo.Manufacture_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                        OfficeBuildingColor = p.String(),
                        LogoImage_StoredFileID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Manufacture]([Name], [OfficeBuildingColor], [LogoImage_StoredFileID])
                      VALUES (@Name, @OfficeBuildingColor, @LogoImage_StoredFileID)
                      
                      DECLARE @ID int
                      SELECT @ID = [ID]
                      FROM [dbo].[Manufacture]
                      WHERE @@ROWCOUNT > 0 AND [ID] = scope_identity()
                      
                      SELECT t0.[ID]
                      FROM [dbo].[Manufacture] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            CreateStoredProcedure(
                "dbo.Manufacture_Update",
                p => new
                    {
                        ID = p.Int(),
                        Name = p.String(maxLength: 255),
                        OfficeBuildingColor = p.String(),
                        LogoImage_StoredFileID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Manufacture]
                      SET [Name] = @Name, [OfficeBuildingColor] = @OfficeBuildingColor, [LogoImage_StoredFileID] = @LogoImage_StoredFileID
                      WHERE ([ID] = @ID)"
            );
            
            CreateStoredProcedure(
                "dbo.Manufacture_Delete",
                p => new
                    {
                        ID = p.Int(),
                        LogoImage_StoredFileID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Manufacture]
                      WHERE (([ID] = @ID) AND (([LogoImage_StoredFileID] = @LogoImage_StoredFileID) OR ([LogoImage_StoredFileID] IS NULL AND @LogoImage_StoredFileID IS NULL)))"
            );
            
            CreateStoredProcedure(
                "dbo.TurboreactorType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                        Description = p.String(),
                    },
                body:
                    @"INSERT [dbo].[TurboreactorType]([Name], [Description])
                      VALUES (@Name, @Description)
                      
                      DECLARE @ID int
                      SELECT @ID = [ID]
                      FROM [dbo].[TurboreactorType]
                      WHERE @@ROWCOUNT > 0 AND [ID] = scope_identity()
                      
                      SELECT t0.[ID]
                      FROM [dbo].[TurboreactorType] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            CreateStoredProcedure(
                "dbo.TurboreactorType_Update",
                p => new
                    {
                        ID = p.Int(),
                        Name = p.String(maxLength: 255),
                        Description = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[TurboreactorType]
                      SET [Name] = @Name, [Description] = @Description
                      WHERE ([ID] = @ID)"
            );
            
            CreateStoredProcedure(
                "dbo.TurboreactorType_Delete",
                p => new
                    {
                        ID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[TurboreactorType]
                      WHERE ([ID] = @ID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.TurboreactorType_Delete");
            DropStoredProcedure("dbo.TurboreactorType_Update");
            DropStoredProcedure("dbo.TurboreactorType_Insert");
            DropStoredProcedure("dbo.Manufacture_Delete");
            DropStoredProcedure("dbo.Manufacture_Update");
            DropStoredProcedure("dbo.Manufacture_Insert");
            DropStoredProcedure("dbo.StoredFile_Delete");
            DropStoredProcedure("dbo.StoredFile_Update");
            DropStoredProcedure("dbo.StoredFile_Insert");
            DropForeignKey("dbo.Manufacture", "LogoImage_StoredFileID", "dbo.StoredFile");
            DropIndex("dbo.Manufacture", new[] { "LogoImage_StoredFileID" });
            DropColumn("dbo.Manufacture", "LogoImage_StoredFileID");
            DropTable("dbo.StoredFile");
        }
    }
}
