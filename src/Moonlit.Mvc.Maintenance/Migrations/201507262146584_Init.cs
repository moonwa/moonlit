namespace Moonlit.Mvc.Maintenance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cultures",
                c => new
                    {
                        CultureId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 32, storeType: "nvarchar"),
                        DisplayName = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CultureId);
            
            CreateTable(
                "dbo.CultureTexts",
                c => new
                    {
                        CultureTextId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 128, storeType: "nvarchar"),
                        CultureId = c.Int(nullable: false),
                        Text = c.String(maxLength: 4000, storeType: "nvarchar"),
                        IsEdited = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CultureTextId);
            
            CreateTable(
                "dbo.ExceptionLogs",
                c => new
                    {
                        ExceptionLogId = c.Int(nullable: false, identity: true),
                        Exception = c.String(maxLength: 8000, storeType: "nvarchar"),
                        RouteData = c.String(maxLength: 1280, storeType: "nvarchar"),
                        CreationTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.ExceptionLogId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 32, storeType: "nvarchar"),
                        Privileges = c.String(maxLength: 8000, storeType: "nvarchar"),
                        IsEnabled = c.Boolean(nullable: false),
                        IsBuildIn = c.Boolean(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.SystemSettings",
                c => new
                    {
                        SystemSettingId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100, storeType: "nvarchar"),
                        Category = c.String(maxLength: 100, storeType: "nvarchar"),
                        Value = c.String(maxLength: 4000, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.SystemSettingId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        LoginName = c.String(maxLength: 32, storeType: "nvarchar"),
                        IsEnabled = c.Boolean(nullable: false),
                        Password = c.String(maxLength: 128, storeType: "nvarchar"),
                        CultureId = c.Int(nullable: false),
                        Gender = c.Int(),
                        DateOfBirth = c.DateTime(precision: 0),
                        UserName = c.String(maxLength: 32, storeType: "nvarchar"),
                        IsSuper = c.Boolean(nullable: false),
                        Avatar = c.String(maxLength: 64, storeType: "nvarchar"),
                        IsBuildIn = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Roles", "User_UserId", "dbo.Users");
            DropIndex("dbo.Roles", new[] { "User_UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.SystemSettings");
            DropTable("dbo.Roles");
            DropTable("dbo.ExceptionLogs");
            DropTable("dbo.CultureTexts");
            DropTable("dbo.Cultures");
        }
    }
}
