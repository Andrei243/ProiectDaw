using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_DAW.Models.ProfileModels
{
    public class AddAlbumModel
    {
        [Required]
        public string Name { get; set; }
    }
}
