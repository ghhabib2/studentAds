namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class populateInitialDataForRolesAndAdmins : DbMigration
    {
        public override void Up()
        {
            Sql(@"  INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'05184916-b246-41f1-8363-be6cc2d76712', N'Admin', N'These users have the privilege to access all parts of the system.', N'ApplicationRole');
                    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'56ac3e36-ffa4-43ce-bdb0-d10e215f47ec', N'User', N'Default user of the system that just can add advertisements into the software and manage his/her account.', N'ApplicationRole');
                    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'dc35be9b-225b-4e5b-b670-03b96d09b9b7', N'AdvancedUser', N'The access level of these users is higher than the system''s default user. They can approve the newly added advertisements.', N'ApplicationRole');

                    INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [Comment]) VALUES (N'41482b5d-7f2d-4db5-9d18-05d94e1f55f8', N'admin@system.com', 1, N'ACtR8ayMwmsfkeP+Z96PHmSX61wrv03Tg4lEMbAnXIqn+wkgZLap+M7BxHGMRRr7DQ==', N'1a1bd8c9-1945-4c8b-848e-ada742eb49b3', NULL, 0, 0, NULL, 1, 0, N'admin@system.com', N'System', N'Admin', NULL);
                    
                    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'41482b5d-7f2d-4db5-9d18-05d94e1f55f8', N'05184916-b246-41f1-8363-be6cc2d76712')
                ");
        }

        public override void Down()
        {
        }
    }
}
