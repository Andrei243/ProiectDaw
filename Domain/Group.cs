using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public partial class Group:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AdminId { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<ApplicationUserGroup> Users { get; set; }
        public virtual ApplicationUser Admin { get; set; }
        public virtual Photo Photo { get; set; }

    }
}
