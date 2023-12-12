using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Classified.Component.Html;
using Classified.Data.Advertisements.Advertisement;
using Classified.Data.Advertisements.Categories;
using Classified.Domain.ViewModels;
using Classified.Domain.ViewModels.Advertisment;
using Classified.Domain.ViewModels.Client;
using Classified.Domain.ViewModels.Client.AboutUs;
using Classified.Services.DataCache.Advertisement.Categories;
using Classified.Services.DataCache.Advertisement.ClassifiedAds;

namespace Classified.Web.Controllers
{
    public class HomeController : Controller
    {

        #region Index Page related Actions and Methods

        public ActionResult Index()
        {
            var tempResult = new ClassifiedAdvertisementClientCore().FetchTop30;

            var fetchClassifiedAdvertisementsViewModels = tempResult as func_FetchClassifiedAdvertisementsViewModel[] ?? tempResult.ToArray();
            var funcFetchClassifiedAdvertisementsViewModels = tempResult as func_FetchClassifiedAdvertisementsViewModel[] ?? fetchClassifiedAdvertisementsViewModels.ToArray();
            foreach (var item in funcFetchClassifiedAdvertisementsViewModels)
            {
                item.DateString = item.UpdatedOnUtc.ToShortDateString();
                item.CategoryUrl = Url.RouteUrl("CategoryHome", new { id = (int)item.ClassifiedCategoryId });
                item.AdsUrl = Url.RouteUrl("AdsDisplay", new { id = (long)item.Id });
                item.PriceString = item.Price > 0 ? $"{item.Price:N0}" : "No Price";
            }


            var tempModel = new ClassifiedAdvertisementListClientViewModel()
            {
                AdsList = fetchClassifiedAdvertisementsViewModels
            };

            ViewBag.Message = "Home";

            return View(tempModel);
        }

        #endregion



        #region About Us  Related Actions and Methods

        [Route("About", Name = "About-Us Page")]
        public ActionResult About()
        {
            ViewBag.Message = "About Us";

            var tempModel = new AboutUsViewModel { BreadcrumbList = PopulateAboutUsBreadcrumb() };

            return View(tempModel);
        }

        /// <summary>
        /// Populate the about Us Breadcrumb
        /// </summary>
        /// <returns>List related to the bread crumb of about Us</returns>
        protected internal IEnumerable<BreadcrumbViewModel> PopulateAboutUsBreadcrumb()
        {
            var tempResult = new List<BreadcrumbViewModel>
            {
                //Add Home Route
                new BreadcrumbViewModel {LInkUrl = Url.Action("Index"), LinkName = "Home"},
                //Add About Us
                new BreadcrumbViewModel {LinkName = "About Us"}
            };

            //Return the Breadcrumb list
            return tempResult;
        }

        #endregion

        #region Contact Us related Actions and Methods

        [Route("Contact", Name = "Contact-Us Page")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Us";

            var tempModel = new ContactUsViewModel { BreadcrumbList = PopulateContactUsBreadcrumb() };

            return View(tempModel);
        }

        /// <summary>
        /// Populate the about Us Breadcrumb
        /// </summary>
        /// <returns>List related to the bread crumb of about Us</returns>
        protected internal IEnumerable<BreadcrumbViewModel> PopulateContactUsBreadcrumb()
        {
            var tempResult = new List<BreadcrumbViewModel>
            {
                //Add Home Route
                new BreadcrumbViewModel {LInkUrl = Url.Action("Index"), LinkName = "Home"},
                //Add Contact Us
                new BreadcrumbViewModel {LinkName = "Contact Us"}
            };

            //Return the Breadcrumb list
            return tempResult;
        }

        #endregion

        #region Advertisement Related Actions

