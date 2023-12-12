namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdvertisementAttributesTableToDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassifiedAdvertisementAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        ClassifiedCategoryAttributeId = c.Int(nullable: false),
                        ClassifiedAdvertisementId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassifiedAdvertisements", t => t.ClassifiedAdvertisementId, cascadeDelete: true)
                .ForeignKey("dbo.ClassifiedCategoryAttributes", t => t.ClassifiedCategoryAttributeId, cascadeDelete: false)
                .Index(t => t.ClassifiedCategoryAttributeId)
                .Index(t => t.ClassifiedAdvertisementId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClassifiedAdvertisementAttributes", "ClassifiedCategoryAttributeId", "dbo.ClassifiedCategoryAttributes");
            DropForeignKey("dbo.ClassifiedAdvertisementAttributes", "ClassifiedAdvertisementId", "dbo.ClassifiedAdvertisements");
            DropIndex("dbo.ClassifiedAdvertisementAttributes", new[] { "ClassifiedAdvertisementId" });
            DropIndex("dbo.ClassifiedAdvertisementAttributes", new[] { "ClassifiedCategoryAttributeId" });
            DropTable("dbo.ClassifiedAdvertisementAttributes");
        }
    }
}
