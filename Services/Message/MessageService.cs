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
                ReceiverId = ReceiverId,
                SenderId = currentUser.Id,
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

        public bool CanDeleteMessage (int MessageId, CurrentUser currentUser)
        {
            if (currentUser.IsAdmin) return true;
            var isBanned = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id)?.IsBanned?? false;
            if (isBanned) return false;
            var message = unitOfWork.Messages.Query
                .First(e => e.Id == MessageId);
            if (message == null) return false;
            var bool1 = message.SenderId == currentUser.Id;
            if (bool1) return true;
            return false;

        }

        public List<Domain.Message> GetSentMessagesTo( string currentUserId, string ReceiverId)
        {
            return unitOfWork.Messages.Query.OrderByDescending(e => e.SendingMoment)
                .Where(e => e.SenderId == currentUserId && !e.Sender.IsBanned)
                .Where(e => e.ReceiverId == ReceiverId && !e.Receiver.IsBanned)
                .OrderByDescending(e => e.SendingMoment)
                .ToList();
        }

        public List<Domain.Message> GetReceivedMessagesFrom( string currentUserId, string SenderId)
        {
            return unitOfWork.Messages.Query.OrderByDescending(e => e.SendingMoment)
                .Where(e => e.ReceiverId == currentUserId && !e.Receiver.IsBanned)
                .Where(e => e.SenderId == SenderId && !e.Sender.IsBanned)
                .OrderByDescending(e => e.SendingMoment)
                .ToList();
        }

        public List<MessageBoxViewer> GetMessageBoxes(CurrentUser currentUser)
        {
            var userId = currentUser.Id;
            var groups = unitOfWork.ApplicationUserGroups.Query.Where(e => e.UserId == userId).Select(e => e.GroupId);
            var messages = unitOfWork.Messages.Query.Where(e => e.ReceiverId == userId || e.SenderId == userId || groups.Any(f => f == e.GroupId))
                .Select(e =>
                new MessageBoxViewer()
                {
                    DateLastMessage = e.SendingMoment,
                    LastMessage = e.Content,
                    ProfilePhoto = e.GroupId != null ? null : e.ReceiverId == userId ? e.Sender.PhotoId : e.Receiver.PhotoId,
                    Call = e.GroupId != null ? "/Messages/MessageGroup?id=" + e.GroupId.ToString() : "/Messages/MessageUser?id=" + (e.ReceiverId == userId ? e.Sender.Id : e.Receiver.Id),
                    UserName = e.GroupId != null ? e.Group.Name : e.ReceiverId == userId ? e.Sender.Name + " " + e.Sender.Surname : e.Receiver.Name + " " + e.Receiver.Surname
                });
            messages = messages.GroupBy(e => e.Call).Select(e => e.OrderByDescending(f => f.DateLastMessage).FirstOrDefault());
            return messages.ToList();
        }

    }
}
