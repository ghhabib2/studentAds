using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Classified.Data;
using Classified.Data.Advertisements.Categories;
using Classified.Domain.Entities;
using Classified.Domain.Entities.Dtos.AdimDtos;
using Classified.Domain.ViewModels.Advertisment;

namespace Classified.Web.Controllers.Api
{
    /// <summary>
    /// API for Category Attributes 
    /// </summary>
    public class CategoryAttributesController : ApiController
    {


        /// <summary>
        /// Response to get request from this API with the Address /api/CategoryAttributes and return all the information
        /// </summary>
        /// <returns>Return List as CategoryAttributesDto</returns>
        public IEnumerable<CategoryAttributesDto> GetCategoryAttributes()
        {
            return new CategoryAttributesCore()
                .GetMany(c => c.AttributeControlTypeId == (int)AttributeControlType.DropdownList || c.AttributeControlTypeId == (int)AttributeControlType.RadioList)
                .ToList().Select(Mapper.Map<CategoryAttributesViewModel, CategoryAttributesDto>);

        }

        /// <summary>
        /// Response to get request from this API with the address //Get /api/CategoryAttributes/{} and return the list filtered by categoryId
        /// </summary>
        /// <param name="id">CategoryId</param>
        /// <returns></returns>
        public IEnumerable<CategoryAttributesDto> GetFilteredAttribues(int id)
        {
            return new CategoryAttributesCore()
                .GetMany(c =>
                    c.ClassifiedCategoryId == id &&
                    (c.AttributeControlTypeId == (int)AttributeControlType.DropdownList ||
                     c.AttributeControlTypeId == (int)AttributeControlType.RadioList))
                .ToList().Select(Mapper.Map<CategoryAttributesViewModel, CategoryAttributesDto>);
        }
    }
}



//namespace ControllerWithMultipleGetMethods.Controllers
//{
//    [Route("api/[controller]")] /* this is the default prefix for all routes, see line 20 for overriding it */
//    public class ValuesController : Controller
//    {
//        [HttpGet] // this api/Values
//        public string Get()
//        {
//            return string.Format("Get: simple get");
//        }

//        [Route("GetByAdminId")] /* this route becomes api/[controller]/GetByAdminId */
//        public string GetByAdminId([FromQuery] int adminId)
//        {
//            return $"GetByAdminId: You passed in {adminId}";
//        }

//        [Route("/someotherapi/[controller]/GetByMemberId")] /* note the / at the start, you need this to override the route at the controller level */
//        public string GetByMemberId([FromQuery] int memberId)
//        {
//            return $"GetByMemberId: You passed in {memberId}";
//        }

//        [HttpGet]
//        [Route("IsFirstNumberBigger")] /* this route becomes api/[controller]/IsFirstNumberBigger */
//        public string IsFirstNumberBigger([FromQuery] int firstNum, int secondNum)
//        {
//            if (firstNum > secondNum)
//            {
//                return $"{firstNum} is bigger than {secondNum}";
//            }
//            return $"{firstNum} is NOT bigger than {secondNum}";
//        }
//    }
//}

