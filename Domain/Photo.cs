using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Domain
{
    public partial class Photo : IEntity
    {
        public int Id { get; set; }
        [Column("Binar")]
        public byte[] Binary { get; set; }
        public string MIMEType { get; set; }
        public int? AlbumId { get; set; }
        public int? PostId { get; set; }
        public int? Position { get; set; }
        public int? GroupId { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }

        public virtual Album Album { get; set; }
        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Group Group {get;set;}
    }
}
