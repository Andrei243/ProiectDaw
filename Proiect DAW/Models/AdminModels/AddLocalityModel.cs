using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Proiect_DAW.Models.AdminModels
{
    public class AddLocalityModel
    {

        public List<SelectListItem> CountyIds { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CountyId { get; set; }
    }
}
