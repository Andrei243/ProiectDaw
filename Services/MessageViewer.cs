﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MessageViewer
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        
        public bool IsMine { get; set; }
        public int? ProfilePhoto { get; set; }
        public DateTime DateTime { get; set; }

    }
}
