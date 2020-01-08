namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedtheadminforagroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "AdminId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Groups", "AdminId");
            AddForeignKey("dbo.Groups", "AdminId", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "AdminId", "dbo.ApplicationUsers");
            DropIndex("dbo.Groups", new[] { "AdminId" });
            DropColumn("dbo.Groups", "AdminId");
        }
    }
}
