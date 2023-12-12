using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Classified.Data;
using Classified.Domain.Entities;
using Quartz;

namespace Classified.Services.Email
{
    public class EmailService : IJob
    {

        public const int NoOfMailsToSend = 100;
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public Task Execute(IJobExecutionContext context)
        {
            List<QueuedEmail> lstQueuedEmail = _context.QueuedEmail.Where(x => x.IsSent == false).Take(NoOfMailsToSend).ToList();
            if (lstQueuedEmail.Count > 0)
            {
                foreach (var queuedEmail in lstQueuedEmail)
                {
                    try
                    {
                        var emailSendStatus = EmailHelper.SendEmail(queuedEmail.From, queuedEmail.To, queuedEmail.Subject,
                            queuedEmail.Body, queuedEmail.FromName);
                        if (emailSendStatus)
                        {

                            queuedEmail.IsSent = true;
                            queuedEmail.SentOnUtc = DateTime.UtcNow;

                        }
                        else
                        {
                            queuedEmail.IsSent = false;
                            if (queuedEmail.SentTries == null)
                            {
                                queuedEmail.SentTries = 1;
                            }
                            else
                            {
                                queuedEmail.SentTries += 1;
                            }
                        }
                        _context.SaveChanges();

                    }
                    catch (Exception)
                    {
                        //log error 
                    }
                }
            }
            
            return null;
        }


    }
}
