using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MessageBoxViewer
    {
        public string Call { get; set; }
        public string UserName { get; set; }
        public int? ProfilePhoto { get; set; }
        public string LastMessage { get; set; }
        public DateTime DateLastMessage { get; set; }
    }
}
