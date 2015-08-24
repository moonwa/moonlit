namespace Moonlit.Mvc.Maintenance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserLoginFailLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLoginFailedLogs",
                c => new
                    {
                        UserLoginFailedLogId = c.Long(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        IpAddress = c.String(maxLength: 32, storeType: "nvarchar"),
                        CreationTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.UserLoginFailedLogId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLoginFailedLogs", "UserId", "dbo.Users");
            DropIndex("dbo.UserLoginFailedLogs", new[] { "UserId" });
            DropTable("dbo.UserLoginFailedLogs");
        }
    }
}
