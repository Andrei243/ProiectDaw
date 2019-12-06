using Proiect_DAW.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_DAW.Models.ProfileModels
{
    public class AlbumViewerModel
    {
        
        public int Id { get; set; }
        public PhotoModel PhotoModel { get; set; }
        public bool HasThisAlbum { get; set; }
        
        public string Name { get; set; }
    }
}
