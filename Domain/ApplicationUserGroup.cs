using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public partial class ApplicationUserGroup:IEntity
    {
        public string UserId { get; set; }
        public int GroupId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }

    }
}
