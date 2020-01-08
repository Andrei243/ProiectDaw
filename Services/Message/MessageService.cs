using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Message
{
    public class MessageService : Base.BaseService
    {
        public MessageService(SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public int AddMessage (string text, string ReceiverId, CurrentUser currentUser )
        {
            var user = unitOfWork.Users.Query.First(e => e.Id == currentUser.Id);
            if (user.IsBanned) return -1;

            var message = new Domain.Message()
            {
                Content = text,
                SentToId = ReceiverId,
                SentByUserId = currentUser.Id,
                SendingMoment = DateTime.Now
            };

            unitOfWork.Messages.Add(message);
            unitOfWork.SaveChanges();
            return message.Id;
        }

        public bool RemoveMessage (int MessageId)
        {
            var message = unitOfWork.Messages.Query
                .FirstOrDefault(e => e.Id == MessageId);

            if (message == null) return false;
            unitOfWork.Messages.Remove(message);
            return unitOfWork.SaveChanges() != 0;
        }

        public bool CanDeleteComment (int MessageId, CurrentUser currentUser)
        {
            if (currentUser.IsAdmin) return true;
            var isBanned = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id)?.IsBanned?? false;
            if (isBanned) return false;
            var message = unitOfWork.Messages.Query
                .First(e => e.Id == MessageId);
            if (message == null) return false;
            var bool1 = message.SentByUserId == currentUser.Id;
            if (bool1) return true;
            return false;

        }

        public List<Domain.Message> GetSentMessagesTo(int toSkip, int howMany, string currentUserId, string ReceiverId)
        {
            return unitOfWork.Messages.Query.OrderByDescending(e => e.SendingMoment)
                .Where(e => e.SentByUserId == currentUserId && !e.Sender.IsBanned)
                .Where(e => e.SentToId == ReceiverId && !e.Receiver.IsBanned)
                .OrderByDescending(e => e.SendingMoment)
                .Skip(toSkip)
                .Take(howMany)
                .ToList();
        }

        public List<Domain.Message> GetReceivedMessagesFrom(int toSkip, int howMany, string currentUserId, string SenderId)
        {
            return unitOfWork.Messages.Query.OrderByDescending(e => e.SendingMoment)
                .Where(e => e.SentToId == currentUserId && !e.Receiver.IsBanned)
                .Where(e => e.SentByUserId == SenderId && !e.Sender.IsBanned)
                .OrderByDescending(e => e.SendingMoment)
                .Skip(toSkip)
                .Take(howMany)
                .ToList();
        }

    }
}
