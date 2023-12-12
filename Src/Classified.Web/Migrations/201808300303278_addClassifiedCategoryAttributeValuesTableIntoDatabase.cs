namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addClassifiedCategoryAttributeValuesTableIntoDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassifiedCategoryAttributeValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttributeValue = c.String(nullable: false),
                        ClassifiedCategoryAttributeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassifiedCategoryAttributes", t => t.ClassifiedCategoryAttributeId, cascadeDelete: true)
                .Index(t => t.ClassifiedCategoryAttributeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClassifiedCategoryAttributeValues", "ClassifiedCategoryAttributeId", "dbo.ClassifiedCategoryAttributes");
            DropIndex("dbo.ClassifiedCategoryAttributeValues", new[] { "ClassifiedCategoryAttributeId" });
            DropTable("dbo.ClassifiedCategoryAttributeValues");
        }
    }
}
