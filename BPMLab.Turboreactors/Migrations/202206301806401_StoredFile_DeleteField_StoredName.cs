namespace BPMLab.Turboreactors.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoredFile_DeleteField_StoredName : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.StoredFile", "StoredName");
            AlterStoredProcedure(
                "dbo.StoredFile_Insert",
                p => new
                    {
                        Name = p.String(),
                    },
                body:
                    @"INSERT [dbo].[StoredFile]([Name])
                      VALUES (@Name)
                      
                      DECLARE @StoredFileID int
                      SELECT @StoredFileID = [StoredFileID]
                      FROM [dbo].[StoredFile]
                      WHERE @@ROWCOUNT > 0 AND [StoredFileID] = scope_identity()
                      
                      SELECT t0.[StoredFileID]
                      FROM [dbo].[StoredFile] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[StoredFileID] = @StoredFileID"
            );
            
            AlterStoredProcedure(
                "dbo.StoredFile_Update",
                p => new
                    {
                        StoredFileID = p.Int(),
                        Name = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[StoredFile]
                      SET [Name] = @Name
                      WHERE ([StoredFileID] = @StoredFileID)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.StoredFile", "StoredName", c => c.String());
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
