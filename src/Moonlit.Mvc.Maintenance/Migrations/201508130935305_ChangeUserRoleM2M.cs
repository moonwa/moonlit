namespace Moonlit.Mvc.Maintenance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserRoleM2M : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Roles", "User_UserId", "Users");
            DropIndex("Roles", new[] { "User_UserId" });
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            DropColumn("dbo.Roles", "User_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Roles", "User_UserId", c => c.Int());
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropTable("dbo.UserRoles");
            CreateIndex("dbo.Roles", "User_UserId");
            AddForeignKey("dbo.Roles", "User_UserId", "dbo.Users", "UserId");
        }
    }
}
