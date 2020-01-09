using Proiect_DAW.Models.FeedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_DAW.Models.ProfileModels
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PostUserModel Admin { get; set; }
        public PhotoModel Photo { get; set; }
    }
}