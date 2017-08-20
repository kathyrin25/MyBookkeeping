namespace MyBookkeeping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookkeepingLogs",
                c => new
                    {
                        sn = c.Int(nullable: false, identity: true),
                        fk_Id = c.Guid(nullable: false),
                        Action = c.String(maxLength: 10),
                        UpdateBy = c.String(maxLength: 100),
                        UpdateDT = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.sn);
            
            CreateTable(
                "dbo.Bookkeepings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        sn = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        Remark = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bookkeepings");
            DropTable("dbo.BookkeepingLogs");
        }
    }
}
