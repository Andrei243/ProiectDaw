using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_DAW.Models.JsonModels
{
    public class InterestSelectJsonModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
        
        public static implicit operator InterestSelectJsonModel(string x)
        {
            var interest = new InterestSelectJsonModel();
            interest.Id = int.Parse(x);
            return interest;
        }

    }
}
