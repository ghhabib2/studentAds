using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Classified.Domain.Entities
{
    public class QueuedEmail
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        public string From { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string To { get; set; }

        public string CC { get; set; }

        public string Bcc { get; set; }

        [AllowHtml]
        [StringLength(3000)]
        public string Body { get; set; }

        [Required]
        public DateTime CreatedOnUtc { get; set; }

        public DateTime? SentOnUtc { get; set; }

        public bool IsSent { get; set; }

        public string Subject { get; set; }

        public int? SentTries { get; set; }

        public string FromName { get; set; }

    }

}
