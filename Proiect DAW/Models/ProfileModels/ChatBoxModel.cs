using Proiect_DAW.Models.FeedModels;
using Services;
using System;
using System.Collections.Generic;

namespace Proiect_DAW.Models.ProfileModels
{
    public class ChatBoxModel
    {
        public string Call { get; set; }
        public PostUserModel SendToUser { get; set; }
        public string LastMessage { get; set; }
        public DateTime DateLastMessage { get; set; }
    }
}
