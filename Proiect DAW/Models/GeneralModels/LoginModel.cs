using System.ComponentModel.DataAnnotations;


namespace Proiect_DAW.Models.GeneralModels
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public bool AreCredentialsInvalid { get; set; }
        public bool IsBanned { get; set; }
    }
}
