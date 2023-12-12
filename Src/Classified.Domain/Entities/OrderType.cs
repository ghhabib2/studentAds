using System.ComponentModel.DataAnnotations;

namespace Classified.Domain.Entities
{
    public class OrderType
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Duration { get; set; }
    }
}
