﻿using System;
using System.Collections.Generic;
using Common;

namespace Domain
{
    public partial class InterestsUsers : IEntity
    {
        public int InterestId { get; set; }
        public string UserId { get; set; }

        public virtual Interest Interest { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
