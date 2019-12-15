using System;
using System.Collections.Generic;
using Common;

namespace Domain
{
    public partial class Comment : IEntity
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime AddingMoment { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
