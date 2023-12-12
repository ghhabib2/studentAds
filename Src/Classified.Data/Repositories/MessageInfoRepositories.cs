using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Classified.Data.Infrastructure;
using Classified.Domain.Entities;

namespace Classified.Data.Repositories
{

    public class MessageInfoRepositories : RepositoryBase<MessageInfo>, IMessageInfoRepositories
    {
        
        public MessageInfoRepositories(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        
        }
        public static int GetNewMessageByUserId(string userId)
        {
            
            var _context = new ApplicationDbContext();

            // ------------------------- This part is going to be activated later -------------------------------------
            //var msg = _context.MessageInfos.Count(c => c.ApplicationUserId == userId);
            //var msg =
            //    _context.MessageInfos.Where(x => x.UserId == userId && x.IsRead == false).OrderByDescending(
            //        x => x.SentDate).Count();
            //_context.Dispose();

            //return msg;
            return 0;
        }
    }
    public interface IMessageInfoRepositories : IRepository<MessageInfo>
    {

       
    }
}
