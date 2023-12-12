namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addQueuedEmailToDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QueuedEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.String(nullable: false, maxLength: 200),
                        To = c.String(nullable: false),
                        CC = c.String(),
                        Bcc = c.String(),
                        Body = c.String(maxLength: 3000),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        SentOnUtc = c.DateTime(),
                        IsSent = c.Boolean(nullable: false),
                        Subject = c.String(),
                        SentTries = c.Int(),
                        FromName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.QueuedEmails");
        }
    }
}
