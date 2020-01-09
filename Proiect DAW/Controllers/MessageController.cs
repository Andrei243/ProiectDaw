using DataAccess;
using Domain;
using Proiect_DAW.Code.Base;
using Proiect_DAW.Models.ProfileModels;
using Services;
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
        public ActionResult ChatBox(string UserName)
        {
            MakeCurrentUser();
            var userId = userService.GetUsersByName(UserName).First().Id;

            var sentMessages = messageService.GetSentMessagesTo(currentUser.Id, userId);
            var receivedMessages = messageService.GetReceivedMessagesFrom(currentUser.Id, userId);
            //List<Domain.Message> allMessages = new List<Domain.Message>();
            sentMessages.AddRange(receivedMessages);
            sentMessages.OrderByDescending(e => e.SendingMoment);

            return View(messageService.GetMessageBoxes(currentUser));
        }

        [HttpGet]
        public ActionResult MessageUser(string id)
        {
            MakeCurrentUser();
            ViewBag.SendTo = id;
            var sentMessages = messageService.GetSentMessagesTo(currentUser.Id, id);
            var receivedMessages = messageService.GetReceivedMessagesFrom(currentUser.Id, id);
            //List<Domain.Message> allMessages = new List<Domain.Message>();
            sentMessages.AddRange(receivedMessages);
            sentMessages.OrderByDescending(e => e.SendingMoment);
            var allMessages = sentMessages.Select(e => new MessageViewer()
            {
                Content = e.Content,
                DateTime = e.SendingMoment,
                Id = e.Id,
                IsMine = e.SenderId == currentUser.Id,
                Name = e.Sender.Name + " " + e.Sender.Surname,
                ProfilePhoto = e.Sender.PhotoId
            });

            return View(new MessageListModel()
            {
                Id = id,
                Messages=allMessages.ToList()
            }) ;
        }
        [HttpGet]
        public ActionResult MessageGroup(int id)
        {
            MakeCurrentUser();

            var messages = messageService.GetMessagesGroup(id, currentUser);

            return View(messages);
        }



        [HttpPost]
        public ActionResult SendMessage(MessageModel message)
        {
            MakeCurrentUser();
            if (ModelState.IsValid)
            {
                messageService.AddMessage(message.Content, message.Receiver.Id, currentUser);

                return RedirectToAction("OpenChatBox", "MessageContoller", message.Receiver.Name);
            }
            return RedirectToAction("ChatBox", "ProfileController");
        }

        [HttpPost]
        public JsonResult SendMessageToPerson(string message, string userId)
        {
            MakeCurrentUser();
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { succeded = false }, JsonRequestBehavior.AllowGet);
            }
            var succeded = messageService.SendMessageToPerson(message, userId, currentUser);
            var response = succeded != -1;
            return Json(new { succeded = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendMessageToGroup(string message,int? groupId)
        {
            MakeCurrentUser();
            if (groupId == 0)
            {
                return Json(new { succeded = false }, JsonRequestBehavior.AllowGet);
            }
            var succeded = messageService.SendMessageToGroup(message, groupId.Value, currentUser);
            var response = succeded != -1;
            return Json(new { succeded = response }, JsonRequestBehavior.AllowGet);
        }
    }
}