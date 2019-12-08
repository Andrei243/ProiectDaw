using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_DAW.Models.JsonModels
{
    public class ImageJsonModel
    {
        public int Id { get; set; }
        public int? AlbumId { get; set; }
        public string Description { get; set; }
    }
}
