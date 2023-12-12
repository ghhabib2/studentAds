using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classified.Domain.Entities.Dtos.AdimDtos
{
    public class CategoryAttributesDto
    {
        /// <summary>
        /// Primary key of each Attribute
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Attribute Label
        /// </summary>
        public string AttributeLabel { get; set; }
    }
}
