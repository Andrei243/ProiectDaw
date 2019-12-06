using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Proiect_DAW.Models.AdminModels
{
    public class EditLocalityModel
    {
        public List<SelectListItem> CountyIds { get; set; }
        [Required]
        public string Name { get; set; }
        public int CountyId { get; set; }
        [Required]
        public int Id { get; set; }
    }
}
