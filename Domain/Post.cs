using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Domain
{
    public partial class Post : IEntity
    {
        public Post()
        {
            Comment = new HashSet<Comment>();
            Reaction = new HashSet<Reaction>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        [Column("Vizibilitate")]
        public string Confidentiality { get; set; }
        public DateTime AddingMoment { get; set; }
        public string Text { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual ICollection<Reaction> Reaction { get; set; }
    }
}
