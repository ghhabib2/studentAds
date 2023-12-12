namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDescriptionToRoleTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "Description", c => c.String());
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String());
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropColumn("dbo.AspNetRoles", "Description");
        }
    }
}
