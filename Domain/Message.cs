using Common;
using System;

namespace Domain
{
    public partial class Message : IEntity
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public int? GroupId { get; set; }
        public string Content { get; set; }
        public DateTime SendingMoment { get; set; }

        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
        public virtual Group Group { get; set; }

    }
}
