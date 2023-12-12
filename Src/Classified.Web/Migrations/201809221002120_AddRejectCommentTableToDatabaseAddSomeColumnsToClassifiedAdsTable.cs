namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRejectCommentTableToDatabaseAddSomeColumnsToClassifiedAdsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassifiedAdvertisementRejectComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        ClassifiedAdvertisementId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassifiedAdvertisements", t => t.ClassifiedAdvertisementId, cascadeDelete: true)
                .Index(t => t.ClassifiedAdvertisementId);
            
            AddColumn("dbo.ClassifiedAdvertisements", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.ClassifiedAdvertisements", "IsSubmitted", c => c.Boolean(nullable: false));
            AddColumn("dbo.ClassifiedAdvertisements", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.ClassifiedAdvertisements", "IsRejected", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClassifiedAdvertisements", "SubmitDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClassifiedAdvertisementRejectComments", "ClassifiedAdvertisementId", "dbo.ClassifiedAdvertisements");
            DropIndex("dbo.ClassifiedAdvertisementRejectComments", new[] { "ClassifiedAdvertisementId" });
            AlterColumn("dbo.ClassifiedAdvertisements", "SubmitDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.ClassifiedAdvertisements", "IsRejected");
            DropColumn("dbo.ClassifiedAdvertisements", "IsApproved");
            DropColumn("dbo.ClassifiedAdvertisements", "IsSubmitted");
            DropColumn("dbo.ClassifiedAdvertisements", "ApprovedDate");
            DropTable("dbo.ClassifiedAdvertisementRejectComments");
        }
    }
}
