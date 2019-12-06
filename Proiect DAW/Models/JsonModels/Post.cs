using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_DAW.Models.JsonModels
{
    public class PostJsonModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ProfilePhoto { get; set; }
        public string Text { get; set; }
        public bool Liked { get; set; }
        public int NoReactions { get; set; }
        public List<int> PhotoId { get; set; }
        public bool IsMine { get; set; }
        public bool IsAdmin { get; set; }
    }
}
