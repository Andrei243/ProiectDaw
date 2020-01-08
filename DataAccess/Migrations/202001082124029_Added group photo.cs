namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedgroupphoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "GroupId", c => c.Int());
            AddColumn("dbo.Groups", "Photo_Id", c => c.Int());
            CreateIndex("dbo.Groups", "Photo_Id");
            AddForeignKey("dbo.Groups", "Photo_Id", "dbo.Photos", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "Photo_Id", "dbo.Photos");
            DropIndex("dbo.Groups", new[] { "Photo_Id" });
            DropColumn("dbo.Groups", "Photo_Id");
            DropColumn("dbo.Photos", "GroupId");
        }
    }
}
