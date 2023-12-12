using System.ComponentModel.DataAnnotations;

namespace Classified.Domain.Entities
{
   public class Country
   {
       [Key]
       public int Id { get; set; }

       public string TwoLetterIsoCode { get; set; }

       public string Name { get; set; }

       public string ThreeLetterIsoCode { get; set; }

       public int NumericIsoCode { get; set; }

       public int DisplayOrder { get; set; }

       //public virtual ICollection<Currency> Currencies { get; set; }
   }

   public class Currency
   {
       [Key]
       public int Id { get; set; }

       public string ISO_Code { get; set; }

       public string Currency_Name { get; set; }

       public string Currency_Sign { get; set; }

       public int CountryId { get; set; }

       public Country Country { get; set; }

   }
    
}
