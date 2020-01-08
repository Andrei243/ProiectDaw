using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_DAW.Models.GeneralModels
{
    public class MessageBoxViewer
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int ProfilePhoto { get; set; }
        public string LastMessage { get; set; }
    }
}