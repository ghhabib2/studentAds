namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sampleFinalUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetRoles", "Description", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetRoles", "Description", c => c.String());
        }
    }
}
