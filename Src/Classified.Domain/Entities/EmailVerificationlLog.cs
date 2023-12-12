using System;

namespace Classified.Domain.Entities
{
    public class EmailVerificationlLog
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string VerifiedId { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
