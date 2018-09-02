namespace Zakat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstupdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.A",
                c => new
                    {
                        AID = c.Guid(nullable: false),
                        AccountName = c.String(nullable: false),
                        password = c.String(),
                    })
                .PrimaryKey(t => t.AID);
            
            CreateTable(
                "dbo.B",
                c => new
                    {
                        ZYear = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        AID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ZYear)
                .ForeignKey("dbo.A", t => t.AID, cascadeDelete: true)
                .Index(t => t.AID);
            
            CreateTable(
                "dbo.C",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemName = c.String(nullable: false),
                        Description = c.String(),
                        MarketPrice = c.Double(nullable: false),
                        ItemZakat = c.Double(nullable: false),
                        ZYear = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.B", t => t.ZYear, cascadeDelete: true)
                .Index(t => t.ZYear);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.C", "ZYear", "dbo.B");
            DropForeignKey("dbo.B", "AID", "dbo.A");
            DropIndex("dbo.C", new[] { "ZYear" });
            DropIndex("dbo.B", new[] { "AID" });
            DropTable("dbo.C");
            DropTable("dbo.B");
            DropTable("dbo.A");
        }
    }
}
