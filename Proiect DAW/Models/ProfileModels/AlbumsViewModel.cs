using Proiect_DAW.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_DAW.Models.ProfileModels
{
    public class AlbumsViewModel
    {
        public int Id { get; set; }
        public AddAlbumModel AddAlbumModel { get; set; }

        public List<AlbumDomainModel> Album { get; set; }

        public bool CanEdit { get; set; }
    }
}
