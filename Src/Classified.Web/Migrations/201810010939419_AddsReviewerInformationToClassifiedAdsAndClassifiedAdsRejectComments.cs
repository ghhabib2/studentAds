namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsReviewerInformationToClassifiedAdsAndClassifiedAdsRejectComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassifiedAdvertisementRejectComments", "RejectedByUser", c => c.String(maxLength: 128));
            AddColumn("dbo.ClassifiedAdvertisements", "ReviewedByUser", c => c.String(maxLength: 128));
            AddColumn("dbo.ClassifiedAdvertisements", "HasReviewPriority", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClassifiedAdvertisements", "HasReviewPriority");
            DropColumn("dbo.ClassifiedAdvertisements", "ReviewedByUser");
            DropColumn("dbo.ClassifiedAdvertisementRejectComments", "RejectedByUser");
        }
    }
}
