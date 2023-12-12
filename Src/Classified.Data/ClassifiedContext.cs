using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Classified.Domain.Entities;

namespace Classified.Data
{
    public class ClassifiedContext : DbContext 
    {
        public DbSet<Location> Location { get; set; }
        public DbSet<ClassifiedAdvertisement> ClassifiedAds { get; set; }
        public DbSet<ClassifiedCategory> ClassifiedCategories { get; set; }
        public DbSet<ClassifiedImages> ClassifiedPictures { get; set; }
        public DbSet<ClassifiedComment> ClassifiedComments { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<ClassifiedCategoryAttribute> ClassifiedCategoryAttributes { get; set; }
        public DbSet<ClassifiedCategoryAttributeValue> AttributeValues { get; set; }
        public DbSet<ClassifiedAdvertisementAttribute> ClassifiedAttributes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<QueuedEmail> QueuedEmails { get; set; }
        public DbSet<TopView> TopViews { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<LocationCount> LocationCount { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<MyAccount> MyAccounts { get; set; }
        public DbSet<EmailVerificationlLog> EmailVerificationlLogs { get; set; }
        public DbSet<MessageInfo> MessageInfos { get; set; }
       
        public virtual void Commit()
        {
            this.SaveChanges();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasMany(u => u.Roles)
        //        .WithMany(r => r.Users)
        //        .Map(m =>
        //                 {
        //                     m.ToTable("RoleMemberships");
        //                     m.MapLeftKey("UserId");
        //                     m.MapRightKey("RoleName");
        //                 });

        //    modelBuilder.Entity<ClassifiedAd>()
        //        .HasMany(u => u.Locations)
        //        .WithMany(r => r.ClassifiedAds)
        //        .Map(m =>
        //                 {
        //                     m.ToTable("ClassifiedLocationsMappings");
        //                     m.MapLeftKey("ClassifiedId");
        //                     m.MapRightKey("LocationId");
        //                 });


        //    modelBuilder.Entity<ClassifiedAd>()
        //         .HasMany(u => u.Users)
        //        .WithMany(r => r.ClassifiedAds)
        //        .Map(m =>
        //        {
        //            m.ToTable("UserClassifiedMappings");
        //            m.MapLeftKey("ClassifiedId");
        //            m.MapRightKey("UserId");
        //        });

        //    modelBuilder.Entity<ClassifiedAd>()
        //         .HasMany(u => u.AttributeValues)
        //        .WithMany(r => r.ClassifiedAds)
        //        .Map(m =>
        //        {
        //            m.ToTable("ClassifiedAttributeValueMappings");
        //            m.MapLeftKey("Classified_Id");
        //            m.MapRightKey("Attr_Value_Id");
        //        });
        //}
    }
}
