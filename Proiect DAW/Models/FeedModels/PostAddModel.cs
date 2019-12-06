using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_DAW.Models.FeedModels
{
    public class PostAddModel : IValidatableObject
    {
        public string Text { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile Binar { get; set; }
        public string Visibility { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (Text==null&&Binar==null)
                result.Add(new ValidationResult("You need at least some text or a photo to be able to post", new List<string>()));
            if (Binar != null && !Binar.ContentType.Contains("image"))
                result.Add(new ValidationResult("You can put only images in a post", new List<string> { nameof(Binar) }));
            return result;

        }
    }
}