        [Route("Categories", Name = "CategoriesHome")]
        public ActionResult Categories()
        {
            ViewBag.Message = "Categories";
            var tempViewModel = new ClassifiedAdvertisementListClientViewModel
            {
                CategoryList = HirarchyList(CategoriesCache.CategoryList.ToList()),
                RecordCount = ClassifiedAdsCache.ClassifiedAdsFullList.Count(),
                SelectedClassifiedCategoryCategoryId = string.Empty,
                SelectedClassifiedCategoryName = string.Empty,
                BreadcrumbList = PopulateCategoriesBreadcrumb()
            };
            return View(tempViewModel);
        }

        [Route("Categories/{id:int}", Name = "CategoryHome")]
        public ActionResult Categoy(int id)
        {
            ViewBag.Message = "Category";

            //Get the Category Name


            var tempViewModel = new ClassifiedAdvertisementListClientViewModel
            {
                CategoryList = HirarchyList(CategoriesCache.CategoryList.ToList()),
                RecordCount = ClassifiedAdsCache.GetClassifiedAdsCategoryList(id).Count(),
                SelectedClassifiedCategoryCategoryId = id.ToString(),
                SelectedClassifiedCategoryName = CategoriesCache.CategoryList.SingleOrDefault(item => item.Id == id)?.Name,
                BreadcrumbList = PopulateCategoriesBreadcrumb(id)
            };

            return View("Categories", tempViewModel);
        }


        /// <summary>
        /// Get the List of Categories and convert it to Presentable Hierarchy List for View Purpose
        /// </summary>
        /// <param name="basicList"></param>
        /// <returns></returns>
        protected internal IEnumerable<CategoryItemClientViewMoel> HirarchyList(List<CategoryViewMoel> basicList)
        {


            //Select the Parent Nodes.
            var categoryViewMoels = basicList.ToList();
            var parentNodes = categoryViewMoels.Where(c => c.ParentCategory == null).ToList();

            var outputList = new List<CategoryItemClientViewMoel>();

            foreach (var category in parentNodes)
            {

                //Check for children nodes
                if (categoryViewMoels.Count(c => c.ParentCategoryId == category.Id) > 0)
                {
                    //Add the Item to the List

                    var tempItem = new CategoryItemClientViewMoel()
                    {
                        Name = category.Name,
                        Id = category.Id.ToString(),
                        Url = Url.RouteUrl("CategoryHome", new { id = category.Id })
                    };

                    //Add the children to the list
                    AppendChildren(ref tempItem, ref basicList);

                    outputList.Add(tempItem);

                }
                else
                {

                    var tempItem = new CategoryItemClientViewMoel()
                    {
                        Name = category.Name,
                        Id = category.Id.ToString(),
                        ChildCategories = new List<CategoryItemClientViewMoel>()
                    };

                    outputList.Add(tempItem);

                }
            }

            return outputList;
        }

        /// <summary>
        /// Add the child nodes to the list of parent nodes
        /// </summary>
        /// <param name="parentItem">Parent Node</param>
        /// <param name="basicList"></param>
        protected internal void AppendChildren(ref CategoryItemClientViewMoel parentItem, ref List<CategoryViewMoel> basicList)
        {

            //Assign Parent Category to a local variable to use
            var parentCategoryId = Convert.ToInt32(parentItem.Id);

            //Fetch the Child List
            var childList = basicList.Where(c => c.ParentCategoryId == parentCategoryId).ToList();


            foreach (var categoryItem in childList)
            {
                //Check for children nodes
                if (basicList.Count(c => c.ParentCategoryId == parentCategoryId) > 0)
                {

                    var tempItem = new CategoryItemClientViewMoel
                    {
                        Name = categoryItem.Name,
                        Id = categoryItem.Id.ToString(),
                        Url = Url.RouteUrl("CategoryHome", new { id = categoryItem.Id })
                    };

                    AppendChildren(ref tempItem, ref basicList);

                    parentItem.Id = (-1).ToString();
                    parentItem.ChildCategories.Add(tempItem);

                }
                else
                {

                    var tempItem = new CategoryItemClientViewMoel
                    {
                        Name = categoryItem.Name,
                        Id = categoryItem.Id.ToString(),
                        ChildCategories = new List<CategoryItemClientViewMoel>()
                    };

                    parentItem.ChildCategories.Add(tempItem);
                }
            }
        }

