namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsEmailBasedToClassifiedAdvertisementTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassifiedAdvertisements", "IsEmailBase", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClassifiedAdvertisements", "IsEmailBase");
        }
    }
}
