
using System.ComponentModel.DataAnnotations;

namespace Classified.Domain.Entities
{
    public class LocationCount
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public int Count { get; set; }
    }
}
