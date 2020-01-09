using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_DAW.Models.ProfileModels
{
    public class MessageListModel
    {
        public string Id { get; set; }
        public List<MessageViewer> Messages { get; set; }
    }
}