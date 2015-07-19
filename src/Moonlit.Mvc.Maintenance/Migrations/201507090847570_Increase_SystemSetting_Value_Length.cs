namespace Moonlit.Mvc.Maintenance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Increase_SystemSetting_Value_Length : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CultureTexts", "IsEdited", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SystemSettings", "Value", c => c.String(maxLength: 4000, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SystemSettings", "Value", c => c.String(maxLength: 1000, storeType: "nvarchar"));
            DropColumn("dbo.CultureTexts", "IsEdited");
        }
    }
}
