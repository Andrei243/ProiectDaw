using System;
using System.Collections.Generic;
using Common;

namespace Domain
{
    public partial class Locality : IEntity
    {
        public Locality()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public int CountyId { get; set; }
        public string Name { get; set; }

        public virtual County County { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
