using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_DAW.Models.DomainModels
{
    public class CountyDomainModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<LocalityDomainModel> Localities { get; set; }
    }
}
