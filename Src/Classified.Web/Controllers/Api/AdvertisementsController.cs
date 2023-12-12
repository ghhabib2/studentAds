using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using Classified.Domain.ViewModels.Advertisment;
using Classified.Services.DataCache.Advertisement.ClassifiedAds;

namespace Classified.Web.Controllers.Api
{
    public class AdvertisementsController : ApiController
    {

        /// <summary>
        /// Get the list of All Advertisements based on the paging
        /// </summary>
        /// <param name="page">Page of the advertisements</param>
        /// <returns></returns>
        [Route("api/Content/{page:int}/",Name = "AllAdvertisementsListAPI")]
        [HttpGet]
        public IEnumerable<func_FetchClassifiedAdvertisementsViewModel> GetAllAdervtisementsList(int page)
        {
            var tempResult= ClassifiedAdsCache.ClassifiedAdsFullList.Skip(page * 9).Take(9);

            var funcFetchClassifiedAdvertisementsViewModels = tempResult as func_FetchClassifiedAdvertisementsViewModel[] ?? tempResult.ToArray();
            foreach (var item in funcFetchClassifiedAdvertisementsViewModels)
            {
                item.DateString = item.UpdatedOnUtc.ToShortDateString();
                item.CategoryUrl = Url.Link("CategoryHome", new {id = (int) item.ClassifiedCategoryId});
                item.AdsUrl = Url.Link("AdsDisplay", new {id = (long) item.Id});
                item.PriceString = item.Price > 0 ? $"{item.Price:N0}" : "No Price";
            }

            return funcFetchClassifiedAdvertisementsViewModels;
        }

        /// <summary>
        /// Get the list of All Advertisements based on the paging and category of Advertisements
        /// </summary>
        /// <param name="categoryId">Advertisements Category</param>
        /// <param name="page">Target Page</param>
        /// <returns></returns>
        [Route("api/Content/{categoryId:int}/{page:int}/", Name = "CategoryBasedAdvertisementsListAPI")]
        [HttpGet]
        public IEnumerable<func_FetchClassifiedAdvertisementsViewModel> GetAdervtisementsListBasedOnCategory(
            int categoryId, int page)
        {

            var tempResult = ClassifiedAdsCache.GetClassifiedAdsCategoryList(categoryId).Skip(page * 9).Take(9);

            var funcFetchClassifiedAdvertisementsViewModels = tempResult as func_FetchClassifiedAdvertisementsViewModel[] ?? tempResult.ToArray();
            foreach (var item in funcFetchClassifiedAdvertisementsViewModels)
            {
                item.DateString = item.UpdatedOnUtc.ToShortDateString();
                item.CategoryUrl = Url.Link("CategoryHome", new { id = (int)item.ClassifiedCategoryId });
                item.AdsUrl = Url.Link("AdsDisplay", new { id = (long)item.Id });
                item.PriceString = item.Price > 0 ? $"{item.Price:N0}" : "No Price";
            }

            return tempResult;
        }

    }
}
