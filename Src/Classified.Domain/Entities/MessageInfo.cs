using System;
using System.ComponentModel.DataAnnotations;

namespace Classified.Domain.Entities
{
    public class MessageInfo
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100000)]
        [Required]
        public string Body { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Sender_FullName { get; set; }

        [Required]
        [Display(Name = "Your Email Address")]
        public string Sender_EmailAddress { get; set; }


        public DateTime SentDate { get; set; }

        public bool IsRead { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
