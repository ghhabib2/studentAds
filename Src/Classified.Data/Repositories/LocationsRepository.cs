

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using Classified.Data.Infrastructure;
using Classified.Domain.Entities;


namespace Classified.Data.Repositories
{
    public interface ILocationRepository : IRepository<Location>
    {

        bool IsLocationNameAvailabel(string name);
    }
    public class LocationRepository : RepositoryBase<Location>, ILocationRepository
    {
        private Location _contextcachedlocation;
        public LocationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public bool IsLocationNameAvailabel(string name)
        {
            var data = this.GetMany(x => x.Name == name).Any();
            return !data;
        }
        
    }
    public interface ICountryRepository : IRepository<Country>
    {

        bool IsCountryNameAvailabel(string name);
    }

    public class CountryRepository : RepositoryBase<Country>, ICountryRepository
    {


        public CountryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            
        }
        public bool IsCountryNameAvailabel(string name)
        {
            var data = this.GetMany(x => x.Name == name).Any();
            return !data;
        }
    }


    public interface ICurrencyRepository : IRepository<Currency>
    {

       
    }

    public class CurrencyRepository : RepositoryBase<Currency>, ICurrencyRepository
    {


        public CurrencyRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
      
    }
}
