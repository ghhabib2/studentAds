namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTheCategoriesActiveAndPublishedStatusToTrue : DbMigration
    {
        public override void Up()
        {
            //Change the status of Categories Active and Published Columns values to True
            Sql("Update [dbo].[ClassifiedCategories] Set IsActive='True',Published='True'");
        }
        
        public override void Down()
        {
        }
    }
}
