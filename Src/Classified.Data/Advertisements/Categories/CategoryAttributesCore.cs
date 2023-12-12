using System.Collections.Generic;
using Classified.Domain.Entities;
using Classified.Domain.ViewModels.Advertisment;
using Classified.Data.Base;

namespace Classified.Data.Advertisements.Categories
{
    /// <summary>
    /// Core for sending data into and fetching information from Database related to ClassifiedCategoryAttributes DataTable
    /// </summary>
    public class CategoryAttributesCore:RepositoryBase<CategoryAttributesViewModel, ClassifiedCategoryAttribute>
    {
        
    }
}
