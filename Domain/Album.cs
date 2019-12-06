using System;
using System.Collections.Generic;
using Common;

namespace Domain
{
    public partial class Album : IEntity
    {
        public Album()
        {
            Photo = new HashSet<Photo>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Photo> Photo { get; set; }
    }
}
