namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassifiedCategoryAttributesTabaleToDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassifiedCategoryAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttributeLabel = c.String(nullable: false),
                        AttributeName = c.String(nullable: false),
                        AttributeSearchBy = c.Int(nullable: false),
                        AttributeControlTypeId = c.Int(nullable: false),
                        ClassifiedCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassifiedCategories", t => t.ClassifiedCategoryId, cascadeDelete: true)
                .Index(t => t.ClassifiedCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClassifiedCategoryAttributes", "ClassifiedCategoryId", "dbo.ClassifiedCategories");
            DropIndex("dbo.ClassifiedCategoryAttributes", new[] { "ClassifiedCategoryId" });
            DropTable("dbo.ClassifiedCategoryAttributes");
        }
    }
}
