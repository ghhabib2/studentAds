namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyKeyofClassifiedAdvertisementAttributesFromIntToBigInt : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ClassifiedAdvertisementAttributes");
            AlterColumn("dbo.ClassifiedAdvertisementAttributes", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.ClassifiedAdvertisementAttributes", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ClassifiedAdvertisementAttributes");
            AlterColumn("dbo.ClassifiedAdvertisementAttributes", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ClassifiedAdvertisementAttributes", "Id");
        }
    }
}
