using Proiect_DAW.Models.FeedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_DAW.Models.ProfileModels
{
    public class MessageModel
    {
        public int Id { get; set; }
        public PostUserModel Sender { get; set; }
        public PostUserModel Receiver { get; set; }
        public GroupModel Group { get; set; }
        public string Content { get; set; }
    }
}