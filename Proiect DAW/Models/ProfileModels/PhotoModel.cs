using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Proiect_DAW.Models.ProfileModels
{
    public class PhotoModel : IValidatableObject
    {
        [Required]
        public HttpPostedFileBase Binar { get; set; }
        public int? AlbumId { get; set; }
        public int? PostId { get; set; }
        public int? Position { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (Binar != null && !Binar.ContentType.Contains("image"))
                result.Add(new ValidationResult("You can put only images in an album", new List<string> { nameof(Binar) }));
            return result;

        }
    }
}
