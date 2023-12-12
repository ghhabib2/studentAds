namespace Classified.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFucntionFunc_FetchClassifiedAdvertisementIntoDatabase : DbMigration
    {
        public override void Up()
        {
            Sql(@"-- =============================================
-- Author:		<Habib Ghaffari Hadigheh>
-- Create date: <10/04/2018>
-- Description:	<Fetch the information of the classified Advertisment based on the category Id>
-- =============================================
Create FUNCTION [dbo].[func_FetchClassifiedAdvertisements] 
(	
	-- Add the parameters for the function here
	@classifiedAdsCategoryId  int = null
)
RETURNS @classifiedAds TABLE 
(
	Id bigint,
	Title nvarchar(MAX),
	ShortDescription nvarchar(MAX),
	UpdatedOnUtc datetime,
	Price decimal(18,2),
	IsEnhanced bit,
	ImageName nvarchar(MAX),
	ClassifiedCategoryId int,
	ClassifiedCategoryName nvarchar(256)
)
AS
Begin

-- Declare tempTable for holding the information of the Table
	Declare @tempResultTable Table
	(
	Id bigint,
	Title nvarchar(MAX),
	ShortDescription nvarchar(MAX),
	UpdatedOnUtc datetime,
	Price decimal(18,2),
	IsEnhanced bit,
	ImageName nvarchar(MAX),
	ClassifiedCategoryId int,
	ClassifiedCategoryName nvarchar(256)
	)

-- Select the current Category information into the tempREsult Table
insert into @tempResultTable
(
	Id, 
	Title, 
	ShortDescription,
	UpdatedOnUtc,
	Price,
	IsEnhanced ,
	ClassifiedCategoryId,
	ClassifiedCategoryName 
	)
Select 
	dbo.ClassifiedAdvertisements.Id,
	dbo.ClassifiedAdvertisements.Title,
	dbo.ClassifiedAdvertisements.ShortDescription,
	dbo.ClassifiedAdvertisements.UpdatedOnUtc,
	dbo.ClassifiedAdvertisements.Price,
	dbo.ClassifiedAdvertisements.IsEnhanced,
	dbo.ClassifiedCategories.Id,
	dbo.ClassifiedCategories.Name
from dbo.ClassifiedAdvertisements
inner join dbo.ClassifiedCategories
on dbo.ClassifiedAdvertisements.ClassifiedCategoryId=dbo.ClassifiedCategories.Id
where
	dbo.ClassifiedCategories.Id=@classifiedAdsCategoryId
and
	dbo.ClassifiedAdvertisements.IsApproved=1
and
	dbo.ClassifiedAdvertisements.IsActive=1


-- Surf the temp table
While (select count(Id) from @tempResultTable) > 0
	begin
		
		-- Declare the variables need for image name
		declare @tempId as bigint;
		declare @imageName as nvarchar(MAX);
		set @tempId=(select top 1 Id from @tempResultTable);

		-- Check if there is a registered image exist for this ads

		if(select count(Id) from dbo.ClassifiedImages where ClassifiedAdvertisementId=@tempId)>0
			-- Select the very first image
			set @imageName = (select top 1 ImageName from dbo.ClassifiedImages where ClassifiedAdvertisementId=@tempId)
		else
			-- Select no AdsImage.png in the case there is no registered image
			set @imageName = 'noAdsImage.png';

		-- Insert the first record of the @tempResult Table into final Result Table With Image Name as value of @imageName
		insert into @classifiedAds
		(
			Id,
			Title,
			ShortDescription,
			UpdatedOnUtc,
			Price,
			IsEnhanced,
			ImageName,
			ClassifiedCategoryId,
			ClassifiedCategoryName
		)
		select top 1
			Id,
			Title,
			ShortDescription,
			UpdatedOnUtc,
			Price,
			IsEnhanced,
			@imageName,
			ClassifiedCategoryId,
			ClassifiedCategoryName
		from
			@tempResultTable

		--Delete the first result from @tempResult Table
		delete from @tempResultTable where Id=@tempId;

	end

	-- Declare another table as Veriable
	Declare @tempChildCategoryIds Table
	(
		Id int
	)

	-- Insert the List of Child Category Ids with this target category Id as their parent.
	if @classifiedAdsCategoryId is null
		begin
			insert into @tempChildCategoryIds(Id)
			select 
				dbo.ClassifiedCategories.Id 
			from 
				dbo.ClassifiedCategories 
			where 
				dbo.ClassifiedCategories.ParentCategoryId is null
		end
	else
		begin
			insert into @tempChildCategoryIds(Id)
			select 
				dbo.ClassifiedCategories.Id 
			from 
				dbo.ClassifiedCategories 
			where 
				dbo.ClassifiedCategories.ParentCategoryId=@classifiedAdsCategoryId
		end
	

	-- Check if there is registered child category Id exist for this target categoy Id
	if(select count(Id) from @tempChildCategoryIds)>0
		Begin
			While(select count(Id) from @tempChildCategoryIds)>0
				begin
					--	Declare variable for storing the information of child category Id
					declare @childcategoryId as int;
					-- Set the value of cild category Id as the top first record in ChildCategoryIds
					set @childcategoryId=(select top 1 Id from @tempChildCategoryIds);

					--Insert the result of child category into the result Table
					insert into @classifiedAds
					(
						Id,
						Title,
						ShortDescription,
						UpdatedOnUtc,
						Price,
						IsEnhanced,
						ImageName,
						ClassifiedCategoryId,
						ClassifiedCategoryName
					)
					select 
						Id,
						Title,
						ShortDescription,
						UpdatedOnUtc,
						Price,
						IsEnhanced,
						ImageName,
						ClassifiedCategoryId,
						ClassifiedCategoryName
					from
						dbo.func_FetchClassifiedAdvertisements(@childcategoryId)

					-- Delete top frist child from @tempChildCategoryIds
					delete from @tempChildCategoryIds where Id=@childcategoryId;
				end
		end
		

	Return
End
");
        }
        
        public override void Down()
        {
        }
    }
}
