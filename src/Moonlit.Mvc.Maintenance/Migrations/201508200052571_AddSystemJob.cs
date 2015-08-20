namespace Moonlit.Mvc.Maintenance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSystemJob : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemJobs",
                c => new
                    {
                        SystemJobId = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false, precision: 0),
                        HandlerType = c.String(maxLength: 300, storeType: "nvarchar"),
                        HandlerData = c.String(maxLength: 4000, storeType: "nvarchar"),
                        Name = c.String(maxLength: 1000, storeType: "nvarchar"),
                        Title = c.String(maxLength: 1000, storeType: "nvarchar"),
                        Category = c.Int(nullable: false),
                        ExecuteTime = c.DateTime(precision: 0),
                        Status = c.Int(nullable: false),
                        Result = c.String(maxLength: 4000, storeType: "nvarchar"),
                        CreationUserId = c.Int(),
                        CreationTime = c.DateTime(precision: 0),
                        UpdateUserId = c.Int(),
                        UpdateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.SystemJobId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemJobs");
        }
    }
}
