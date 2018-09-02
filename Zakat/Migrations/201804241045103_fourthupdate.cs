namespace Zakat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fourthupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.C", "ZakatYear_ID", "dbo.B");
            DropIndex("dbo.C", new[] { "ZakatYear_ID" });
            DropPrimaryKey("dbo.B");
            DropColumn("dbo.B", "ID");
            RenameColumn(table: "dbo.C", name: "ZakatYear_ID", newName: "ZID");
            AddColumn("dbo.B", "ZID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.C", "ZID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.B", "ZID");
            CreateIndex("dbo.C", "ZID");
            AddForeignKey("dbo.C", "ZID", "dbo.B", "ZID", cascadeDelete: true);
            DropColumn("dbo.C", "ZYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.C", "ZYear", c => c.String(nullable: false));
            AddColumn("dbo.B", "ID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.C", "ZID", "dbo.B");
            DropIndex("dbo.C", new[] { "ZID" });
            DropPrimaryKey("dbo.B");
            AlterColumn("dbo.C", "ZID", c => c.Int());
            DropColumn("dbo.B", "ZID");
            AddPrimaryKey("dbo.B", "ID");
            RenameColumn(table: "dbo.C", name: "ZID", newName: "ZakatYear_ID");
            CreateIndex("dbo.C", "ZakatYear_ID");
            AddForeignKey("dbo.C", "ZakatYear_ID", "dbo.B", "ID");
        }
    }
}