        /// <summary>
        /// Bread Crumb for Categories Page
        /// </summary>
        /// <returns></returns>
        protected internal IEnumerable<BreadcrumbViewModel> PopulateCategoriesBreadcrumb()
        {
            var tempResult = new List<BreadcrumbViewModel>
            {
                //Add Home Route
                new BreadcrumbViewModel {LInkUrl = Url.Action("Index"), LinkName = "Home"},
                //Add About Us
                new BreadcrumbViewModel {LinkName = "Categories"}
            };

            //Return the Breadcrumb list
            return tempResult;
        }

        /// <summary>
        /// Populate the breadcrumb of the specific categories Page
        /// </summary>
        /// <param name="classifiedCategoryId">Classified Category Id</param>
        /// <returns></returns>
        protected internal IEnumerable<BreadcrumbViewModel> PopulateCategoriesBreadcrumb(int classifiedCategoryId)
        {

            var finalResult = new List<BreadcrumbViewModel>
            {

                //Add Home
                new BreadcrumbViewModel {LinkName = "Home", LInkUrl = Url.Action("Index", "Home")},

                //Add Categories
                new BreadcrumbViewModel {LinkName = "Categories", LInkUrl = Url.RouteUrl("CategoriesHome")}
            };


            //Create a list of categories and their parents
            //=====================================================
            var tempParentsList = new List<CategoryItemViewMoel>();
            ParentCategoryLoad(classifiedCategoryId, ref tempParentsList);

            //Check if there is a record in parents list
            if (tempParentsList.Any())
            {
                foreach (var categoryItem in tempParentsList.ToArray().Reverse())
                {
                    //Add Categories
                    finalResult.Add(new BreadcrumbViewModel
                    {
                        LinkName = categoryItem.Name,
                        LInkUrl = Url.RouteUrl("CategoryHome", new { id = categoryItem.Id })
                    });
                }
            }

            //Find the current Category in the List
            var tempCurrentCategory =
                CategoriesCache.CategoryList.SingleOrDefault(item => item.Id == classifiedCategoryId);

            //Add Current Category
            if (tempCurrentCategory != null)
                finalResult.Add(new BreadcrumbViewModel{LinkName = tempCurrentCategory.Name});

            return finalResult;
        }

        /// <summary>
        /// Load the parent categories into the list
        /// </summary>
        /// <param name="categoryId">CategoryId</param>
        /// <param name="parentsList">ParentList</param>
        /// <returns></returns>
        protected internal void ParentCategoryLoad(int categoryId, ref List<CategoryItemViewMoel> parentsList)
        {
            //load categories list
            var categoiresList = CategoriesCache.CategoryList;

            //Find the Category in the List
            var tempCurrentCategory = categoiresList.SingleOrDefault(item => item.Id == categoryId);

            //Find the parent information
            if (tempCurrentCategory?.ParentCategoryId != null)
            {
                var tempCurrentParentCategory =
                    categoiresList.SingleOrDefault(item => item.Id == tempCurrentCategory.ParentCategoryId);

                parentsList.Add(new CategoryItemViewMoel { Id = tempCurrentParentCategory.Id, Name = tempCurrentParentCategory.Name });

                if (tempCurrentParentCategory.ParentCategoryId.HasValue)
                {
                    //Reload the current parent categories parent
                    ParentCategoryLoad(tempCurrentParentCategory.Id, ref parentsList);
                }
            }

        }

        #endregion

        #region Search Related Actions and Methods


        public ActionResult FullTextSearch(string id)
        {
            return View();
        }

        #endregion



    }
}
