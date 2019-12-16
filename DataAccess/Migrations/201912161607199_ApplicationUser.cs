namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUser : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Users", newName: "ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.IdentityUserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.IdentityUserRoles", "UserId", "dbo.Users");
            DropIndex("dbo.IdentityUserClaims", new[] { "UserId" });
            DropIndex("dbo.IdentityUserLogins", new[] { "UserId" });
            DropIndex("dbo.IdentityUserRoles", new[] { "UserId" });
            AddColumn("dbo.IdentityUserClaims", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.IdentityUserLogins", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.IdentityUserRoles", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.IdentityUserClaims", "UserId", c => c.String());
            CreateIndex("dbo.IdentityUserClaims", "ApplicationUser_Id");
            CreateIndex("dbo.IdentityUserLogins", "ApplicationUser_Id");
            CreateIndex("dbo.IdentityUserRoles", "ApplicationUser_Id");
            AddForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.IdentityUserClaims", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.IdentityUserRoles", "ApplicationUser_Id");
            DropColumn("dbo.IdentityUserLogins", "ApplicationUser_Id");
            DropColumn("dbo.IdentityUserClaims", "ApplicationUser_Id");
            CreateIndex("dbo.IdentityUserRoles", "UserId");
            CreateIndex("dbo.IdentityUserLogins", "UserId");
            CreateIndex("dbo.IdentityUserClaims", "UserId");
            AddForeignKey("dbo.IdentityUserRoles", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.IdentityUserLogins", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.IdentityUserClaims", "UserId", "dbo.Users", "Id");
            RenameTable(name: "dbo.ApplicationUsers", newName: "Users");
        }
    }
}
