namespace Zakat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secondupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.B", "AID", "dbo.A");
            DropPrimaryKey("dbo.A");
            AlterColumn("dbo.A", "AID", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.A", "AID");
            AddForeignKey("dbo.B", "AID", "dbo.A", "AID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.B", "AID", "dbo.A");
            DropPrimaryKey("dbo.A");
            AlterColumn("dbo.A", "AID", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.A", "AID");
            AddForeignKey("dbo.B", "AID", "dbo.A", "AID", cascadeDelete: true);
        }
    }
}
