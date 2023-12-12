using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using Classified.Data.Advertisements.Advertisement;
using Classified.Domain.ViewModels.Advertisment;

namespace Classified.Services.DataCache.Advertisement.ClassifiedAds
{
    public static class ClassifiedAdsCache
    {
        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<func_FetchClassifiedAdvertisementsViewModel> ClassifiedAdsFullList
        {
            get
            {
                if (HttpContext.Current.Cache.Get("classifiedAdsList") != null)
                {
                    return HttpContext.Current.Cache.Get("classifiedAdsList") as IEnumerable<func_FetchClassifiedAdvertisementsViewModel>;
                }
                else
                {
                    //Fetch the information of categories and load it into the list
                    var tempList = new ClassifiedAdvertisementClientCore().FetchClassifiedAdvertisementsBasedOnRoot;
                    //Set the cache value and load data
                    HttpContext.Current.Cache.Insert("classifiedAdsList", tempList, null, DateTime.UtcNow.AddHours(24), Cache.NoSlidingExpiration);

                    //Return the list
                    return HttpContext.Current.Cache.Get("classifiedAdsList") as IEnumerable<func_FetchClassifiedAdvertisementsViewModel>;
                }
            }
            set => HttpContext.Current.Cache.Insert("classifiedAdsList  ", value, null, DateTime.UtcNow.AddHours(24), Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Get the list based on Category Id
        /// </summary>
        /// <param name="classifiedCategoryId">Classified Category Id</param>
        /// <returns></returns>
        public static IEnumerable<func_FetchClassifiedAdvertisementsViewModel> GetClassifiedAdsCategoryList(int classifiedCategoryId)
        {

            //Create the Name
            var classifiedAdsList = $"classifiedAdsList{classifiedCategoryId}";


            if (HttpContext.Current.Cache.Get(classifiedAdsList) != null)
            {
                return HttpContext.Current.Cache.Get(classifiedAdsList) as IEnumerable<func_FetchClassifiedAdvertisementsViewModel>;
            }
            else
            {
                //Fetch the information of categories and load it into the list
                var tempList = new ClassifiedAdvertisementClientCore().FetchClassifiedAdvertisementsBasedOnCategory(classifiedCategoryId);
                //Set the cache value and load data
                HttpContext.Current.Cache.Insert(classifiedAdsList, tempList, null, DateTime.UtcNow.AddHours(24), Cache.NoSlidingExpiration);

                //Return the list
                return HttpContext.Current.Cache.Get(classifiedAdsList) as IEnumerable<func_FetchClassifiedAdvertisementsViewModel>;
            }
        }

        public static bool SetClassifiedAdsCategoryList(int classifiedCategoryId)
        {
            try
            {
                var classifiedAdsList = $"classifiedAdsList{classifiedCategoryId}";
                var tempList = new ClassifiedAdvertisementClientCore().FetchClassifiedAdvertisementsBasedOnCategory(classifiedCategoryId);
                //Set the cache value and load data
                HttpContext.Current.Cache.Insert(classifiedAdsList, tempList, null, DateTime.UtcNow.AddHours(24), Cache.NoSlidingExpiration);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            

        }

    }
}
