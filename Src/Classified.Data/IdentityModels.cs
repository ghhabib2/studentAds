using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Classified.Domain.Entities;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.SqlClient;

namespace Classified.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        /// <summary>
        /// Queued Email Database Set. We use this table to store the information of emails that must be sent later.
        /// </summary>
        public DbSet<QueuedEmail> QueuedEmail { get; set; }

        /// <summary>
        /// Used for storing the information of Categories in Database
        /// </summary>
        public DbSet<ClassifiedCategory> ClassifiedCategories { get; set; }

        /// <summary>
        /// Use for storing the information of each category attributes
        /// </summary>
        public DbSet<ClassifiedCategoryAttribute> ClassifiedCategoryAttributes { get; set; }

        /// <summary>
        /// Use for storing the information of values related to attributes with kinds like Drop-down and Radio-button List
        /// </summary>
        public DbSet<ClassifiedCategoryAttributeValue> ClassifiedCategoryAttributeValues { get; set; }

        /// <summary>
        /// Use for storing the information of Classified Advertisements submitted by users
        /// </summary>
        public DbSet<ClassifiedAdvertisement> ClassifiedAdvertisements { get; set; }

        /// <summary>
        /// Use for storing the information of Classified Advertisements Confirmation Data when it is needed to be confirmed.
        /// </summary>
        public DbSet<ClassifiedAdvertisementConfirmation> ClassifiedAdvertisementConfirmations { get; set; }

        /// <summary>
        /// Use for storing the information of Classified Advertisement Attributes Data
        /// </summary>
        public DbSet<ClassifiedAdvertisementAttribute> ClassifiedAdvertisementAttributes { get; set; }

        /// <summary>
        /// Store the information of images related to the Advertisement
        /// </summary>
        public DbSet<ClassifiedImages> ClassifiedImageses { get; set; }

        /// <summary>
        /// Store the information of reject comments written by Admin users
        /// </summary>
        public DbSet<ClassifiedAdvertisementRejectComment> AdvertisementRejectComments { get; set; }

        /// <summary>
        /// Function for fetching information of Attributes related to classified Advertisements  
        /// </summary>
        /// <param name="classifiedAdvertisementId">Classified Advertisement Id</param>
        /// <returns>The list of Attributes and their values</returns>
        [DbFunction("ApplicationDbContext", "func_FetchClassifiedAdsAttributes")]
        public IQueryable<FunctionClassifiedAdvertisementAttribute> Func_ClassifiedAdvertisementAttributes(
            long classifiedAdvertisementId)
        {
            //Define a object parameter
            var funcClassifiedAdvertisementId =
                new SqlParameter("classifiedAdvertisementId", classifiedAdvertisementId);

            return this.Database.SqlQuery<FunctionClassifiedAdvertisementAttribute>("SELECT * FROM [dbo].[func_FetchClassifiedAdsAttributes](@classifiedAdvertisementId)",
        funcClassifiedAdvertisementId).AsQueryable();
            
        }

        /// <summary>
        /// Function for fetching information of Attributes related to classified Advertisements  
        /// </summary>
        /// <returns>The list of TOP30 Advertisements based on their promotion order</returns>
        [DbFunction("ApplicationDbContext", "func_FetchClassifiedAdvertisementsTOP30")]
        public IQueryable<func_FetchClassifiedAdvertisements> Func_FetchClassifiedAdvertisementsTOP30()
        {

            return this.Database.SqlQuery<func_FetchClassifiedAdvertisements>("SELECT * FROM [dbo].[func_FetchClassifiedAdvertisementsTOP30]() order by UpdatedOnUtc desc").AsQueryable();


        }

        /// <summary>
        /// Function for fetching information of Attributes related to classified Advertisements  
        /// </summary>
        /// <returns>The list of All Advertisements based on their promotion order</returns>
        [DbFunction("ApplicationDbContext", "func_FetchClassifiedAdvertisements")]
        public IQueryable<func_FetchClassifiedAdvertisements> Func_FetchClassifiedAdvertisements()
        {
            
            return this.Database.SqlQuery<func_FetchClassifiedAdvertisements>("SELECT * FROM [dbo].[func_FetchClassifiedAdvertisements](DEFAULT) order by UpdatedOnUtc desc").AsQueryable();

            
        }

        /// <summary>
        /// Function for fetching information of Attributes related to classified Advertisements  
        /// </summary>
        /// <param name="classifiedAdsCategoryId">Classified Advertisement Category Id</param>
        /// <returns>The list of Advertisements related to the selected Category and its children based on their promotion order</returns>
        [DbFunction("ApplicationDbContext", "func_FetchClassifiedAdvertisements")]
        public IQueryable<func_FetchClassifiedAdvertisements> Func_FetchClassifiedAdvertisements(
            int classifiedAdsCategoryId)
        {
            //Define a object parameter
            var funcClassifiedAdvertisementId =
                new SqlParameter("classifiedAdsCategoryId", classifiedAdsCategoryId);

            return this.Database.SqlQuery<func_FetchClassifiedAdvertisements>("SELECT * FROM [dbo].[func_FetchClassifiedAdvertisements](@classifiedAdsCategoryId) order by UpdatedOnUtc desc",
                funcClassifiedAdvertisementId).AsQueryable();

           
        }

        /*
                public DbSet<MessageInfo> MessageInfos { get; set; }
        */

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}