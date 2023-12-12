using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Classified.Data.Advertisements.Categories;
using Classified.Domain.ViewModels.Advertisment;

namespace Classified.Services.DataCache.Advertisement.Categories
{
    /// <summary>
    /// Cache the categories Information
    /// </summary>
    public static class CategoriesCache
    {
        public static IEnumerable<CategoryViewMoel> CategoryList
        {
            get
            {
                if (HttpContext.Current.Cache.Get("categoryList") != null)
                {
                    return HttpContext.Current.Cache.Get("categoryList") as IEnumerable<CategoryViewMoel>;
                }
                else
                {
                    //Fetch the information of categories and load it into the list
                    var tempList = new CategoriesCore().GetAll();
                    //Set the cache value and load data
                    HttpContext.Current.Cache.Insert("categoryList",tempList,null,DateTime.UtcNow.AddHours(24),Cache.NoSlidingExpiration);

                    //Return the list
                    return HttpContext.Current.Cache.Get("categoryList") as IEnumerable<CategoryViewMoel>;
                }
            }
            set => HttpContext.Current.Cache.Insert("categoryList", value, null, DateTime.UtcNow.AddHours(24), Cache.NoSlidingExpiration);
        }
    }
}
