using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Classified.Data.Infrastructure;
using Classified.Domain.Entities;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;



namespace Classified.Data.Repositories
{
    public class ClassifiedAdCategoryRepository : RepositoryBase<ClassifiedCategory>, IClassifiedAdCategoryRepository
    {
        public ClassifiedAdCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }


    }
    public class ClassifiedAdRepository : RepositoryBase<ClassifiedAdvertisement>, IClassifiedRepository
    {
        private ApplicationDbContext _context;
        
        private readonly ILocationRepository _locationRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        public ClassifiedAdRepository(IDatabaseFactory databaseFactory, ILocationRepository locationRepository, IAttributeValueRepository attributeValueRepository)
            : base(databaseFactory)
        {
            //Open Connection to Database
            _context = new ApplicationDbContext();

            _locationRepository = locationRepository;
            _attributeValueRepository = attributeValueRepository;

        }

        public IEnumerable<Location> Locations()
        {
            var data = _locationRepository.GetAll();
            return data;
        }
        public IEnumerable<ClassifiedCategoryAttributeValue> AttributeValues()
        {
            var data = _attributeValueRepository.GetAll();
            return data;
        }
        public void Assignlocations(int id, int locations)
        {
            //var classified = this.GetById(id);
            //if (classified.Locations != null)
            //{
            //    classified.Locations.Clear();
            //}
            //else
            //{
            //    classified.Locations = new List<Location>();
            //}

            //var location = this.DataContext.Location.Find(locations);
            //    classified.Locations.Add(location);
           

            //this.DataContext.ClassifiedAds.Attach(classified);
            //this.DataContext.Entry(classified).State = EntityState.Modified;
            this.DataContext.SaveChanges();
        }

        public void AssignUser(long id, string userId)
        {
            var userClassifiedMapping = new UserClassifiedMapping
            {
                ApplicationUserId = userId,
                ClassifiedAdId = id
            };

            //--------------- This part is going to be active later ---------------------
            //_context.UserClassifiedMappings.Add(userClassifiedMapping);

            //_context.SaveChanges();

            //-------------------------------------------------------------------------

            ////var currentUser = this.GetById(id);
            //if (currentUser.Users != null)
            //{
            //    currentUser.Users.Clear();
            //}
            //else
            //{
            //    currentUser.Users = new List<User>();
            //}

            //var user = this.DataContext.Users.Find(userId);
            //currentUser.Users.Add(user);


            //this.DataContext.ClassifiedAds.Attach(currentUser);
            //this.DataContext.Entry(currentUser).State = EntityState.Modified;
            //this.DataContext.SaveChanges();
        }

        public void AssignAttributeVaues(long Id, IEnumerable<int> attributes)
        {
            //var classified = this.GetById(id);
            //if (classified.AttributeValues != null)
            //{
            //    classified.AttributeValues.Clear();
            //}
            //else
            //{
            //    classified.AttributeValues = new List<AttributeValue>();
            //}

            //--------------------  This part is going to be activated later    ----------------------------

            //foreach (var item in attributes)
            //{
            //    var data = _context.ClassifiedAttributeValueMappings.Add(
            //        new ClassifiedAttributeValueMapping {ClassifiedAdId = Id, AttributeValueId = item});
                
            //}

            //_context.SaveChanges();


            //----------------------------------------------------------------------------------------------

            //this.DataContext.ClassifiedAds.Attach(classified);
            //this.DataContext.Entry(classified).State = EntityState.Modified;
            //this.DataContext.SaveChanges();


        }

        public List<ClassifiedAdvertisement> GetClassifiedAdsByLocationCategoryAttributes(string categoryId, string andParam,string locationId)
        {
           // var sql = "SearchByAttributes @attrubuteID='" + andParam + "', @categoryId='" + categoryId + "'";
            var data = DataContext.ClassifiedAds.SqlQuery("GetClassifiedAdsByLocationCategoryAttributes @attrubuteID='" + andParam + "', @categoryId='" + categoryId + "',@locationId='" + locationId + "'").ToList();
            
            return data;
        }
        public List<ClassifiedAdvertisement> GetClassifiedByUserId(int userId)
        {
            var data = DataContext.ClassifiedAds.SqlQuery("Sp_ClassifiedByUserId @userId='" + userId + "'").ToList();
            return data;
        }
        public List<ClassifiedAdvertisement> ClassifiedInActiveByUserId(int userId)
        {
            var data = DataContext.ClassifiedAds.SqlQuery("Sp_ClassifiedInActiveByUserId @userId='" + userId + "'").ToList();
            return data;
        }
        


        public List<ClassifiedAdvertisement> SearchByTextValue(string textvalue,string categoryid,string locationId)
        {
            textvalue = textvalue.Replace("'", "''");
            var sql = " exec SearchByText @TextValue='" + textvalue + "',@categoryId='" + categoryid + "',@locationId='" + locationId + "'";
            var parameter = new SqlParameter("@TextValue", textvalue);
            var parameter2 = new SqlParameter { ParameterName = "@categoryId", Value = categoryid };
            var parameter3 = new SqlParameter { ParameterName = "@locationId", Value = locationId };
            var data = DataContext.ClassifiedAds.SqlQuery("SearchByText @TextValue,@categoryId,@locationId",parameter, parameter2, parameter3).ToList();
            return data;
        }

        

    }

    public class ClassifiedPictureRepositories : RepositoryBase<ClassifiedImages>, IClassifiedPictureRepositories
    {
        public ClassifiedPictureRepositories(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }

    public class ClassifiedCommentRepository : RepositoryBase<ClassifiedComment>, IClassifiedCommentRepository
    {
        public ClassifiedCommentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }


    }

    public interface IClassifiedCommentRepository : IRepository<ClassifiedComment>
    {


    }

    public interface IClassifiedAdCategoryRepository : IRepository<ClassifiedCategory>
    {


    }
    public interface IClassifiedRepository : IRepository<ClassifiedAdvertisement>
    {
        void Assignlocations(int id, int locations);
        IEnumerable<Location> Locations();
        void AssignUser(long id, string userId);
        void AssignAttributeVaues(long id, IEnumerable<int> attributes);
        IEnumerable<ClassifiedCategoryAttributeValue> AttributeValues();

        List<ClassifiedAdvertisement> GetClassifiedAdsByLocationCategoryAttributes(string categoryId, string andParam,
                                                                        string locationId);
        List<ClassifiedAdvertisement> SearchByTextValue(string textvalue, string categoryid, string locationId);
        List<ClassifiedAdvertisement> GetClassifiedByUserId(int userId);
        List<ClassifiedAdvertisement> ClassifiedInActiveByUserId(int userId);

    }

    public interface IClassifiedPictureRepositories : IRepository<ClassifiedImages>
    {


    }


}