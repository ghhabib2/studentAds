namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class populateClassifiedCategoryAttributesTableInitialData : DbMigration
    {
        public override void Up()
        {
            Sql(@"SET IDENTITY_INSERT [dbo].[ClassifiedCategoryAttributes] ON
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (7, 39, N'Breed', N'Breed', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (9, 50, N'Height', N'Height', 3, 2)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (10, 50, N'Body Type', N'Body_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (11, 50, N'Ethnicity', N'Ethnicity', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (13, 50, N'Age', N'Age', 4, 2)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (14, 40, N'Programme', N'Programme', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (17, 53, N'Type of car', N'Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (18, 53, N'Year', N'Year', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (19, 66, N'Model', N'Model', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (20, 67, N'Model', N'Model', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (21, 53, N'Condition', N'Condition', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (23, 57, N'Bathrooms', N'Bathrooms', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (25, 57, N'Square Meters', N'Square_Meters', 4, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (26, 145, N'Breed', N'Breed', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (27, 146, N'Breed', N'Breed', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (28, 147, N'Breed', N'Breed', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (29, 148, N'Breed', N'Breed', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (30, 149, N'Breed', N'Breed', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (31, 150, N'Breed', N'Breed', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (32, 151, N'Breed', N'Breed', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (33, 161, N'Accessories Type', N'Accessories_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (34, 162, N'Type', N'Binoculars_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (35, 163, N'Type', N'Camcorders_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (36, 164, N'Type', N'Digital_camera_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (37, 165, N'Type', N'Flim_Camera', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (38, 166, N'Type', N'Lenses_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (39, 167, N'Type', N'Spy_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (40, 168, N'Type', N'Telescope_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (42, 169, N'Type', N'Vintage_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (43, 170, N'Type', N'CDsAccessories_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (44, 171, N'Type', N'Cassettes_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (45, 172, N'Type', N'Cds_type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (46, 173, N'Style', N'Record_style', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (47, 178, N'Type', N'Wmn_Footwear', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (48, 170, N'Type', N'Clth_Acc_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (49, 175, N'Type', N'Men_Type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (50, 176, N'Type', N'Men_footer_type', 1, 1)
                    INSERT INTO [dbo].[ClassifiedCategoryAttributes] ([Id], [ClassifiedCategoryId], [AttributeLabel], [AttributeName], [AttributeControlTypeId], [AttributeSearchBy]) VALUES (51, 177, N'Type', N'Women_Type', 1, 1)
               SET IDENTITY_INSERT [dbo].[ClassifiedCategoryAttributes] OFF
                ");
        }
        
        public override void Down()
        {
        }
    }
}
