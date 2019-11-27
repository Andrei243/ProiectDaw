using System;
using System.Collections.Generic;
using Common;

namespace Domain
{
    public partial class Role : IEntity
    {
        public Role()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
