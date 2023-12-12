namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategroiesTableToDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassifiedCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MetaDescription = c.String(maxLength: 300),
                        MetaTitle = c.String(maxLength: 60),
                        MetaKeyWord = c.String(maxLength: 120),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        Published = c.Boolean(nullable: false),
                        ShowOnHomePage = c.Boolean(nullable: false),
                        IsPriceShown = c.Boolean(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        UpdatedOnUtc = c.DateTime(),
                        ParentCategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassifiedCategories", t => t.ParentCategoryId)
                .Index(t => t.ParentCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClassifiedCategories", "ParentCategoryId", "dbo.ClassifiedCategories");
            DropIndex("dbo.ClassifiedCategories", new[] { "ParentCategoryId" });
            DropTable("dbo.ClassifiedCategories");
        }
    }
}
