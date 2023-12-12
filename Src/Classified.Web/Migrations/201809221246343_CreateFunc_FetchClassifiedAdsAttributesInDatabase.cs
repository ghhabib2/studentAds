namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateFunc_FetchClassifiedAdsAttributesInDatabase : DbMigration
    {
        public override void Up()
        {
            Sql(@"--    Function that fetch the information related to Attributes from database
                   CREATE FUNCTION [dbo].[func_FetchClassifiedAdsAttributes]
                    (
	                    @classifiedAdvertisementId bigint)
                    RETURNS @returntable TABLE
                    (
	                    Id bigint,
	                    AttributeLabel nvarchar(MAX),
	                    AttributeValue nvarchar(MAX)
                    )
                    AS
                    BEGIN
                        --	Insert data into the declared variable
                       insert into @returntable
	                    select	dbo.ClassifiedAdvertisementAttributes.Id as 'Id',
			                    dbo.ClassifiedCategoryAttributes.AttributeLabel as 'AttributeLabel',
			                    dbo.ClassifiedAdvertisementAttributes.[Value] as 'AttributeValue'
	                    from dbo.ClassifiedAdvertisementAttributes inner join dbo.ClassifiedCategoryAttributes
	                    on dbo.ClassifiedAdvertisementAttributes.ClassifiedCategoryAttributeId=dbo.ClassifiedCategoryAttributes.Id
	                    where dbo.ClassifiedAdvertisementAttributes.ClassifiedAdvertisementId=@classifiedAdvertisementId
	                       
	                    RETURN
                    END"
            );
        }
        
        public override void Down()
        {
        }
    }
}
