using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_DAW.Models.FeedModels
{
    public class FeedModel
    {
        public List<PostModel> Posts { get; set; }
        public PostAddModel PostAdd { get; set; }
        public int CurrentPage { get; set; }


    }
}
