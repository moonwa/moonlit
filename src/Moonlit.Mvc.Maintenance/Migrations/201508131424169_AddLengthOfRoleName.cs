namespace Moonlit.Mvc.Maintenance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLengthOfRoleName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Roles", "Name", c => c.String(nullable: false, maxLength: 32, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("Roles", "Name", c => c.String(maxLength: 32, storeType: "nvarchar"));
        }
    }
}
