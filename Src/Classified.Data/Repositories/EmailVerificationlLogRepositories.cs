using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Classified.Data.Infrastructure;
using Classified.Domain.Entities;

namespace Classified.Data.Repositories
{

    public class EmailVerificationlLogRepositories : RepositoryBase<EmailVerificationlLog>, IEmailVerificationlLogRepositories
    {

        public EmailVerificationlLogRepositories(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IEmailVerificationlLogRepositories : IRepository<EmailVerificationlLog>
    {


    }
}
