using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_DAW.AdminModels
{
    public class EditCountyModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
