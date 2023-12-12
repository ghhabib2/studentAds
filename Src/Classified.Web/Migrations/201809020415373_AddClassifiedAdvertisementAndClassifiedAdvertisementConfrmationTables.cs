namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassifiedAdvertisementAndClassifiedAdvertisementConfrmationTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassifiedAdvertisementConfirmations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProvidedToken = c.String(),
                        EmailAddress = c.String(),
                        ClassifiedAdvertisementId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassifiedAdvertisements", t => t.ClassifiedAdvertisementId, cascadeDelete: true)
                .Index(t => t.ClassifiedAdvertisementId);
            
            CreateTable(
                "dbo.ClassifiedAdvertisements",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        MetaDescription = c.String(),
                        MetaTitle = c.String(),
                        MetaKeyWord = c.String(),
                        ShortDescription = c.String(),
                        Description = c.String(),
                        Address = c.String(nullable: false),
                        PhoneNumber = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        SubmitDate = c.DateTime(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        UpdatedOnUtc = c.DateTime(nullable:true),
                        EmailAddress = c.String(nullable: false, maxLength: 200),
                        IsEmailConfirmed = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsEnhanced = c.Boolean(nullable: false),
                        ClassifiedCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassifiedCategories", t => t.ClassifiedCategoryId, cascadeDelete: true)
                .Index(t => t.ClassifiedCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClassifiedAdvertisementConfirmations", "ClassifiedAdvertisementId", "dbo.ClassifiedAdvertisements");
            DropForeignKey("dbo.ClassifiedAdvertisements", "ClassifiedCategoryId", "dbo.ClassifiedCategories");
            DropIndex("dbo.ClassifiedAdvertisements", new[] { "ClassifiedCategoryId" });
            DropIndex("dbo.ClassifiedAdvertisementConfirmations", new[] { "ClassifiedAdvertisementId" });
            DropTable("dbo.ClassifiedAdvertisements");
            DropTable("dbo.ClassifiedAdvertisementConfirmations");
        }
    }
}
