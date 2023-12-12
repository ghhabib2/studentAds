namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassifiedImageTableToDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassifiedImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageName = c.String(),
                        ImageGuid = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        UploadedOnUtc = c.DateTime(nullable: false),
                        ClassifiedAdvertisementId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassifiedAdvertisements", t => t.ClassifiedAdvertisementId, cascadeDelete: true)
                .Index(t => t.ClassifiedAdvertisementId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClassifiedImages", "ClassifiedAdvertisementId", "dbo.ClassifiedAdvertisements");
            DropIndex("dbo.ClassifiedImages", new[] { "ClassifiedAdvertisementId" });
            DropTable("dbo.ClassifiedImages");
        }
    }
}
