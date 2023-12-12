using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Classified.Data.Infrastructure;
using Classified.Domain.Entities;

namespace Classified.Data.Repositories
{
    public class CategoryAttributeRepository : RepositoryBase<ClassifiedCategoryAttribute>, ICategoryAttributeRepository
    {
        public CategoryAttributeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public bool IsAttrNameAvailabel(string name)
        {
            var user = this.GetMany(x => x.AttributeName == name).Any();
            return !user;
        }

    }
    public interface ICategoryAttributeRepository : IRepository<ClassifiedCategoryAttribute>
    {
        bool IsAttrNameAvailabel(string name);

    }
    public class AttributeValueRepository : RepositoryBase<ClassifiedCategoryAttributeValue>, IAttributeValueRepository
    {
        public AttributeValueRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IAttributeValueRepository : IRepository<ClassifiedCategoryAttributeValue>
    {


    }
    public class ClasifiedAttributeRepository : RepositoryBase<ClassifiedAdvertisementAttribute>, IClasifiedAttributeRepository
    {
        public ClasifiedAttributeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IClasifiedAttributeRepository : IRepository<ClassifiedAdvertisementAttribute>
    {


    }
}
