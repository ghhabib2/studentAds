using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Classified.Domain.Entities;
using Classified.Domain.ViewModels.Advertisment;
using Classified.Data.Base;

namespace Classified.Data.Advertisements.Categories
{
    /// <summary>
    /// Categories Core that Fetch the data from Database and send them to Controllers
    /// </summary>
    public class CategoriesCore:RepositoryBase<CategoryViewMoel, ClassifiedCategory>
    {
        
        
    }
}
