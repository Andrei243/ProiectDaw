using System;
using System.Collections.Generic;
using Common;

namespace Domain
{
    public partial class Friendship : IEntity
    {
        public string IdSender { get; set; }
        public string IdReceiver { get; set; }
        public bool? Accepted { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual User IdReceiverNavigation { get; set; }
        public virtual User IdSenderNavigation { get; set; }
    }
}
