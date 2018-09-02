namespace Zakat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirdupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.C", "ZYear", "dbo.B");
            DropIndex("dbo.C", new[] { "ZYear" });
            DropPrimaryKey("dbo.B");
            AddColumn("dbo.B", "ID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.C", "ZakatYear_ID", c => c.Int());
            AlterColumn("dbo.B", "ZYear", c => c.String());
            AlterColumn("dbo.C", "ZYear", c => c.String(nullable: false));
            AddPrimaryKey("dbo.B", "ID");
            CreateIndex("dbo.C", "ZakatYear_ID");
            AddForeignKey("dbo.C", "ZakatYear_ID", "dbo.B", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.C", "ZakatYear_ID", "dbo.B");
            DropIndex("dbo.C", new[] { "ZakatYear_ID" });
            DropPrimaryKey("dbo.B");
            AlterColumn("dbo.C", "ZYear", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.B", "ZYear", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.C", "ZakatYear_ID");
            DropColumn("dbo.B", "ID");
            AddPrimaryKey("dbo.B", "ZYear");
            CreateIndex("dbo.C", "ZYear");
            AddForeignKey("dbo.C", "ZYear", "dbo.B", "ZYear", cascadeDelete: true);
        }
    }
}
