namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedmessagesandgroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUserGroups",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GroupId, t.UserId })
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.GroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.String(nullable: false, maxLength: 128),
                        ReceiverId = c.String(maxLength: 128),
                        GroupId = c.Int(),
                        Content = c.String(nullable: false),
                        SendingMoment = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ReceiverId)
                .ForeignKey("dbo.ApplicationUsers", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Messages", "GroupId", "dbo.Groups");
            DropIndex("dbo.Messages", new[] { "GroupId" });
            DropIndex("dbo.Messages", new[] { "ReceiverId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "UserId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropTable("dbo.Messages");
            DropTable("dbo.Groups");
            DropTable("dbo.ApplicationUserGroups");
        }
    }
}
