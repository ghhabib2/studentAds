namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFirstNameAndLastNameAndCommentToUsetTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.AspNetUsers", "Comment", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Comment");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
