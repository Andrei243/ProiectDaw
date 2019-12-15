namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Binar = c.Binary(nullable: false, storeType: "image"),
                        MIMEType = c.String(nullable: false, maxLength: 100),
                        AlbumId = c.Int(),
                        PostId = c.Int(),
                        Position = c.Int(),
                        Description = c.String(),
                        UserId = c.String(),
                        User_Id = c.String(maxLength: 128),
                        Post_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.AlbumId)
                .Index(t => t.User_Id)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Vizibilitate = c.String(maxLength: 20, unicode: false),
                        AddingMoment = c.DateTime(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Content = c.String(nullable: false, maxLength: 3000),
                        AddingMoment = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        Surname = c.String(nullable: false, maxLength: 100, unicode: false),
                        Password = c.String(nullable: false, maxLength: 100, unicode: false),
                        RoleId = c.Int(nullable: false),
                        BirthDay = c.DateTime(nullable: false, storeType: "date"),
                        LocalityId = c.Int(nullable: false),
                        SexualIdentity = c.String(nullable: false, maxLength: 50, unicode: false),
                        Vizibility = c.String(nullable: false, maxLength: 20, unicode: false),
                        PhotoId = c.Int(),
                        IsBanned = c.Boolean(nullable: false),
                        Email = c.String(nullable: false, maxLength: 100, unicode: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Localities", t => t.LocalityId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.LocalityId);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        IdSender = c.String(nullable: false, maxLength: 128),
                        IdReceiver = c.String(nullable: false, maxLength: 128),
                        Accepted = c.Boolean(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdSender, t.IdReceiver })
                .ForeignKey("dbo.Users", t => t.IdReceiver)
                .ForeignKey("dbo.Users", t => t.IdSender)
                .Index(t => t.IdSender)
                .Index(t => t.IdReceiver);
            
            CreateTable(
                "dbo.InterestsUsers",
                c => new
                    {
                        InterestId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.InterestId, t.UserId })
                .ForeignKey("dbo.Interests", t => t.InterestId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.InterestId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Localities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountyId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Counties", t => t.CountyId)
                .Index(t => t.CountyId);
            
            CreateTable(
                "dbo.Counties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                    })
                .PrimaryKey(t => new { t.UserId, t.ProviderKey })
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Reactions",
                c => new
                    {
                        PostId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PostId, t.UserId })
                .ForeignKey("dbo.Posts", t => t.PostId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.UserId)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.ErrorLogCustoms",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        number = c.Int(),
                        severity = c.Int(),
                        state = c.Int(),
                        message = c.String(unicode: false),
                        line = c.Int(),
                        proced = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.Albums", "UserId", "dbo.Users");
            DropForeignKey("dbo.Photos", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Posts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.IdentityUserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Reactions", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reactions", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Photos", "User_Id", "dbo.Users");
            DropForeignKey("dbo.IdentityUserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "LocalityId", "dbo.Localities");
            DropForeignKey("dbo.Localities", "CountyId", "dbo.Counties");
            DropForeignKey("dbo.InterestsUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.InterestsUsers", "InterestId", "dbo.Interests");
            DropForeignKey("dbo.Friendships", "IdSender", "dbo.Users");
            DropForeignKey("dbo.Friendships", "IdReceiver", "dbo.Users");
            DropForeignKey("dbo.IdentityUserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Photos", "AlbumId", "dbo.Albums");
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "UserId" });
            DropIndex("dbo.Reactions", new[] { "UserId" });
            DropIndex("dbo.Reactions", new[] { "PostId" });
            DropIndex("dbo.IdentityUserLogins", new[] { "UserId" });
            DropIndex("dbo.Localities", new[] { "CountyId" });
            DropIndex("dbo.InterestsUsers", new[] { "UserId" });
            DropIndex("dbo.InterestsUsers", new[] { "InterestId" });
            DropIndex("dbo.Friendships", new[] { "IdReceiver" });
            DropIndex("dbo.Friendships", new[] { "IdSender" });
            DropIndex("dbo.IdentityUserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "LocalityId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropIndex("dbo.Posts", new[] { "UserId" });
            DropIndex("dbo.Photos", new[] { "Post_Id" });
            DropIndex("dbo.Photos", new[] { "User_Id" });
            DropIndex("dbo.Photos", new[] { "AlbumId" });
            DropIndex("dbo.Albums", new[] { "UserId" });
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.ErrorLogCustoms");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.Reactions");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.Counties");
            DropTable("dbo.Localities");
            DropTable("dbo.Interests");
            DropTable("dbo.InterestsUsers");
            DropTable("dbo.Friendships");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.Posts");
            DropTable("dbo.Photos");
            DropTable("dbo.Albums");
        }
    }
}
