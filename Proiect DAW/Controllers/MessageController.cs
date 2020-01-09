using DataAccess;
using Domain;
using Proiect_DAW.Code.Base;
using Proiect_DAW.Models.ProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_DAW.Controllers
{
    public class MessageController : BaseController
    {
        private readonly Services.Message.MessageService messageService;
        private readonly Services.User.UserService userService;

        public MessageController() : base()
        {
            messageService = new Services.Message.MessageService(new SocializRUnitOfWork(new SocializRContext()));
            userService = new Services.User.UserService(new SocializRUnitOfWork(new SocializRContext()));
            PageSize = 10;
        }

        [HttpGet]

        public ActionResult OpenChatBox(string UserName)
        {
            MakeCurrentUser();
            var userId = userService.GetUsersByName(UserName).First().Id;

            var sentMessages = messageService.GetSentMessagesTo(currentUser.Id, userId);
            var receivedMessages = messageService.GetReceivedMessagesFrom(currentUser.Id, userId);
            List<Domain.Message> allMessages = new List<Domain.Message>();
            sentMessages.AddRange(receivedMessages);
            sentMessages.OrderByDescending(e => e.SendingMoment);

            return View(sentMessages);
        }

        [HttpPost]
        public ActionResult SendMessage (MessageModel message)
        {
            MakeCurrentUser();
            if(ModelState.IsValid)
            {
                messageService.AddMessage(message.Content, message.Receiver.Id, currentUser);

                return RedirectToAction("OpenChatBox", "MessageContoller", message.Receiver.Name);
            }
            return RedirectToAction("ChatBox", "ProfileController");
        }
    }
}